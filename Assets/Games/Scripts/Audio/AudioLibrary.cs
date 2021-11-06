using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Audio Libraries", fileName = "Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField] private List<AudioData> bgmLib = new List<AudioData>();
    [SerializeField] private List<AudioData> sfxLib = new List<AudioData>();

    public bool TryGetAudioBGM(string id, out AudioClip clip)
    {
        if (bgmLib.Exists(x => x.id.Equals(id)))
        {
            clip = bgmLib.Find(x => x.id.Equals(id)).clip;
            return true;
        }

        clip = null;
        return false;
    }

    public bool TryGetAudioSFX(string id, out AudioClip clip)
    {
        if (sfxLib.Exists(x => x.id.Equals(id)))
        {
            clip = sfxLib.Find(x => x.id.Equals(id)).clip;
            return true;
        }

        clip = null;
        return false;
    }

    [System.Serializable]
    public struct AudioData
    {
        public string id;
        public AudioClip clip;
    }
}
