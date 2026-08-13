using UnityEngine;

public class PlataformaSubeyBaja : MonoBehaviour
{
    public Transform posicionArriba;
    public Transform posicionAbajo;
    public float velocidad = 2f;

    public bool activada = false;

    void Update()
    {
        if (activada)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                posicionArriba.position,
                velocidad * Time.deltaTime
            );
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                posicionAbajo.position,
                velocidad * Time.deltaTime
            );
        }
    }
}
