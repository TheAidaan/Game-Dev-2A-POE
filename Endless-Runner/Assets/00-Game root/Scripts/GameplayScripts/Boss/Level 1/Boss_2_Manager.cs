﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2_Manager : BossManager
{
    Rigidbody _rb;
    Boss_2_Animator _animator;
    Boss_2_Gun _gun;

    bool _maySpawn,_moveForward, _attacking,_assessing;

    float _forwardSpeed, _totalDistance, _startPosition;
    int _increaseStagepoint;

    public override void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Boss_2_Animator>();
        _gun = GetComponentInChildren<Boss_2_Gun>();
        _rb = GetComponent<Rigidbody>();

        _startPosition = transform.position.z;
        _increaseStagepoint = 50;
        _maySpawn = true;

        coolOffTime = 1.4f;


    }
    void FixedUpdate()
    {
        if (_moveForward)
        {
            _rb.velocity = Vector3.forward * _forwardSpeed;
        }else
        {
            _rb.velocity = Vector3.zero;
        }
  
    }
    void Update()
    {
        if (!GameManager.characterDeath)
        {
            if (bossActive)
            {
                if (mayAttack&& !stopAttacking)
                {
                   Attack();
                }
                

                if (Player.transform.position.z > (spawnPoint - 15))
                {
                    _totalDistance = (transform.position.z - _startPosition);

                    if (_totalDistance > _increaseStagepoint)
                    {
                        IncreaseStage();
                        _increaseStagepoint += 50;
                    }
                    if (_maySpawn)
                    {
                        gameSpawner.SpawnBuildingBlocks(spawnPoint, gameSpawner.PickObject());
                        spawnPoint++;
                    }

                }
            }
            else
            {
                if (transform.position.z > (spawnPoint - 13))
                {
                    gameSpawner.SpawnBuildingBlocks(spawnPoint, null);
                    spawnPoint++;
                }
            }
        }else
        {
            _moveForward = false;
        }
    }

    public override void ActivateBoss()
    {
        mayAttack = true;
        _forwardSpeed = Player.GetComponent<CharacterMovement>().CurrentSpeed();
        _gun.Activate(Player);
        _moveForward = true;
        base.ActivateBoss();
    }

    void  Attack()
    {
        int RandNum = Random.Range(0, 10);

        if (RandNum<4)
        {
            StartCoroutine(CoolOff());
            stopAttacking = true;

            StartCoroutine(_animator.Smash());
            
        }
        else
        {
            if (RandNum > 7)
            {
                StartCoroutine(CoolOff());
                stopAttacking = true;

                _animator.Aim();
                StartCoroutine(_gun.Shoot());  
            }
            else
            {
                StartCoroutine(CoolOff());
            }
        }
        
    }
            

    public override void EndBoss()
    {
        gameSpawner.SpawnPlatform(spawnPoint, true);
        _maySpawn = false;
        _moveForward = false;
        Transform[] ActivationTriggers = Resources.LoadAll<Transform>("Prefabs/Boss/Triggers");

        for (int i = 0; i < 3; i++)
        {
            Instantiate(ActivationTriggers[0], new Vector3(Spawner.FirstLane + i, Spawner.WorldHeight + 1, spawnPoint + 1), ActivationTriggers[0].rotation);
        }

        base.EndBoss();
    }

    public override void DeactivateBoss()
    {
        base.DeactivateBoss();
        Destroy(gameObject);
    }

    public void StartAttacking()
    {
        stopAttacking = false;
    }


}
