using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D bc;

    protected Animator anim;

    protected Player player;

    [HideInInspector]
    public bool isFacingLeft;
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool isCrouching;
    [HideInInspector]
    public bool grabbingLedge;
    [HideInInspector]
    public bool takingDamage;
    [HideInInspector]
    public bool isDead;

    Vector2 facingLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Features();
    }

    protected virtual void Features()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        player = GetComponent<Player>();

        facingLeft = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public void Flip()
    {
        if (isFacingLeft)
        {
            transform.localScale = facingLeft;
        }

        if (!isFacingLeft)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

}
