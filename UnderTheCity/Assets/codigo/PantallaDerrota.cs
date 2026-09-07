using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PantallaDerrota : MonoBehaviour
{
    public static PantallaDerrota instancia;

    [Header("PANEL")]
    public GameObject panel;

    [Header("ESCENA DEL MENU PRINCIPAL")]
    public string escenaMenu;

    private void Awake()
    {
        instancia = this;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Mostrar()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAMenu()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(escenaMenu))
        {
            SceneManager.LoadScene(escenaMenu);
        }
        else
        {
            Debug.LogError("No asignaste el campo 'Escena Menu' en PantallaDerrota.");
        }
    }
}
