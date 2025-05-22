using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

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
    public bool canFlip= true;
    //para pisar enemigos
    public GameObject patas;
    //pa la posicion de patas
    public Transform posicionPatas;

    ///////////
    public Interactuar_Objeto interactuarObjeto;
    public BoxCollider2D bc;
    private bool pata =false;
    ///////////

    ///
    public Vector2 sizeBoxOnFloor = new Vector2(0.76f, 0.6f);
    public Vector2 sizeBoxToRoof = new Vector2 (0.78f, 0.76f);
    ///

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
        if (canFlip)
        {
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(playerScale.x * -1, playerScale.y, playerScale.z);
            }

            else if (movement.x > 0)
            {
                transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
            }
        }
        if (onFloor)
        {
            Vector3 finalPos = posicionPatas.position + Vector3.down * 0.3f;
            Instantiate(patas, finalPos, Quaternion.identity);
        } else
        {
            if (pata)
            {
                Destroy(patas);
                pata = false;
            }
            else
                Debug.Log("no ha saltado aun");
        }

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, floorLayer);
            //onFloor = hit.collider == true;
            //Debug.Log(transform.position);

            RaycastHit2D hit_floor = Physics2D.BoxCast(transform.position, sizeBoxOnFloor, 0f, Vector2.down, raycastDistance_hit_floor, floorLayer);
        onFloor = hit_floor.collider != null;

        RaycastHit2D hit_roof = Physics2D.BoxCast(transform.position, sizeBoxToRoof, 0f, Vector2.up, raycastDistance_hit_roof, floorLayer);
        noRoof = hit_roof.collider == false;


        //animator.SetBool("onFloor", onFloor);

        //animator.SetFloat("movement", movement.x);

        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            Jump();
            onFloor = false;
            pata = true;
            //Debug.Log("TOY SALTANDO");
        }

        else
        {
            movement.x = 0f;
        }

        ///////////
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //Debug.Log("TOY AGACHADO");
            interactuarObjeto.AgarrarSoltar();  //Funcion del Script Agarrar_Objeto
            bc.offset = new Vector2(0f, -0.25f);
            bc.size = new Vector2(1f, 0.5f);
            velocity = 2.5f;
            jumpForce = 1.5f;
        }

        else
        {
            if (noRoof)
            {
                //Debug.Log("TOY DE PIE");
                interactuarObjeto.Arrojar();
                bc.offset = new Vector2(0f, 0f);
                bc.size = new Vector2(1f, 1f);
                velocity = 5f;
                jumpForce = 5f;
            }
        }
        ///////////
        /// 

    }
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //animator.SetBool("onFloor", false);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = UnityEngine.Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);

        Gizmos.color = UnityEngine.Color.blue;

        Vector2 origin = transform.position;
        Vector2 direction_hit_floor = Vector2.down;

        //BoxOriginal
        Gizmos.DrawWireCube(origin, sizeBoxOnFloor);

        //Box del Onfloor
        Vector2 endPos_hit_floor = origin + direction_hit_floor * raycastDistance_hit_floor;
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(endPos_hit_floor, sizeBoxOnFloor);

        Vector2 direction_hit_roof = Vector2.up;
        Vector2 endPos_hit_roof = origin + direction_hit_roof * raycastDistance_hit_roof;
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireCube(endPos_hit_roof, sizeBoxToRoof);
    }

}
