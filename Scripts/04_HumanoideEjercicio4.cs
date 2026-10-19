using UnityEngine;

public class HumanoideEjercicio4 : MonoBehaviour
{
    [Header("Objetivos Ejercicio 4")]
    [Tooltip("Para Tipo 1: Escudo al que se teletransportan de golpe")]
    public Transform escudoObjetivo;

    [Tooltip("Para Tipo 2: Objeto de la escena hacia el que se quedan mirando")]
    public Transform objetoParaOrientarse;

    void OnEnable()
    {
        ReferenciaNotificador.OnCuboCerca += ReaccionarAAproximacion;
    }

    void OnDisable()
    {
        ReferenciaNotificador.OnCuboCerca -= ReaccionarAAproximacion;
    }

    void ReaccionarAAproximacion()
    {
        if (CompareTag("Tipo1"))
        {
            // Los del grupo 1 se teletransportan directamente a la posición del escudo objetivo
            if (escudoObjetivo != null)
            {
                transform.position = escudoObjetivo.position;
            }
        }
        else if (CompareTag("Tipo2"))
        {
            // Los del grupo 2 giran sobre sí mismos para mirar hacia el objeto indicado
            if (objetoParaOrientarse != null)
            {
                Vector3 puntoAMirar = new Vector3(
                    objetoParaOrientarse.position.x,
                    transform.position.y,
                    objetoParaOrientarse.position.z
                );

                transform.LookAt(puntoAMirar);
            }
        }
    }
}
