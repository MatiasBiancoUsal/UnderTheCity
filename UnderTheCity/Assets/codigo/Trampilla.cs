using UnityEngine;

public class Trampilla : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private bool activada = false;

    void Update()
    {
        if (activada)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                puntoB.position,
                velocidad * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                puntoA.position,
                velocidad * Time.deltaTime
            );
        }
    }

    public void Activar()
    {
        activada = !activada;
    }
}
