﻿using UnityEngine;


public abstract class PowerUps : Objects
{
    public virtual void Effect() { Destroy(gameObject); }
    public override void CollisionEffect()
    {
        switch (PlayerResistance())
        {
            case 1: //action done if player has simple resistance
                Effect();
                break;

            case 2: //action done if player has time-based resistance
                Destroy(gameObject);
                break;

            default: //action done if player has no resistance
                Effect();
                break;
        }
    }

}

public abstract class Obstacle : Objects
{
    bool _counted = false;
    void Update()
    {
        if (!GameManager.characterDeath)
        {
            if ((transform.position.z < GameManager.Player.position.z) && (!_counted))
            {
                Debug.Log("sent");
                FindObjectOfType<GameManager>().ObstaclePassed();
                _counted = true;
            }
            Debug.Log(_counted);
        }
    }

    public virtual void Effect() 
    { if (Player.GetComponent<CharacterReact>()!= null) 
        { 
            Player.GetComponent<CharacterReact>().Die(true, true);
        }
    }
    public override void CollisionEffect()
    { 
        switch (PlayerResistance())
        {
            case 1: Player.GetComponentInChildren<CharacterReact>().Hit();
                Destroy(gameObject);
                break;

            case 2: //action done if player has time-based resistance
                Destroy(gameObject);
                break;

            default: //action done if player has no resistance
                Effect();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}

public abstract class Trigger : Objects
{
    public virtual void Effect() { Destroy(gameObject); }
    public virtual void BounceEffect() { }
    public override void CollisionEffect()
    {
        switch (PlayerResistance())
        {
            case 1: //action done if player has simple resistance
                Effect();
                break;

            case 2: //action done if player has time-based resistance
                BounceEffect();
                break;

            default: //action done if player has no resistance
                Effect();
                break;
        }
    }

}

public abstract class World : MonoBehaviour
{
    void Update()
    {
        if (!GameManager.characterDeath)
        {
            if (gameObject.transform.position.z < GameManager.Player.position.z - GameManager.DestroyDist)
            {
                Destroy(gameObject);
            }
        }

    }
}

public abstract class Objects : World
{

    public GameObject Player;

    void OnTriggerEnter(Collider collision) // All objects except obstacles,excluding hole
    { 

        Player = collision.gameObject;

        CollisionEffect();
    }

    public virtual int PlayerResistance()
    {
        if (Player.GetComponent<CharacterReact>() !=null)
        {
            return Player.GetComponent<CharacterReact>().PlayerResistance();
        }else
        { 
            return 0; 
        }
        
    }
    public virtual void CollisionEffect() { }


}




