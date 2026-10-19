using UnityEngine;

public class MovimientoCubo : MonoBehaviour
{
    public float velocidad = 6.0f;

    void Update()
    {
        // Leemos las teclas WASD o las flechas de dirección
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        // Calculamos el desplazamiento en el plano horizontal y movemos el cubo
        Vector3 movimiento = new Vector3(movX, 0f, movZ) * velocidad * Time.deltaTime;
        transform.Translate(movimiento, Space.World);
    }
}
