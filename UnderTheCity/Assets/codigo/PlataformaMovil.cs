using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private Transform objetivo;

    void Start()
    {
        objetivo = puntoB;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivo.position,
            velocidad * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, objetivo.position) < 0.1f)
        {
            objetivo = (objetivo == puntoA) ? puntoB : puntoA;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vagabundo") ||
            collision.gameObject.CompareTag("Rata"))
        {
            foreach (ContactPoint2D contacto in collision.contacts)
            {
                // El personaje está arriba de la plataforma
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