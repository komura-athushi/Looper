using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SoundDatabase soundDatabase;

    private AudioSource[] _audioSourcePool;
    private int _currentIndex = 0;
    private int audioSourcePoolSize;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // ← シーン遷移時も破棄されない
    
        InitializePool();
    }

    private void InitializePool()
    {
        audioSourcePoolSize = soundDatabase.GetAudioSourcePoolSize();
        _audioSourcePool = new AudioSource[audioSourcePoolSize];
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            _audioSourcePool[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string soundId)
    {
        SoundData data = soundDatabase.GetSound(soundId);
        if (data == null || data.clip == null)
        {
            Debug.LogWarning($"Sound '{soundId}' not found!");
            return;
        }

        AudioSource source = _audioSourcePool[_currentIndex];
        source.clip = data.clip;
        source.volume = data.volume;
        source.pitch = data.pitch;
        source.loop = data.isLoop;
        source.Play();

        _currentIndex = (_currentIndex + 1) % audioSourcePoolSize;
    }

    public void PlaySoundWithPitch(string soundId, float pitchOverride)
    {
        SoundData data = soundDatabase.GetSound(soundId);
        if (data == null) return;

        AudioSource source = _audioSourcePool[_currentIndex];
        source.clip = data.clip;
        source.volume = data.volume;
        source.pitch = pitchOverride;
        source.Play();

        _currentIndex = (_currentIndex + 1) % audioSourcePoolSize;
    }
}
