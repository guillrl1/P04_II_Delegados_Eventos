using UnityEngine;

public class RecolectorEscudosConsola : MonoBehaviour
{
    public float velocidad = 6.0f;
    public int puntuacion = 0;

    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movX, 0f, movZ) * velocidad * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión con: " + collision.collider.gameObject.name);
        ProcesarRecoleccion(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger con: " + other.gameObject.name);
        ProcesarRecoleccion(other);
    }

    void ProcesarRecoleccion(Collider otro)
    {
        // Escudos de Tipo 1 suman 5 puntos
        if (otro.CompareTag("Escudo1"))
        {
            puntuacion += 5;
            Debug.Log("Escudo Tipo 1 recogido (+5). Puntuación total: " + puntuacion);
            Destroy(otro.gameObject);
        }
        // Escudos de Tipo 2 suman 10 puntos
        else if (otro.CompareTag("Escudo2"))
        {
            puntuacion += 10;
            Debug.Log("Escudo Tipo 2 recogido (+10). Puntuación total: " + puntuacion);
            Destroy(otro.gameObject);
        }
    }
}
