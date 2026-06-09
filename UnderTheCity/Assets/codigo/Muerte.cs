using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Vagabundo") || collision.CompareTag("Rata"))
        {
            ReiniciarNivel();
        }
    }

    void ReiniciarNivel()
    {
        int indiceActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indiceActual);
    }
}