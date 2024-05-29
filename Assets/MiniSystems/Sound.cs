using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public static class Sound
{

    public static AudioSource PlaySoundAtPos(Vector3 target, AudioClip clip, float vol = 1f, bool play_right_away = true, bool sound2D = false, bool destroyAfter = false, float initialFadeDur = 0)
    {
        var AudioGO = new GameObject(clip.name);
        AudioGO.transform.parent = null;
        AudioGO.transform.position = target;
        AudioGO.transform.rotation = Quaternion.identity;
        var AudioS = AudioGO.AddComponent<AudioSource>();
        Play(AudioS, clip, vol, play_right_away, sound2D, destroyAfter, initialFadeDur);
        return AudioS;
    }

    public static AudioSource PlaySoundAtTarget(Transform target, AudioClip clip, float vol = 1f, bool play_right_away = true, bool sound2D = false, bool destroyAfter = false, float initialFadeDur = 0)
    {
        var AudioGO = new GameObject(clip.name);
        AudioGO.transform.parent = target;
        AudioGO.transform.localPosition = Vector3.zero;
        AudioGO.transform.localRotation = Quaternion.identity;
        var AudioS = AudioGO.AddComponent<AudioSource>();
        Play(AudioS, clip, vol, play_right_away, sound2D, destroyAfter, initialFadeDur);
        return AudioS;
    }

    public static void Play(AudioSource AudioS, AudioClip clip, float vol = 1f, bool play_right_away = true, bool sound2D = false, bool destroyAfter = false, float initialFadeDur = 0)
    {
        AudioS.clip = clip;
        AudioS.volume = vol;
        AudioS.spatialBlend = sound2D ? 0 : 1;
        if (play_right_away)
        {
            if (initialFadeDur > 0)
            {
                AudioS.volume = 0;
                AudioS.DOFade(vol, initialFadeDur);
            }
            AudioS.Play();
        }
        if (destroyAfter && play_right_away)
            DOVirtual.DelayedCall(clip.length + 0.1f + initialFadeDur, () => Object.Destroy(AudioS));
    }

    public static AudioSource CrossFade(AudioSource baseAS, AudioClip newClip, float duration, float customVolToFadeTo = -1, AnimationCurve fadeCurve = null)
    {
        var AS = baseAS.gameObject.AddComponent<AudioSource>();
        AS.volume = 0;
        AS.SetCustomCurve(AudioSourceCurveType.CustomRolloff, baseAS.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
        AS.clip = newClip;
        AS.Play();
        AS.gameObject.name = newClip.name;
        AS.spatialBlend = baseAS.spatialBlend;
        float targetVol = customVolToFadeTo < 0 ? baseAS.volume : customVolToFadeTo;
        float oldVol = baseAS.volume;
        if (fadeCurve == null)
        {
            AS.DOFade(targetVol, duration);
            baseAS.DOFade(0, duration);
        }
        else
        {
            DOVirtual.Float(0f, 1f, duration, (f) =>
            {
                AS.volume = fadeCurve.Evaluate(f) * targetVol;
            });
            DOVirtual.Float(1f, 0f, duration, (f) =>
            {
                baseAS.volume = fadeCurve.Evaluate(f) * oldVol;
            });
        }
        DOVirtual.DelayedCall(duration + 0.1f, () => Object.Destroy(baseAS));
        return AS;
    }
}
