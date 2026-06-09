using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuDeJuego : MonoBehaviour
{
    public GameObject canvasControles;

    private bool menuActivo = false;

    void Start()
    {
        canvasControles.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            menuActivo = !menuActivo;

            canvasControles.SetActive(menuActivo);

            if (menuActivo)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void VolverAlMenu()
    {
        Debug.Log("CLICK FUNCIONA"); 
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
}