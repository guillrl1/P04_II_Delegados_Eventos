using UnityEngine;

public class CuboNotificador : MonoBehaviour
{
    public float velocidad = 6.0f;

    // Usamos eventos estáticos para que los humanoides puedan suscribirse directamente a la clase
    public delegate void MensajeColision();
    public static event MensajeColision OnColisionHumanoideTipo1;
    public static event MensajeColision OnColisionHumanoideTipo2;

    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movX, 0f, movZ) * velocidad * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ComprobarColision(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        ComprobarColision(other);
    }

    void ComprobarColision(Collider otro)
    {
        // Si chocamos con un Lich (Tipo 2), avisamos para que los de Tipo 1 vayan al escudo fijado
        if (otro.CompareTag("Tipo2"))
        {
            Debug.Log("El cubo ha chocado con un humanoide Tipo 2.");
            if (OnColisionHumanoideTipo2 != null)
            {
                OnColisionHumanoideTipo2();
            }
        }
        // Si tocamos a un Soldado (Tipo 1), avisamos para ir hacia los escudos físicos de Tipo 2
        else if (otro.CompareTag("Tipo1"))
        {
            Debug.Log("El cubo ha tocado a un humanoide Tipo 1.");
            if (OnColisionHumanoideTipo1 != null)
            {
                OnColisionHumanoideTipo1();
            }
        }
    }
}
