﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : World {

    [SerializeField] bool wall;
    static int materialIndex = 3;

    //static int counter=0;
    int currentLevel;

    Material[] materials = new Material[12];
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        materials = Resources.LoadAll<Material>("Materials/Blocks");

        SetLevel();

        GetMaterial();

        rend.sharedMaterial = materials[materialIndex];
    }

    void GetMaterial()
    {
        switch (currentLevel)
        {
            case 1:
                materialIndex = 0;
                break;
            case 2 : materialIndex = 2;
                if(wall)
                {
                    materialIndex--;
                }
                break;
            case 3: materialIndex = 3;
                break;
            default:
                materialIndex = 10;
                break;
        }
    }


    public void SetLevel()
    {
        currentLevel = FindObjectOfType<GameManager>().CurrentLevel(); ;
    }

    //void ColourCycle()
    //{
        
    //    if (counter < 5)
    //    {
    //        materialIndex++;

    //        if (materialIndex > 9) 
    //        {
    //            materialIndex = 5;
    //        }

    //        counter++;

    //    }
    //    else
    //    {
    //        counter = 0;
    //        materialIndex = 4;
    //    }
        
    //}
}//Blocks
