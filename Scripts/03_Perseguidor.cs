using UnityEngine;

public class Perseguidor : MonoBehaviour
{
    [Header("Objetivos (Escudos)")]
    public Transform escudoSeleccionado; // Escudo concreto fijado de antemano para el grupo 1
    public float velocidad = 4.0f;
    public Color colorAlChocar = Color.cyan;

    private bool debeMoverse = false;
    private Transform destinoActual;

    void Start()
    {
        // Nos suscribimos a los eventos estáticos del cubo según nuestro grupo
        if (CompareTag("Tipo1"))
        {
            CuboNotificador.OnColisionHumanoideTipo2 += IrAEscudoSeleccionado;
            CuboNotificador.OnColisionHumanoideTipo1 += IrAEscudoTipo2;
        }
        else if (CompareTag("Tipo2"))
        {
            CuboNotificador.OnColisionHumanoideTipo1 += IrAEscudoTipo2;
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

            // Miramos en dirección al escudo que estamos persiguiendo
            transform.LookAt(posicionDestino);

            // Nos acercamos al escudo
            transform.position = Vector3.MoveTowards(
                transform.position,
                posicionDestino,
                velocidad * Time.deltaTime
            );
        }
    }

    // Callback cuando el cubo choca con un Tipo 2
    void IrAEscudoSeleccionado()
    {
        destinoActual = escudoSeleccionado;
        debeMoverse = true;
    }

    // Callback cuando el cubo toca un Tipo 1: buscamos el escudo físico (Escudo2) más cercano
    void IrAEscudoTipo2()
    {
        GameObject[] escudosGrupo2 = GameObject.FindGameObjectsWithTag("Escudo2");
        float distanciaMinima = Mathf.Infinity;
        Transform escudoMasCercano = null;

        foreach (GameObject escudo in escudosGrupo2)
        {
            float distancia = Vector3.Distance(transform.position, escudo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                escudoMasCercano = escudo.transform;
            }
        }

        if (escudoMasCercano != null)
        {
            destinoActual = escudoMasCercano;
            debeMoverse = true;
        }
    }

    // Cuando el humanoide choca físicamente con alguno de los escudos, cambia de color y se detiene
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Escudo1") || collision.collider.CompareTag("Escudo2"))
        {
            CambiarColor();
            debeMoverse = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Escudo1") || other.CompareTag("Escudo2"))
        {
            CambiarColor();
            debeMoverse = false;
        }
    }

    void CambiarColor()
    {
        // Usamos GetComponentInChildren porque el SkinnedMeshRenderer del humanoide está en un objeto hijo
        Renderer renderizador = GetComponentInChildren<Renderer>();
        if (renderizador != null)
        {
            renderizador.material.color = colorAlChocar;
        }
    }

    private void OnDestroy()
    {
        if (CompareTag("Tipo1"))
        {
            CuboNotificador.OnColisionHumanoideTipo2 -= IrAEscudoSeleccionado;
            CuboNotificador.OnColisionHumanoideTipo1 -= IrAEscudoTipo2;
        }
        else if (CompareTag("Tipo2"))
        {
            CuboNotificador.OnColisionHumanoideTipo1 -= IrAEscudoTipo2;
        }
    }
}
