using UnityEngine;

public class EsferaObservador : MonoBehaviour
{
    [Header("Referencias del Evento")]
    public CilindroNotificador notificador; // Aquí arrastramos el cilindro desde la escena

    [Header("Configuración de Movimiento")]
    public Transform objetivoEsferaTipo2; // Esfera verde fijada como objetivo para las de Tipo 1
    public float velocidad = 4.0f;

    private bool debeMoverse = false;
    private Transform destinoActual;

    void Start()
    {
        // Pintamos cada esfera de su color automáticamente según su Tag para no hacerlo a mano
        Renderer renderizador = GetComponent<Renderer>();

        if (CompareTag("Tipo1"))
        {
            if (renderizador != null) renderizador.material.color = Color.red;
            
            // Las rojas (Tipo 1) se suscriben con su propio comportamiento
            if (notificador != null) notificador.OnColisionCubo += RespuestaTipo1;
        }
        else if (CompareTag("Tipo2"))
        {
            if (renderizador != null) renderizador.material.color = Color.green;
            
            // Las verdes (Tipo 2) se suscriben para ir hacia el cilindro
            if (notificador != null) notificador.OnColisionCubo += RespuestaTipo2;
        }
    }

    void Update()
    {
        // Solo nos movemos cuando hayamos recibido la notificación del cilindro
        if (debeMoverse && destinoActual != null)
        {
            // Mantenemos nuestra propia altura (Y) para no hundirnos ni flotar
            Vector3 posicionDestino = new Vector3(
                destinoActual.position.x,
                transform.position.y,
                destinoActual.position.z
            );

            // Avanzamos poco a poco hacia el objetivo en cada fotograma
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionDestino,
                velocidad * Time.deltaTime
            );
        }
    }

    // Función que se ejecuta en las esferas de Tipo 1 cuando salta el evento
    void RespuestaTipo1()
    {
        destinoActual = objetivoEsferaTipo2;
        debeMoverse = true;
    }

    // Función que se ejecuta en las esferas de Tipo 2 cuando salta el evento
    void RespuestaTipo2()
    {
        destinoActual = notificador.transform;
        debeMoverse = true;
    }

    // Importante: si el objeto se destruye, nos borramos de la lista del evento para evitar errores
    private void OnDestroy()
    {
        if (notificador != null)
        {
            if (CompareTag("Tipo1"))
                notificador.OnColisionCubo -= RespuestaTipo1;
            else if (CompareTag("Tipo2"))
                notificador.OnColisionCubo -= RespuestaTipo2;
        }
    }
}
