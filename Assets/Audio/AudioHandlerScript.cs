using UnityEngine;
using UnityEngine.UI;

public class AudioHandlerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string param;

    public void changeVolume(float a)
    {
        Sound.GetMixer(0).audioMixer.SetFloat(param, a);    
    }
        

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
