using UnityEngine;
using System.Collections;
using UnityEngine.TextCore.Text;

public class Horizontal_Movement : Player
{
    public Vector2 movement;
    private Crouch crouch;
    public float velocity = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Features();
        crouch = GetComponent<Crouch>();
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

            anim.SetBool("Idle", false);

            if (crouch.isCrouching)
                anim.SetBool("moveCrouching", true);

            else if (!crouch.isCrouching)
                anim.SetBool("Walking", true);

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

            anim.SetBool("Idle", true);


            anim.SetBool("moveCrouching", false);
            anim.SetBool("Walking", false);
        }
    }
}
