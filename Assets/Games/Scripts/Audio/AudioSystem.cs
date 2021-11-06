using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private Transform audioHolder;
    [SerializeField] private AudioLibrary audioLibrary;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float bgmVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    public static AudioSystem Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
            bgmSource = audioHolder.gameObject.AddComponent<AudioSource>();
            bgmSource.volume = bgmVolume;
            sfxSource = audioHolder.gameObject.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayBGM(string bgmName)
    {
        if (audioLibrary.TryGetAudioBGM(bgmName, out AudioClip clip))
        {
            bgmSource.volume = bgmVolume;
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(string sfxName)
    {
        if (audioLibrary.TryGetAudioSFX(sfxName, out AudioClip clip))
        {
            sfxSource.volume = sfxVolume;
            sfxSource.PlayOneShot(clip);
        }
    }
}
