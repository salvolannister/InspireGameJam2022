using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Patrol patrol;
    public SpriteRenderer enemySpriteRenderer;
    public int damagePoint = 1;

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(" Get Damage called on Player");
            collision.gameObject.GetComponent<SwordManCS>().GetDamage(damagePoint);
        }
    }


    public void OnDisable()
    {
        patrol.OnChangeDir -= SwitchTextureDir;

    }
}
