using UnityEngine;

public class CilindroNotificador : MonoBehaviour
{
    // Definimos el delegado y el evento que servirá para avisar a las esferas
    // cuando el cubo choque con este cilindro.
    public delegate void MensajeColision();
    public event MensajeColision OnColisionCubo;

    // Si el collider del cilindro es físico normal (sin Is Trigger)
    private void OnCollisionEnter(Collision collision)
    {
        // Comprobamos si quien nos ha chocado tiene el tag del jugador (el cubo)
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("El cubo ha chocado físicamente con el cilindro.");
            
            // Si hay alguna esfera suscrita al evento, lanzamos el aviso
            if (OnColisionCubo != null)
            {
                OnColisionCubo();
            }
        }
    }

    // Por si preferimos poner el cilindro en modo Trigger para que no nos frene al chocar
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El cubo ha entrado en el área del cilindro.");
            
            if (OnColisionCubo != null)
            {
                OnColisionCubo();
            }
        }
    }
}
