using UnityEngine;
using System.Collections;
using UnityEngine.TextCore.Text;

public class Horizontal_Movement : Player
{
    public Vector2 movement;

    public float velocity = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Features();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.grabbingLedge)
            movement.x = Input.GetAxisRaw("Horizontal");


    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {

        rb.linearVelocity = new Vector2(movement.x * velocity, rb.linearVelocity.y);

        if (movement.x != 0)
        {
            //Debug.Log("TOY CAMINANDO");

            //anim.SetBool("Idle", true);
            //anim.SetBool("Walking", false);

            if (movement.x < 0 && !player.isFacingLeft)
            {
                player.isFacingLeft = true;
                Flip();
                //Debug.Log("IZQ");
            }

            else if (movement.x > 0 && player.isFacingLeft)
            {
                player.isFacingLeft = false;
                Flip();
                //Debug.Log("DER");
            }
        }

        else 
        {
            //Debug.Log("TOY PARADO");

            //player.anim.SetBool("Idle", true);
            //player.anim.SetBool("Walking", false);
        }
    }
}
