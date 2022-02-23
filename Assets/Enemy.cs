using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Patrol patrol;
    public SpriteRenderer enemySpriteRenderer;

    private void OnEnable()
    {
        patrol.OnChangeDir += SwitchTextureDir;
    }

    private void SwitchTextureDir(Vector3 newDest)
    {
        if( (newDest.x - transform.position.x) > 0)
        {
            enemySpriteRenderer.flipX = true;
        }
        else
        {
            enemySpriteRenderer.flipX = false;
        } 
        
    }

    private void OnDisable()
    {
        patrol.OnChangeDir -= SwitchTextureDir;

    }
}
