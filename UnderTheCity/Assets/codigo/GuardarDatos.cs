using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardadoDatos : MonoBehaviour
{
    [Header("Canvas de Términos y Condiciones")]
    public GameObject canvasTerminos;

    private void Start()
    {
        if (canvasTerminos == null)
        {
            GameObject objetoCanvas = GameObject.Find("CanvasTerminos");

            if (objetoCanvas != null)
            {
                canvasTerminos = objetoCanvas;
            }
        }

        if (canvasTerminos == null)
        {
            Debug.LogWarning("No se encontró el Canvas de Términos y Condiciones en esta escena.");
            return;
        }

        if (PlayerPrefs.GetInt("TerminosAceptados", 0) == 1)
        {
            canvasTerminos.SetActive(false);
        }
        else
        {
            canvasTerminos.SetActive(true);
        }
    }

    public void GuardarNivel(int nivelCompletado)
    {
        int nivelDesbloqueado = PlayerPrefs.GetInt("NivelDesbloqueado", 1);

        if (nivelCompletado >= nivelDesbloqueado)
        {
            PlayerPrefs.SetInt("NivelDesbloqueado", nivelCompletado + 1);
            PlayerPrefs.Save();

            Debug.Log("Nuevo nivel desbloqueado: " + (nivelCompletado + 1));
        }
    }

    public void AceptarTerminos()
    {
        PlayerPrefs.SetInt("TerminosAceptados", 1);
        PlayerPrefs.Save();

        if (canvasTerminos != null)
        {
            canvasTerminos.SetActive(false);
        }

        Debug.Log("Términos y condiciones aceptados");
    }

    public bool TerminosFueronAceptados()
    {
        return PlayerPrefs.GetInt("TerminosAceptados", 0) == 1;
    }

    public void ResetearProgreso()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Progreso reseteado");

        if (canvasTerminos != null)
        {
            canvasTerminos.SetActive(true);
        }
    }
}