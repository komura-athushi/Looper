using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string id;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
    public bool isLoop = false;
}
