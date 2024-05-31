using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlaySFXMethod(AudioClip clip)
    {
        Sound.PlaySoundAtPos(Vector3.zero, clip, Sound.MixerTypes.SFX, sound2D: true, destroyAfter: true);
    }
}
