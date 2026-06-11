using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Niveles");
    }

    public void Nivel1()
    {
        SceneManager.LoadScene("nivel 1");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Controles()
    {
        SceneManager.LoadScene("Controles");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}