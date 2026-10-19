using UnityEngine;

public class ReferenciaNotificador : MonoBehaviour
{
    // Evento estático que se lanza cuando el cubo entra en el radio de aproximación
    public delegate void AccionAproximacion();
    public static event AccionAproximacion OnCuboCerca;

    // El objeto de referencia tiene un Collider amplio con "Is Trigger" activado
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El cubo se ha aproximado al objeto de referencia.");
            if (OnCuboCerca != null)
            {
                OnCuboCerca();
            }
        }
    }
}
