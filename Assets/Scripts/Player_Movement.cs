using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float velocity = 5f;
    public float jumpForce = 5f;

    public Vector2 movement;
    public Rigidbody2D rb;

    public Vector3 playerScale;

    public Animator animator;
    public float raycastDistance_hit_floor = 0.5f;
    public float raycastDistance_hit_roof = 0.38f;
    public LayerMask floorLayer;
    private bool onFloor;
    private bool noRoof;
    public bool canFlip = true;

    public Interactuar_Objeto interactuarObjeto;
    public BoxCollider2D bc;

    public Vector2 sizeBoxOnFloor = new Vector2(0.76f, 0.6f);
    public Vector2 sizeBoxToRoof = new Vector2(0.78f, 0.76f);

    private bool jumpRequested = false;

    void Start()
    {
        playerScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Movimiento horizontal (guardamos input)
        movement.x = Input.GetAxisRaw("Horizontal");

        // Flip
        if (canFlip)
        {
            if (movement.x < 0)
                transform.localScale = new Vector3(playerScale.x * -1, playerScale.y, playerScale.z);
            else if (movement.x > 0)
                transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
        }

        // Verificaci�n de suelo y techo
        RaycastHit2D hit_floor = Physics2D.BoxCast(transform.position, sizeBoxOnFloor, 0f, Vector2.down, raycastDistance_hit_floor, floorLayer);
        onFloor = hit_floor.collider != null;

        RaycastHit2D hit_roof = Physics2D.BoxCast(transform.position, sizeBoxToRoof, 0f, Vector2.up, raycastDistance_hit_roof, floorLayer);
        noRoof = hit_roof.collider == null;

        // Salto (pedimos salto)
        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            jumpRequested = true;
        }

        // Agacharse
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            interactuarObjeto.AgarrarSoltar();
            bc.offset = new Vector2(0f, -0.25f);
            bc.size = new Vector2(1f, 0.5f);
            velocity = 2.5f;
            jumpForce = 1.5f;
        }
        else
        {
            if (noRoof)
            {
                interactuarObjeto.Arrojar();
                bc.offset = new Vector2(0f, 0f);
                bc.size = new Vector2(1f, 1f);
                velocity = 5f;
                jumpForce = 5f;
            }
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento y salto con f�sica
        rb.linearVelocity = new Vector2(movement.x * velocity, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 origin = transform.position;

        Gizmos.DrawWireCube(origin, bc.size);

        Vector2 endPos_hit_floor = origin + Vector2.down * raycastDistance_hit_floor;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endPos_hit_floor, sizeBoxOnFloor);

        Vector2 endPos_hit_roof = origin + Vector2.up * raycastDistance_hit_roof;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(endPos_hit_roof, sizeBoxToRoof);
    }
}
