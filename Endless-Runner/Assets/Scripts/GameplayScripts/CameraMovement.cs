﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform player;
    Vector3 m_CameraOffset = new Vector3(0, 2.4f, -5.65f);
    private float m_FollowSpeed = 10;
    private Quaternion _rotation;

    public Vector3 Offset
    {

        get
        {
            return player.position + transform.TransformDirection(m_CameraOffset);
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Player.transform;

        transform.position = Offset;
        transform.rotation = player.rotation;
        _rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
       
      transform.position = Vector3.Lerp(transform.position, Offset, Time.deltaTime * m_FollowSpeed);
        
    }

}
