using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class ControladorVideoFinal : MonoBehaviour
{
    [Header("VIDEO PLAYER")]
    public VideoPlayer videoPlayer;

    [Header("ESCENA DEL MENU")]
    public string escenaMenu;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoTerminado;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No se asignó el VideoPlayer en ControladorVideoFinal.");
        }
    }

    private void OnVideoTerminado(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(escenaMenu))
        {
            SceneManager.LoadScene(escenaMenu);
        }
        else
        {
            Debug.LogError("No asignaste el campo 'Escena Menu' en ControladorVideoFinal.");
        }
    }
}