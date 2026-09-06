using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class PantallaVictoria : MonoBehaviour
{
    public static PantallaVictoria instancia;

    [Header("PANEL")]
    public GameObject panel;

    [Header("TEXTO DEL TIEMPO")]
    public TMP_Text textoTiempo;

    [Header("ESTRELLAS (arrastrar las 3 imagenes en orden)")]
    public Image[] estrellas;
    public Sprite estrellaLlena;
    public Sprite estrellaVacia;

    [Header("UMBRALES DE TIEMPO DE ESTE NIVEL")]
    public float tiempoParaTresEstrellas = 20f;
    public float tiempoParaDosEstrellas = 35f;

    private string siguienteNivelGuardado;

    private void Awake()
    {
        instancia = this;

        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void Mostrar(float tiempoFinal, string siguienteNivel)
    {
        siguienteNivelGuardado = siguienteNivel;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (textoTiempo != null)
        {
            textoTiempo.text = "Tiempo: " + tiempoFinal.ToString("F2") + " s";
        }

        int cantidadEstrellas = CalcularEstrellas(tiempoFinal);
        MostrarEstrellas(cantidadEstrellas);

        Time.timeScale = 0f;
    }

    private int CalcularEstrellas(float tiempoFinal)
    {
        if (tiempoFinal <= tiempoParaTresEstrellas)
        {
            return 3;
        }
        else if (tiempoFinal <= tiempoParaDosEstrellas)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void MostrarEstrellas(int cantidad)
    {
        for (int i = 0; i < estrellas.Length; i++)
        {
            if (estrellas[i] == null)
                continue;

            estrellas[i].sprite = (i < cantidad) ? estrellaLlena : estrellaVacia;
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrASiguienteNivel()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(siguienteNivelGuardado))
        {
            SceneManager.LoadScene(siguienteNivelGuardado);
        }
        else
        {
            Debug.LogError("No hay siguiente nivel guardado para cargar.");
        }
    }
}