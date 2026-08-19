using UnityEngine;
using UnityEngine.UI;

public class ControlVolumen : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.onValueChanged.AddListener(CambiarVolumen);
    }

    void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;
    }
}
