using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Interactuar_Objeto : MonoBehaviour
{
    public Transform puntoDeAgarre; //pap�
    private GameObject objetoEnZona; //candidato a hijo
    private GameObject objetoAgarrado; //hijo
    private bool estaAgarrando = false;
    private Rigidbody2D rbObjeto;
    public float fuerzaDeArroje = 10f;
    public float anguloDeArrojeNormal = 45f;//lanzar en arco
    public float anguloDeArrojeVertical = 65f;//lanzar mas alto

    //Variables para latigo
    public float timing = 0.1f;
    public GameObject hitB1;
    public GameObject hitB2;
    public GameObject hitB3;
    public Transform playerTransform;
    private bool isWhipping = false;
    private Player_Movement Player_Movement;


    public void AgarrarSoltar()
    {
        //AGARRAR OBJETO
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (estaAgarrando) // Soltar
            {
                Debug.Log("TOY SOLTANDO");
                rbObjeto = objetoAgarrado.GetComponent<Rigidbody2D>();
                rbObjeto.simulated = true;
                //rbObjeto.bodyType = RigidbodyType2D.Dynamic;

                objetoAgarrado.transform.SetParent(null);
                objetoAgarrado = null;
                estaAgarrando = false;
            }

            else if (objetoEnZona != null) // Agarrar
            {
                Debug.Log("TOY AGARRANDO");
                objetoAgarrado = objetoEnZona; //se hace esto para que solo haya un objeto agarrado

                rbObjeto = objetoAgarrado.GetComponent<Rigidbody2D>();
                //rbObjeto.bodyType = RigidbodyType2D.Kinematic;
                rbObjeto.simulated = false; //se quitan las fisicas para que el objeto siga al padre mientras salta

                objetoAgarrado.transform.position = puntoDeAgarre.position;
                objetoAgarrado.transform.SetParent(puntoDeAgarre);
                estaAgarrando = true;
            }
        }
    }

    public void Arrojar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!estaAgarrando)
            {
                if (Input.GetKeyDown(KeyCode.X) && !isWhipping)
                {
                    StartCoroutine(WhipSequence());
                }
            }//para poner que cuendo se pique y si este agarrando algo lo lanze
        }
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("TOY ARROJANDO");
                objetoAgarrado.transform.SetParent(null);
                rbObjeto.simulated = true;
                float anguloFinal = anguloDeArrojeNormal * Mathf.Deg2Rad;

                int direccion = transform.localScale.x > 0 ? 1 : -1;
                Vector2 fuerzaArco = new Vector2(Mathf.Cos(anguloFinal) * direccion, Mathf.Sin(anguloFinal)) * fuerzaDeArroje;
                rbObjeto.AddForce(fuerzaArco, ForceMode2D.Impulse);
                objetoAgarrado = null;
                estaAgarrando = false;
            }
            if (Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("TOY ARROJANDO");
                objetoAgarrado.transform.SetParent(null);
                rbObjeto.simulated = true;
                float anguloFinal = anguloDeArrojeVertical * Mathf.Deg2Rad;

                int direccion = transform.localScale.x > 0 ? 1 : -1;
                Vector2 fuerzaArco = new Vector2(Mathf.Cos(anguloFinal) * direccion, Mathf.Sin(anguloFinal)) * fuerzaDeArroje;
                rbObjeto.AddForce(fuerzaArco, ForceMode2D.Impulse);
                objetoAgarrado = null;
                estaAgarrando = false;
            }


        }
    }
        //Ataque de latigo
        IEnumerator WhipSequence()
    {
        isWhipping = true;
        Player_Movement.canFlip = false; // no deja gorar mientras ataca

        bool facingRight = transform.localScale.x > 0;
        //atras 
        Vector2 offsetBack = facingRight ? new Vector2(-0.5f, 1f) : new Vector2(0.5f, 1f);
        CreateHitbox(hitB1, offsetBack);
        yield return new WaitForSeconds(timing);

        //arriba
        CreateHitbox(hitB2, new Vector2(0f, 1.5f));
        yield return new WaitForSeconds(timing);


        // ne frente 
        Vector2 offsetFront = facingRight ? new Vector2(0.5f, 1f) : new Vector2(-0.5f, 1f);
        CreateHitbox(hitB3, offsetFront);
        yield return new WaitForSeconds(timing);

        
        isWhipping = false;
        Player_Movement.canFlip = true;
    }
    void CreateHitbox(GameObject prefab, Vector2 offset)
    {
        GameObject hitbox = Instantiate(prefab, (Vector2)playerTransform.position + offset, Quaternion.identity);
        hitBoxFollow follow = hitbox.GetComponent<hitBoxFollow>();
        follow.player = playerTransform;
        follow.offset = offset;
    }
   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Agarrable"))
        {
            objetoEnZona = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Agarrable"))
        {
            objetoEnZona = null;
        }
    }

}
