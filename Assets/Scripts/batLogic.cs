using NUnit.Framework;
using UnityEngine;

public class batLogic : MonoBehaviour
{
    public float speed = 5f;
    public Transform player;
    private bool isActive = false;
    //public Animation animation;
       void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("activao");
            isActive = true;
        }
        
    }
    void Update()
    {
        if (isActive==true)
        {
            Debug.Log("moviendose?");
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }
}
