using UnityEngine;

public class hitBoxFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;
    public float duration = 0.2f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = (Vector2)player.position + offset;
        }
    }
}