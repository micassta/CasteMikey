using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections;
using System.Collections.Generic;

public class LedgeLocator : MonoBehaviour
{
    public AnimationClip clip;
    public float climbingHorizontalOffset;

    private Vector2 topOfPlayer;
    private GameObject ledge;
    private float animationTime = 0.5f;
    private bool falling;
    private bool moved;
    private bool climbing;

    private Player_Movement player;

    //[HideInInspector]
    public bool grabbingLedge;
    //private Collider2D col;
    //private Rigidbody2D rb;
    //private Animator anim;

    void Start()
    {
        //col = GetComponent<Collider2D>();
        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();

        player = GetComponent<Player_Movement>();

        if (clip != null)
        {
            animationTime = clip.length;
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        CheckForLedge();
        LedgeHanging();
    }

    protected virtual void CheckForLedge()
    {
        if (!falling)
        {
            if (transform.localScale.x > 0) //Viendo a la derecha
            {
                topOfPlayer = new Vector2(player.bc.bounds.max.x + 0.1f, player.bc.bounds.max.y);

                RaycastHit2D hit = Physics2D.Raycast(topOfPlayer, Vector2.right, 0.2f);

                if (hit && hit.collider.gameObject.GetComponent<Ledge>())
                {
                    ledge = hit.collider.gameObject;

                    Collider2D ledgeCollider = ledge.GetComponent<Collider2D>();

                    if (player.bc.bounds.max.y < ledgeCollider.bounds.max.y && player.bc.bounds.max.y > ledgeCollider.bounds.center.y && player.bc.bounds.min.x < ledgeCollider.bounds.min.x)
                    {
                        grabbingLedge = true;
                        //anim.SetBool("LedgeHanging", true);
                    }
                }
            }

            else //Viendo a la Izquierda
            {
                topOfPlayer = new Vector2(player.bc.bounds.min.x - .1f, player.bc.bounds.max.y);
                RaycastHit2D hit = Physics2D.Raycast(topOfPlayer, Vector2.left, .2f);
                if (hit && hit.collider.gameObject.GetComponent<Ledge>())
                {
                    ledge = hit.collider.gameObject;

                    Collider2D ledgeCollider = ledge.GetComponent<Collider2D>();

                    if (player.bc.bounds.max.y < ledgeCollider.bounds.max.y && player.bc.bounds.max.y > ledgeCollider.bounds.center.y && player.bc.bounds.max.x > ledgeCollider.bounds.max.x)
                    {
                        grabbingLedge = true;
                        //anim.SetBool("LedgeHanging", true);
                    }
                }
            }

            if (ledge != null && grabbingLedge) //si la plataforma tiene el script ledge y la zona de grabbinledge se cumple
            {
                AdjustPlayerPosition();
                player.rb.linearVelocity = Vector2.zero;
                player.rb.bodyType = RigidbodyType2D.Kinematic;
                //GetComponent<Player_Movement>().enabled = false;
            }

            else
            {
                player.rb.bodyType = RigidbodyType2D.Dynamic;
                //GetComponent<Player_Movement>().enabled = true;
            }
        }
    }



    protected virtual void LedgeHanging()
    {
        if (grabbingLedge && Input.GetAxis("Vertical") > 0 && !climbing) //presiona la tecla de abajo, esta aggarando un ledge y no est� escalando
        {
            climbing = true;

            //Stops playing the LedgeHanging bool
            //anim.SetBool("LedgeHanging", false);

            if (transform.localScale.x > 0) //Viendo a la derecha
            {
                StartCoroutine(ClimbingLedge(new Vector2(transform.position.x + climbingHorizontalOffset, ledge.GetComponent<Collider2D>().bounds.max.y + player.bc.bounds.extents.y), animationTime));
            }

            else //Viendo a la izquierda
            {
                StartCoroutine(ClimbingLedge(new Vector2(transform.position.x - climbingHorizontalOffset, ledge.GetComponent<Collider2D>().bounds.max.y + player.bc.bounds.extents.y), animationTime));
            }

        }

        if (grabbingLedge && Input.GetAxis("Vertical") < 0) // Presiona la tecla de abajo
        {
            ledge = null;
            moved = false;
            grabbingLedge = false;

            //Stops playing the LedgeHanging animation
            //anim.SetBool("LedgeHanging", false);

            falling = true;
            player.rb.bodyType = RigidbodyType2D.Dynamic;
            //I've set up my HorizontalMovement script to not run the movement logic if true, but if you're not sure how to set that up, use the line below
            //GetComponent<HorizontalMovement>().enabled = true;
            //Runs the NotFalling method half a second later to make sure the falling bool gets set back to false quickly
            Invoke("NotFalling", 0.5f);
        }
    }

    protected virtual IEnumerator ClimbingLedge(Vector2 topOfPlatform, float duration)
    {
        float time = 0;
        Vector2 startValue = transform.position;
        //Vector2 startValue = player.rb.position;

        while (time <= duration)
        {
            //Plays the LedgeClimbing animation
            //anim.SetBool("LedgeClimbing", true);

            transform.position = Vector2.Lerp(startValue, topOfPlatform, time / duration);
            //player.rb.linearVelocity = Vector2.Lerp(startValue, topOfPlatform, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        ledge = null;
        moved = false;
        climbing = false;
        grabbingLedge = false;

        //Stops playing the LedgeClimbing animation
        //anim.SetBool("LedgeClimbing", false);
    }

    protected virtual void AdjustPlayerPosition()
    {
        if (!moved)
        {
            moved = true;
            Collider2D ledgeCollider = ledge.GetComponent<Collider2D>();
            Ledge platform = ledge.GetComponent<Ledge>();

            if (transform.localScale.x > 0) //Viendo a la derecha
            {
                transform.position = new Vector2((ledgeCollider.bounds.min.x - player.bc.bounds.extents.x) + platform.hangingHorizontalOffset, (ledgeCollider.bounds.max.y - player.bc.bounds.extents.y - 0.5f) + platform.hangingVerticalOffset);
                //player.rb.position = new Vector2((ledgeCollider.bounds.min.x - player.bc.bounds.extents.x) + platform.hangingHorizontalOffset, (ledgeCollider.bounds.max.y - player.bc.bounds.extents.y - 0.5f) + platform.hangingVerticalOffset);
            }

            else //Viendo a la izquierda
            {
                transform.position = new Vector2((ledgeCollider.bounds.max.x + player.bc.bounds.extents.x) - platform.hangingHorizontalOffset, (ledgeCollider.bounds.max.y - player.bc.bounds.extents.y - 0.5f) + platform.hangingVerticalOffset);
                //player.rb.position = new Vector2((ledgeCollider.bounds.max.x + player.bc.bounds.extents.x) - platform.hangingHorizontalOffset, (ledgeCollider.bounds.max.y - player.bc.bounds.extents.y - 0.5f) + platform.hangingVerticalOffset);
            }
        }
    }

    protected virtual void NotFalling()
    {
        falling = false;
    }
}
