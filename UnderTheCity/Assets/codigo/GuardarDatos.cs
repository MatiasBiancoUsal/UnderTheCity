using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardadoDatos : MonoBehaviour
{
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

    public void ResetearProgreso()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progreso reseteado");
    }
}