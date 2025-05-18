using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Movement : MonoBehaviour
{
    public float velocity = 5f;
    public float jumpForce = 5f;

    public Vector2 movement;
    public Rigidbody2D rb;

    public Vector3 playerScale;

    public Animator animator;
    public float raycastDistance = 0.1f;
    public LayerMask floorLayer;
    private bool onFloor;

    ///////////
    public Agarrar_Objeto agarrarObjeto;
    public BoxCollider2D bc;
    ///////////

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();

        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        transform.Translate(movement * velocity * Time.deltaTime);

        if (movement.x < 0)
        {
            transform.localScale = new Vector3(playerScale.x * -1, playerScale.y, playerScale.z);
        }

        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, floorLayer);
        onFloor = hit.collider == true;
        //animator.SetBool("onFloor", onFloor);

        //animator.SetFloat("movement", movement.x);

        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            Jump();
            onFloor = false;
            Debug.Log("TOY SALTANDO");
        }

        else
        {
            movement.x = 0f;
        }

        ///////////
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //Debug.Log("TOY AGACHADO");
            agarrarObjeto.AgarrarSoltar();  //Funcion del Script Agarrar_Objeto
            bc.offset = new Vector2(0f, -0.25f);
            bc.size = new Vector2(1f, 0.5f);
            velocity = 2.5f;
            jumpForce = 1.5f;
        }

        else
        {
            //Debug.Log("TOY DE PIE");
            bc.offset = new Vector2(0f, 0f);
            bc.size = new Vector2(1f, 1f);
            velocity = 5f;
            jumpForce = 5f;
        }
        ///////////

    }
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //animator.SetBool("onFloor", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
    }

}
