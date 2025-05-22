using JetBrains.Annotations;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.UI.Image;

public class Crouch : Player
{
    public float raycastDistance_hit_roof = 0.38f;
    private bool noRoof;
    public Vector2 sizeBoxToRoof = new Vector2(0.78f, 0.76f);
    public Interactuar_Objeto interactuarObjeto;
    public Horizontal_Movement horizontal;
    private Jump jump;
    public LayerMask floorLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Features();
        horizontal = GetComponent<Horizontal_Movement>();
        jump = GetComponent<Jump>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit_roof = Physics2D.BoxCast(transform.position, sizeBoxToRoof, 0f, Vector2.up, raycastDistance_hit_roof, floorLayer);
        noRoof = hit_roof.collider == null;

        // Agacharse
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && jump.onFloor)
        {
            isCrouching = true;
            interactuarObjeto.AgarrarSoltar();
            bc.offset = new Vector2(0f, -0.25f);
            bc.size = new Vector2(1f, 0.5f);
            horizontal.velocity = 2.5f;
            jump.jumpForce = 1.5f;
        }
        else
        {
            if (noRoof)
            {
                isCrouching = false;
                interactuarObjeto.Arrojar();
                bc.offset = new Vector2(0f, 0f);
                bc.size = new Vector2(1f, 1f);
                horizontal.velocity = 5f;
                jump.jumpForce = 5f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;

        Vector2 endPos_hit_roof = origin + Vector2.up * raycastDistance_hit_roof;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(endPos_hit_roof, sizeBoxToRoof);
    }
}
