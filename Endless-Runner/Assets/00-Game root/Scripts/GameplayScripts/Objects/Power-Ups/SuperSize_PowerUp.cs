﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSize_PowerUp : PowerUps
{
    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, 3f);
    }

    public override void Effect()
    {
        if (Player.GetComponent<CharacterReact>() != null)
        {
            Player.GetComponent<CharacterReact>().SuperSize();
        }
       
        base.Effect();
    }

}
