using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SoundDatabase soundDatabase;

    private AudioSource[] _seAudioSourcePool;
    private AudioSource _bgmAudioSource;
    private int _currentSeIndex = 0;
    private int seAudioSourcePoolSize;
    private string _currentBgmId; // 現在再生中のBGMのID
    
    // ボリューム設定（0.0 ～ 1.0）
    private float _bgmVolume = 1.0f;
    private float _seVolume = 1.0f;
    
    // PlayerPrefs用のキー
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SE_VOLUME_KEY = "SEVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // ← シーン遷移時も破棄されない
    
        LoadVolumeSettings(); // ボリューム設定を読み込み
        InitializePool();
    }

    private void InitializePool()
    {
        // SE用のAudioSourceプールを初期化
        seAudioSourcePoolSize = soundDatabase.GetAudioSourcePoolSize();
        _seAudioSourcePool = new AudioSource[seAudioSourcePoolSize];
        for (int i = 0; i < seAudioSourcePoolSize; i++)
        {
            _seAudioSourcePool[i] = gameObject.AddComponent<AudioSource>();
        }
        
        // BGM用のAudioSourceを初期化
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string soundId)
    {
        SoundData data = soundDatabase.GetSound(soundId);

        // BGMかSEかを判定して適切なAudioSourceを使用
        if (data is BGMData)
        {
            PlayBGM(soundId);
        }
        else
        {
            PlaySE(soundId);
        }
    }

    public void PlaySoundWithPitch(string soundId, float pitchOverride)
    {
        SoundData data = soundDatabase.GetSound(soundId);
        if (data == null) return;

        // BGMかSEかを判定して適切なAudioSourceを使用
        if (data is BGMData)
        {
            PlayBGMWithPitch(soundId, pitchOverride);
        }
        else
        {
            PlaySEWithPitch(soundId, pitchOverride);
        }
    }

    public void PlaySE(string soundId)
    {
        SoundData data = soundDatabase.GetSound(soundId);

        AudioSource source = _seAudioSourcePool[_currentSeIndex];
        source.clip = data.clip;
        source.volume = data.volume * _seVolume; // SEボリュームを適用
        source.pitch = data.pitch;
        source.loop = data.IsLoop;
        source.Play();

        _currentSeIndex = (_currentSeIndex + 1) % seAudioSourcePoolSize;
    }

    public void PlaySEWithPitch(string soundId, float pitchOverride)
    {
        SoundData data = soundDatabase.GetSound(soundId);
        if (data == null || data.clip == null) return;

        AudioSource source = _seAudioSourcePool[_currentSeIndex];
        source.clip = data.clip;
        source.volume = data.volume * _seVolume; // SEボリュームを適用
        source.pitch = pitchOverride;
        source.loop = data.IsLoop;
        source.Play();

        _currentSeIndex = (_currentSeIndex + 1) % seAudioSourcePoolSize;
    }

    public void PlayBGM(string bgmId)
    {
        SoundData data = soundDatabase.GetSound(bgmId);

        _currentBgmId = bgmId; // 現在のBGM IDを保持
        _bgmAudioSource.clip = data.clip;
        _bgmAudioSource.volume = data.volume * _bgmVolume; // BGMボリュームを適用
        _bgmAudioSource.pitch = data.pitch;
        _bgmAudioSource.loop = data.IsLoop;
        _bgmAudioSource.Play();
    }

    public void PlayBGMWithPitch(string bgmId, float pitchOverride)
    {
        SoundData data = soundDatabase.GetSound(bgmId);
        if (data == null || data.clip == null) return;

        _currentBgmId = bgmId; // 現在のBGM IDを保持
        _bgmAudioSource.clip = data.clip;
        _bgmAudioSource.volume = data.volume * _bgmVolume; // BGMボリュームを適用
        _bgmAudioSource.pitch = pitchOverride;
        _bgmAudioSource.loop = data.IsLoop;
        _bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    public void PauseBGM()
    {
        _bgmAudioSource.Pause();
    }

    public void ResumeBGM()
    {
        _bgmAudioSource.UnPause();
    }
    
    // ボリューム調整メソッド
    public void SetBGMVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp01(volume);
        if (_bgmAudioSource.clip != null && !string.IsNullOrEmpty(_currentBgmId))
        {
            // 現在再生中のBGMの音量も更新
            SoundData data = soundDatabase.GetSound(_currentBgmId);
            if (data != null)
            {
                _bgmAudioSource.volume = data.volume * _bgmVolume;
            }
        }
        SaveVolumeSettings();
    }
    
    public void SetSEVolume(float volume)
    {
        _seVolume = Mathf.Clamp01(volume);
        // 再生中のSEの音量も更新
        foreach (var source in _seAudioSourcePool)
        {
            if (source.isPlaying && source.clip != null)
            {
                // SEのベース音量を取得して適用
                foreach (var se in soundDatabase.soundEffects)
                {
                    if (se.clip == source.clip)
                    {
                        source.volume = se.volume * _seVolume;
                        break;
                    }
                }
            }
        }
        SaveVolumeSettings();
    }
    
    public float GetBGMVolume()
    {
        return _bgmVolume;
    }
    
    public float GetSEVolume()
    {
        return _seVolume;
    }
    
    // ボリューム設定の保存と読み込み
    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, _bgmVolume);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, _seVolume);
        PlayerPrefs.Save();
    }
    
    private void LoadVolumeSettings()
    {
        _bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1.0f);
        _seVolume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, 1.0f);
    }
}
