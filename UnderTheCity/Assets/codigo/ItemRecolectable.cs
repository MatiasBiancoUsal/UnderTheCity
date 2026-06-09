using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public string tipo; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (tipo == "cerveza" && collision.CompareTag("Vagabundo"))
        {
            GameManager.instancia.cervezas++;
            Destroy(gameObject);
        }

        
        if (tipo == "queso" && collision.CompareTag("Rata"))
        {
            GameManager.instancia.quesos++;
            Destroy(gameObject);
        }
    }
}