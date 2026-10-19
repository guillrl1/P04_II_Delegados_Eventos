using UnityEngine;
using TMPro;

public class PuntuacionUI : MonoBehaviour
{
    public float velocidad = 6.0f;
    public int puntuacion = 0;

    [Header("Interfaz de Usuario")]
    public TMP_Text textoPuntuacion; // Referencia al componente TextMeshPro del Canvas

    void Start()
    {
        // Inicializamos el marcador a 0 al arrancar la partida
        ActualizarTextoUI();
    }

    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movX, 0f, movZ) * velocidad * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcesarRecoleccion(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcesarRecoleccion(other);
    }

    void ProcesarRecoleccion(Collider otro)
    {
        if (otro.CompareTag("Escudo1"))
        {
            puntuacion += 5;
            ActualizarTextoUI();
            Destroy(otro.gameObject);
        }
        else if (otro.CompareTag("Escudo2"))
        {
            puntuacion += 10;
            ActualizarTextoUI();
            Destroy(otro.gameObject);
        }
    }

    void ActualizarTextoUI()
    {
        if (textoPuntuacion != null)
        {
            textoPuntuacion.text = "Puntuación: " + puntuacion;
        }
    }
}
