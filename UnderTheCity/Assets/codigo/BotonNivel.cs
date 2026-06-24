using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BotonNivel : MonoBehaviour
{
    public Button[] botones;
    public GameObject[] candados;
    public GameObject[] textos;

    void Start()
    {
        Desbloquear();
    }

    public void CargarNivel(int numeroNivel)
    {
        SceneManager.LoadScene(numeroNivel);
    }

    public void Desbloquear()
    {
        int nivelDesbloqueado = PlayerPrefs.GetInt("NivelDesbloqueado", 1);

        for (int i = 0; i < botones.Length; i++)
        {
            bool desbloqueado = i + 1 <= nivelDesbloqueado;

            botones[i].interactable = desbloqueado;

            if (i < candados.Length && candados[i] != null)
                candados[i].SetActive(!desbloqueado);

            if (i < textos.Length && textos[i] != null)
                textos[i].SetActive(desbloqueado);
        }
    }
}