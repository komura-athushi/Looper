using UnityEngine;
using System.Collections.Generic;

// Autio用のデータベースクラス
[CreateAssetMenu(menuName = "LOOPER/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    // サウンドデータのリスト
    [Tooltip("SEデータのリスト")]
    public List<SoundEffectSoundData> soundEffects = new();
    [Tooltip("BGMデータのリスト")]
    public List<BGMData> bgms = new();

    [Tooltip("オーディオソースプールのサイズ")]
    [SerializeField] private int audioSourcePoolSize = 5;

    public SoundData GetSound(string id)
    {
        foreach (var se in soundEffects)
        {
            if (se.id == id)
                return se;
        }

        foreach (var bgm in bgms)
        {
            if (bgm.id == id)
                return bgm;
        }
        return null;
    }

    public int GetAudioSourcePoolSize()
    {
        return audioSourcePoolSize;
    }
}
