using Unity.VisualScripting;
using UnityEngine;

public class Agarrar_Objeto : MonoBehaviour
{
    public Transform puntoDeAgarre; //papá
    private GameObject objetoEnZona; //candidato a hijo
    private GameObject objetoAgarrado; //hijo
    private bool estaAgarrando = false;
    private Rigidbody2D rbObjeto;

    //public BoxCollider2D bx;
    //private BoxCollider2D bxParado;
    //private BoxCollider2D bxAgachado;

    private void Start()
    {
        //bx = GetComponent<BoxCollider2D>();
        //bxParado = bx;
        //bxAgachado.offset = new Vector2(0, -0.25f);
        //bxAgachado.size = new Vector2(0, 0.3f);
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void AgarrarSoltar()
    {
        //AGARRAR OBJETO
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (estaAgarrando) // Soltar
            {
                rbObjeto = objetoAgarrado.GetComponent<Rigidbody2D>();
                rbObjeto.simulated = true;
                //rbObjeto.bodyType = RigidbodyType2D.Dynamic;

                objetoAgarrado.transform.SetParent(null);
                objetoAgarrado = null;
                estaAgarrando = false;
            }

            else if (objetoEnZona != null) // Agarrar
            {
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
