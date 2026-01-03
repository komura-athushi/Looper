using UnityEngine;

[System.Serializable]
public class SoundData
{
    [Tooltip("サウンドのID(識別名)")]
    public string id;
    [Tooltip("サウンドソース")]
    public AudioClip clip;
    [Tooltip("ボリューム")]
    [Range(0f, 1f)] public float volume = 1f;
    [Tooltip("ピッチ")]
    [Range(0.5f, 1.5f)] public float pitch = 1f;
    [Tooltip("trueならループ再生")]
    public bool isLoop = false;
}
