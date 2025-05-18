using UnityEngine;

public class Agarrar_Objeto : MonoBehaviour
{
    public Transform puntoDeAgarre;
    private GameObject objetoAgarrado; //Referencia al objeto que vamos a agarrar
    private bool agarrando = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            Debug.Log("TOY AGACHADO");

            if (agarrando)
            {
                // Soltar
                objetoAgarrado.transform.SetParent(null);
                objetoAgarrado = null;
                agarrando = false;
            }
            else
            {
                // Buscar cerca del jugador
                Collider2D[] objetos = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (var col in objetos)
                {
                    if (col.CompareTag("Agarrable"))
                    {
                        objetoAgarrado = col.gameObject;
                        objetoAgarrado.transform.position = puntoDeAgarre.position;
                        objetoAgarrado.transform.SetParent(puntoDeAgarre);
                        agarrando = true;
                        break;
                    }
                }
            }
        }


    }
}
