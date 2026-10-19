using UnityEngine;

public class HumanoideObservador : MonoBehaviour
{
    [Header("Referencias del Evento")]
    public CilindroNotificador notificador; // Arrastrar el Cilindro de la escena

    [Header("Objetivos")]
    public Transform objetivoHumanoideTipo2; // Arrastrar el Lich objetivo para los soldados (Tipo 1)
    public float velocidad = 3.5f;
    public float distanciaParada = 1.2f; // Para frenar un poco antes y no atravesar el modelo

    private bool debeMoverse = false;
    private Transform destinoActual;

    void Start()
    {
        // Nos suscribimos al evento del cilindro dependiendo de si somos Soldado (Tipo 1) o Lich (Tipo 2)
        if (notificador != null)
        {
            if (CompareTag("Tipo1"))
            {
                notificador.OnColisionCubo += RespuestaTipo1;
            }
            else if (CompareTag("Tipo2"))
            {
                notificador.OnColisionCubo += RespuestaTipo2;
            }
        }
    }

    void Update()
    {
        if (debeMoverse && destinoActual != null)
        {
            Vector3 posicionDestino = new Vector3(
                destinoActual.position.x,
                transform.position.y,
                destinoActual.position.z
            );

            float distancia = Vector3.Distance(transform.position, posicionDestino);

            if (distancia > distanciaParada)
            {
                // Rotamos al personaje para que mire de frente hacia donde está caminando
                transform.LookAt(posicionDestino);

                // Lo desplazamos hacia su objetivo
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    posicionDestino,
                    velocidad * Time.deltaTime
                );
            }
        }
    }

    void RespuestaTipo1()
    {
        destinoActual = objetivoHumanoideTipo2;
        debeMoverse = true;
    }

    void RespuestaTipo2()
    {
        destinoActual = notificador.transform;
        debeMoverse = true;
    }

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
