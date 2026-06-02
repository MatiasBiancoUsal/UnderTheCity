using UnityEngine;

public class VagabundoControl : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;

    private Rigidbody rb;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float movimiento = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            movimiento = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movimiento = 1f;
        }

        rb.linearVelocity = new Vector3(movimiento * velocidad, rb.linearVelocity.y, 0);

        if (Input.GetKeyDown(KeyCode.W) && enSuelo)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, fuerzaSalto, 0);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            enSuelo = false;
        }
    }
}