﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_3_Manager : BossManager
{
    bool  _mayAnimate, _switchRotation;

    Boss_3_Animator animator;
    Circle_SpawnPositions _pos;
    Boss_3_CharacterMovement playerMovement;

    Transform _obj;

    float _rotationSpeed;

    int _playerRotations, _rotationsNeededForNextStage;

    static bool _clockwiseRotation;
    public static bool ClockwiseRotation { get { return _clockwiseRotation; } }


    public override void Start()
    {
        
        base.Start();
        animator = GetComponentInChildren<Boss_3_Animator>();

        gameSpawner.SetSpawnPoint(spawnPoint);

        gameSpawner.ForceBreak(2, 47);

        _rotationSpeed = 1;
        _playerRotations = 0;
        _rotationsNeededForNextStage = 3;

        coolOffTime = 0.35f;
        _clockwiseRotation = false;
        _pos = GetComponentInChildren<Circle_SpawnPositions>();

    }

    void Update()
    {
        if (!GameManager.characterDeath)
        {
            if (bossActive)
            {                
                if (_mayAnimate) //can the animator be called?
                {
                   animator.SetAnimationState(); // the animator is called
                   _mayAnimate = false; // the animator looses permission to be called
                }

                if ((mayAttack) && (!stopAttacking))
                {
                    _obj = gameSpawner.PickObject();

                    if (_obj != null)
                    {
                        if (_obj.gameObject.name == "1.BasicMoving")
                        {
                            Instantiate(_obj, _pos.MiddleLane(), Quaternion.Euler(_pos.Rotation()));
                        }
                        else
                        {
                            Instantiate(_obj, _pos.Spawnposition(), Quaternion.Euler(_pos.Rotation()));
                        }

                        StartCoroutine(CoolOff());
                    }
                }
            }
            else
            {
                if (CurrrentStage == 1)
                {
                    FetchPlayer();
                }
            }
        }else
        {
            base.DeactivateBoss();
            if (Player != null)
            {
                Player.GetComponent<CharacterMovement>().StopForwardMovement(false, true);
                Destroy(playerMovement);
            }
            
        }

        if (_switchRotation)
        {
            _rotationSpeed -= Time.deltaTime;

            if (_rotationSpeed < 0)
            {
                _switchRotation = false;
                _clockwiseRotation = !_clockwiseRotation;
                _rotationSpeed = 1;

                animator.SetTriggers();

                playerMovement.TurnAround();
                GetComponentInChildren<Boss_3_SpawnerMovement>().TurnAround();
            }
        }
    }

    public override void ActivateBoss()
    {
        mayAttack = true;
        Player.GetComponent<CharacterMovement>().StopForwardMovement(true, true); 
        Player.GetComponent<CharacterMovement>().SetLane(3);
        Player.transform.eulerAngles = new Vector3(0,90,0);

        Player.gameObject.AddComponent<Boss_3_CharacterMovement>();

        playerMovement = Player.GetComponent<Boss_3_CharacterMovement>();

        gameSpawner.RemoveHole();
    }


    public override void DeactivateBoss()
    {
        Destroy(playerMovement);
        Player.transform.eulerAngles = new Vector3(0, 0, 0);
        
        Player.GetComponent<CharacterMovement>().StopForwardMovement(false, true);
        Destroy(gameObject);
        
        base.DeactivateBoss();
    }

    public void MayAnimate()// the player triggers this class
    {
       
        if (CurrrentStage == 3)
        {
            animator.ActivatePlatform();
        }else
        {
            _mayAnimate = true; //the animator gets permission to be called
            _playerRotations++;
        }
    }

    public override void IncreaseStage()
    {
        if ((_playerRotations >= _rotationsNeededForNextStage) && !Boss_3_Animator.Animated)
        {
            base.IncreaseStage();

            if (CurrrentStage == 3)
            {
                EndBoss();
            }
            else
            {
                _rotationsNeededForNextStage += 3;
                _switchRotation = true;
            }
        }
    }

    public override void EndBoss()
    {
        if (_clockwiseRotation)
        {
            animator.FinalAnimation();
        }
        else
        {
            _switchRotation = true;
        }

        base.EndBoss(); 
    }

//    public void SpawnerNotGrounded()
//    {
//        stopAttacking = true;
//    }
//    public void SpawnerGrounded()
//    {
//        stopAttacking = false;
//    }
}
