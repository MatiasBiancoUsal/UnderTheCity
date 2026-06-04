using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public bool vagabundoListo = false;
    public bool rataLista = false;

    public string siguienteNivel;

    bool yaCargo = false;

    void Awake()
    {
        instancia = this;
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            int indiceActual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(indiceActual);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Saliendo del juego...");
            Application.Quit();
        }

        if (!yaCargo && vagabundoListo && rataLista)
        {
            yaCargo = true;
            SceneManager.LoadScene(siguienteNivel);
        }
    }
}