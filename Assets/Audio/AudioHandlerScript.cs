using UnityEngine;
using UnityEngine.UI;

public class AudioHandlerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string param;
    [SerializeField] private Slider settingsSlider;

    public void changeVolume(float a)
    {
        Sound.GetMixer(0).audioMixer.SetFloat(param, a);
        PlayerPrefs.SetFloat($"Audio_{param}", a);
    }
        

    void Start()
    {
        float initValue = PlayerPrefs.GetFloat($"Audio_{param}", 0);
        // changeVolume(initValue);
        settingsSlider.value = initValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
