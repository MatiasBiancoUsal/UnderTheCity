using UnityEngine;

public class PantallaTacho : MonoBehaviour
{
    public GameObject canvasTerminos;

    private const string claveTerminos = "TerminosAceptados";

    private void Start()
    {
        if (canvasTerminos == null)
        {
            UnityEngine.Debug.LogError("No asignaste el Canvas de términos en PantallaTacho.");
            return;
        }

        if (PlayerPrefs.GetInt(claveTerminos, 0) == 1)
        {
            canvasTerminos.SetActive(false);
        }
        else
        {
            canvasTerminos.SetActive(true);
        }
    }

    public void Aceptar()
    {
        PlayerPrefs.SetInt(claveTerminos, 1);
        PlayerPrefs.Save();

        if (canvasTerminos != null)
        {
            canvasTerminos.SetActive(false);
        }
    }
}
