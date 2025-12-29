using UnityEngine;
using System.Collections.Generic;

// Autio用のデータベースクラス
[CreateAssetMenu(menuName = "LOOPER/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [SerializeField] private List<SoundData> sounds = new();

    public SoundData GetSound(string id)
    {
        return sounds.Find(s => s.id == id);
    }
}
