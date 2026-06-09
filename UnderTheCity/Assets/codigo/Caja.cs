using UnityEngine;

public class CajaSoloVagabundo : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool empujando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (!empujando)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        empujando = false; 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Vagabundo"))
        {
            empujando = true;
        }
    }
}