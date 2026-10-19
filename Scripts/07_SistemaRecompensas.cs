using UnityEngine;
using TMPro;

public class SistemaRecompensas : MonoBehaviour
{
    public float velocidad = 6.0f;
    public int puntuacion = 0;

    // Controla cada cuántos puntos saltará la siguiente recompensa (100, 200, 300...)
    private int siguienteMeta = 100;

    [Header("Interfaz de Usuario")]
    public TMP_Text textoPuntuacion;
    public TMP_Text textoRecompensa;

    // Eventos estáticos para integrarlo todo en el prototipo final (Ejercicio 8)
    public delegate void MensajeColision();
    public static event MensajeColision OnColisionHumanoideTipo1;
    public static event MensajeColision OnColisionHumanoideTipo2;

    void Start()
    {
        ActualizarTextoUI();
        
        // Al empezar ocultamos el aviso de recompensa para que solo salga al llegar a 100 puntos
        if (textoRecompensa != null)
            textoRecompensa.gameObject.SetActive(false);
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
        ComprobarColision(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        ComprobarColision(other);
    }

    void ComprobarColision(Collider otro)
    {
        // Recolección de escudos
        if (otro.CompareTag("Escudo1"))
        {
            SumarPuntos(5);
            Destroy(otro.gameObject);
        }
        else if (otro.CompareTag("Escudo2"))
        {
            SumarPuntos(10);
            Destroy(otro.gameObject);
        }

        // Notificación a los humanoides de la escena
        if (otro.CompareTag("Tipo2"))
        {
            if (OnColisionHumanoideTipo2 != null) OnColisionHumanoideTipo2();
        }
        else if (otro.CompareTag("Tipo1"))
        {
            if (OnColisionHumanoideTipo1 != null) OnColisionHumanoideTipo1();
        }
    }

    void SumarPuntos(int cantidad)
    {
        puntuacion += cantidad;
        ActualizarTextoUI();

        // Si alcanzamos o pasamos el múltiplo de 100 actual, damos recompensa
        if (puntuacion >= siguienteMeta)
        {
            siguienteMeta += 100;
            OtorgarRecompensa();
        }
    }

    void OtorgarRecompensa()
    {
        // Subimos la velocidad del jugador como premio y mostramos el mensaje en el Canvas
        velocidad += 2.0f;

        if (textoRecompensa != null)
        {
            textoRecompensa.text = "¡RECOMPENSA DESBLOQUEADA! (+Velocidad)";
            textoRecompensa.gameObject.SetActive(true);

            // Quitamos el texto de la pantalla pasados 3 segundos
            CancelInvoke(nameof(OcultarRecompensa));
            Invoke(nameof(OcultarRecompensa), 3.0f);
        }
    }

    void OcultarRecompensa()
    {
        if (textoRecompensa != null)
            textoRecompensa.gameObject.SetActive(false);
    }

    void ActualizarTextoUI()
    {
        if (textoPuntuacion != null)
            textoPuntuacion.text = "Puntuación: " + puntuacion;
    }
}
