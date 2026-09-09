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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vagabundo") ||
            collision.gameObject.CompareTag("Rata"))
        {
            foreach (ContactPoint2D contacto in collision.contacts)
            {
                if (contacto.normal.y < -0.5f)
                {
                    collision.transform.SetParent(transform);
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vagabundo") ||
            collision.gameObject.CompareTag("Rata"))
        {
            collision.transform.SetParent(null);
        }
    }

}
