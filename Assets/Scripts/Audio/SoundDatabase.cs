using UnityEngine;
using System.Collections.Generic;
using UnityEditor.EditorTools;

// Autio用のデータベースクラス
[CreateAssetMenu(menuName = "LOOPER/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    // サウンドデータのリスト
    [Tooltip("サウンドデータのリスト")]
    [SerializeField] private List<SoundData> sounds = new();

    [Tooltip("オーディオソースプールのサイズ")]
    [SerializeField] private int audioSourcePoolSize = 5;

    public SoundData GetSound(string id)
    {
        return sounds.Find(s => s.id == id);
    }

    public int GetAudioSourcePoolSize()
    {
        return audioSourcePoolSize;
    }
}
