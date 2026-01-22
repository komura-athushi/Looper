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
    public bool IsLoop => isLoop;
    protected bool isLoop = false;
}

[System.Serializable]
public class SoundEffectSoundData : SoundData
{
    public SoundEffectSoundData()
    {
        isLoop = false;
    }
}

[System.Serializable]
public class BGMData : SoundData
{
    public BGMData()
    {
        isLoop = true;
    }
}