using UnityEngine;

public class Ventilador : MonoBehaviour
{
    public float fuerza = 10f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Rata"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerza);
            }
        }
    }
}