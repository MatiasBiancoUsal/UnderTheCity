using UnityEngine;
using UnityEngine.Video;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(VideoPlayer))]
public class IntroNivel : MonoBehaviour
{
    [SerializeField] private GameObject contenidoDelNivel;

    private static bool introYaVista;

    private VideoPlayer video;
    private bool introTerminada;

    [RuntimeInitializeOnLoadMethod(
        RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ReiniciarEstado()
    {
        introYaVista = false;
    }

    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.playOnAwake = false;
        video.isLooping = false;
        video.loopPointReached += TerminarIntro;
    }

    private void Start()
    {
        if (introYaVista)
        {
            TerminarIntro(video);
            return;
        }

        video.Play();
    }

    private void Update()
    {
        if (introTerminada) return;

#if ENABLE_INPUT_SYSTEM
        bool espacioPresionado = Keyboard.current != null
            && Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        bool espacioPresionado = Input.GetKeyDown(KeyCode.Space);
#endif

        if (espacioPresionado)
        {
            TerminarIntro(video);
        }
    }

    private void TerminarIntro(VideoPlayer reproductor)
    {
        if (introTerminada) return;

        introTerminada = true;
        introYaVista = true;

        video.Stop();
        contenidoDelNivel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        video.loopPointReached -= TerminarIntro;
    }
}