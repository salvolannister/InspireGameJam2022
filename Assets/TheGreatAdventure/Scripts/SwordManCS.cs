using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* copied and improved from swordman.cs of of Low_swordman asset */
public class SwordManCS : PlayerControllerCS
{
    public bool immuneToAttack = false;
    private LayerMask enemiesLayer;
    private SpriteRenderer[] srs;
    Color[] startColors;
    private void Start()
    {

        m_CapsulleCollider = this.transform.GetComponent<CapsuleCollider2D>();
        m_Anim = this.transform.Find("model").GetComponent<Animator>();
        m_rigidbody = this.transform.GetComponent<Rigidbody2D>();
        enemiesLayer = LayerMask.GetMask("Enemy");
        srs = GetComponentsInChildren<SpriteRenderer>();
        startColors = new Color[srs.Length];
        for (int i =0, len = srs.Length; i < len; i++)
        {
            startColors[i] = srs[i].color;
        }
        
    }



    private void Update()
    {


        checkInput();

        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);

        }




    }

    public void checkInput()
    {
  
        if (Input.GetKeyDown(KeyCode.S))  //아래 버튼 눌렀을때. 
        {

            IsSit = true;
            m_Anim.Play("Sit");


        }
        else if (Input.GetKeyUp(KeyCode.S))
        {

            m_Anim.Play("Idle");
            IsSit = false;

        }


        // sit나 die일때 애니메이션이 돌때는 다른 애니메이션이 되지 않게 한다. 
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentJumpCount < JumpCount)  // 0 , 1
                {
                    DownJump();
                }
            }

            return;
        }


        m_MoveX = Input.GetAxis("Horizontal");



        GroundCheckUpdate();


        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") )
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {

                m_Anim.Play("Attack");
                CheckForHit();
            }
            else
            {

                if (m_MoveX == 0)
                {
                    if (!OnceJumpRayCheck)
                        m_Anim.Play("Idle");

                }
                else
                {

                    m_Anim.Play("Run");
                }

            }
        }


        if (Input.GetKey(KeyCode.Alpha1))
        {
            m_Anim.Play("Die");

        }

        // 기타 이동 인풋.

        if (Input.GetKey(KeyCode.D))
        {

            if (isGrounded)  // 땅바닥에 있었을때. 
            {



                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;

                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);



            }
            else
            {

                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));

            }




            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.A))
                Filp(false);


        }
        else if (Input.GetKey(KeyCode.A))
        {


            if (isGrounded)  // 땅바닥에 있었을때. 
            {



                if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    return;


                transform.transform.Translate(Vector2.right * m_MoveX * MoveSpeed * Time.deltaTime);

            }
            else
            {

                transform.transform.Translate(new Vector3(m_MoveX * MoveSpeed * Time.deltaTime, 0, 0));

            }


            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.D))
                Filp(true);


        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;


            if (currentJumpCount < JumpCount)  // 0 , 1
            {

                if (!IsSit)
                {
                    PerformJump();


                }
                else
                {
                    DownJump();

                }

            }


        }



    }

    private void CheckForHit()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, enemiesLayer.value);
        if (hit)
        {
            hit.collider.gameObject.GetComponent<Enemy>().GetDamage();
        }
    }


    protected override void LandingEvent()
    {


        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            m_Anim.Play("Idle");

    }
    public override void TakeDamage(int damage = 1)
    {
        if (!immuneToAttack)
        {
            if (LIFES <= damage)
            {
                LIFES = 0;
                // Play Animation Then Make Game Over
                StartCoroutine(HandlePlayerDeath());


            }
            else
            {

                LIFES -= damage;
                StartCoroutine(HandleDamage());
            }
        }



    }


    private IEnumerator HandlePlayerDeath()
    {
        WaitForSeconds waitForEndOfFrame = new WaitForSeconds(0.030f);
        // PLAY DEATH SOUND
        m_Anim.Play("Die");

        while (!(m_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !m_Anim.IsInTransition(0)))
        {
            yield return waitForEndOfFrame;
        }

        TheGreatAdventure.GameOver();
    }

    IEnumerator HandleDamage()
    {

        immuneToAttack = true;
        Debug.Log("playing damage");

        yield return DamageAnimation();

        immuneToAttack = false;
        //PLAY damage sound
    }

   
    IEnumerator DamageAnimation()
    {
        int len = srs.Length;


        for (int i = 0; i < 3; i++)
        {
            for (int x = 0; x < len; x++)
            {
                Color c = srs[x].color;
                c.a = 0;
                c.r = 1f;
                c.b = c.g = 0;
                srs[x].color = c;
            }
            yield return new WaitForSeconds(.1f);

            for (int w = 0; w < len; w++)
            {
                Color c = srs[w].color;
                c.a = startColors[w].a;
                c.b = c.g = 0;
                c.r = 0.9f;
                srs[w].color = c;
            }

            yield return new WaitForSeconds(.1f);
        }

        for (int w = 0; w < len; w++)
        {
            Color c = srs[w].color;
            c.a = startColors[w].a;
            c.b = startColors[w].b;
            c.g = startColors[w].g;
            c.r = startColors[w].r;
            srs[w].color = c;
        }
    }


}





