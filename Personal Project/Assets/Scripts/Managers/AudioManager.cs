using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float volume = 1;

    private AudioSource mainAudioSource;
    private AudioSource nextAudioSource;

    private AudioSource attackAudioSource;

    [SerializeField] float transitionTime;

    [SerializeField] List<AudioClip> audioClip = new();

    [SerializeField] List<AudioClip> attackAudioClip = new();

    // Start is called before the first frame update
    void Start()
    {
        mainAudioSource = gameObject.AddComponent<AudioSource>();
        mainAudioSource.loop = true;
        nextAudioSource = gameObject.AddComponent<AudioSource>();
        nextAudioSource.loop = true;

        attackAudioSource = gameObject.AddComponent<AudioSource>();
        attackAudioSource.loop = false;

        mainAudioSource.volume = GameManager.Instance.OptionsManager.volume;
        nextAudioSource.volume = 0;

        mainAudioSource.clip = audioClip[0];
        PlayAudio(mainAudioSource);
    }

    void PlayAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }

   public void TransitionClip(int nextClip)
    {
        if (mainAudioSource.volume < volume)
        {
            mainAudioSource.clip = audioClip[nextClip];

            StartCoroutine(VolumeTransition(nextAudioSource,mainAudioSource));
        }
        else
        {
            nextAudioSource.clip = audioClip[nextClip];

            StartCoroutine(VolumeTransition(mainAudioSource,nextAudioSource));
        }

    }
    IEnumerator VolumeTransition(AudioSource last, AudioSource next)
    {
       
        next.volume = 0;
        next.Play();

        float time = 0;
        while (next.volume < volume)
        {
            time += Time.deltaTime;

            last.volume = Easing.BaseEasing(volume, 0, time / transitionTime, Easing.EaseOutCubic);
            next.volume = Easing.BaseEasing(0, volume, time / transitionTime, Easing.EaseInQuart);

            yield return new WaitForEndOfFrame();
        }

        last.Stop();

        yield return null;
    }

    public void Mute(bool _mute)
    {
        mainAudioSource.mute = _mute;
        nextAudioSource.mute = _mute;
        attackAudioSource.mute = _mute;
    }

    public void SetVolume(float _volume)
    {
        volume = _volume;
        mainAudioSource.volume = volume;
        attackAudioSource.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void PlayAttackAudio(int attackType)
    {
        attackAudioSource.clip = attackAudioClip[attackType];

        attackAudioSource.volume = volume;
        attackAudioSource.Play();
    }
}
