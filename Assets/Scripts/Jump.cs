using UnityEditor.Hardware;
using UnityEngine;

public class Jump : Player
{
    public float jumpForce = 7f;
    public float raycastDistance_hit_floor = 0.5f;
    public LayerMask floorLayer;
    public bool onFloor;
    public Vector2 sizeBoxOnFloor = new Vector2(0.76f, 0.6f);
    private bool jumpRequested = false;

    void Start()
    {
        Features();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit2D hit_floor = Physics2D.BoxCast(transform.position, sizeBoxOnFloor, 0f, Vector2.down, raycastDistance_hit_floor, floorLayer);
        onFloor = hit_floor.collider != null;

        anim.SetBool("OnFloor", onFloor);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && onFloor && !isJumping)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 endPos_hit_floor = origin + Vector2.down * raycastDistance_hit_floor;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endPos_hit_floor, sizeBoxOnFloor);
    }
}
