using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    // 기존 BGM 오디오 소스
    private AudioSource startSceneBGM;
    private AudioSource bossBGM;
    private AudioSource deadSceneBGM;
    
    // 효과음을 위한 오디오 소스 추가
    private AudioSource sfxAudioSource;
    
    // 오디오 클립
    [SerializeField] private AudioClip startSceneClip;
    [SerializeField] private AudioClip bossClip;
    [SerializeField] private AudioClip deadSceneClip;
    
    // 효과음 클립
    [SerializeField] private AudioClip buttonClickSound; // 버튼 클릭 효과음
    
    // 볼륨 설정
    private float masterVolume = 1.0f;
    private float sfxVolume = 1.0f;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void SetupAudioSources()
    {
        // BGM 오디오 소스 설정
        startSceneBGM = gameObject.AddComponent<AudioSource>();
        startSceneBGM.clip = startSceneClip;
        startSceneBGM.loop = true;
        startSceneBGM.playOnAwake = false;
        
        bossBGM = gameObject.AddComponent<AudioSource>();
        bossBGM.clip = bossClip;
        bossBGM.loop = true;
        bossBGM.playOnAwake = false;
        
        deadSceneBGM = gameObject.AddComponent<AudioSource>();
        deadSceneBGM.clip = deadSceneClip;
        deadSceneBGM.loop = true;
        deadSceneBGM.playOnAwake = false;
        
        // 효과음 오디오 소스 설정
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.loop = false;
        sfxAudioSource.playOnAwake = false;
        
        // 볼륨 설정 불러오기
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        
        // 볼륨 적용
        ApplyVolume();
    }
    
    // 볼륨 적용
    private void ApplyVolume()
    {
        if (startSceneBGM != null) startSceneBGM.volume = masterVolume;
        if (bossBGM != null) bossBGM.volume = masterVolume;
        if (deadSceneBGM != null) deadSceneBGM.volume = masterVolume;
        if (sfxAudioSource != null) sfxAudioSource.volume = sfxVolume * masterVolume;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에 따른 BGM 재생
        if (scene.name == "StartScene")
        {
            PlayStartSceneBGM();
        }
        else if (scene.name == "SampleScene")
        {
            PlayBossBGM();
        }
        else if (scene.name == "DeadScene")
        {
            PlayDeadSceneBGM();
        }
    }
    
    // BGM 메서드들은 그대로...
    public void PlayStartSceneBGM()
    {
        StopAllBGM();
        if (startSceneBGM.clip != null)
            startSceneBGM.Play();
    }
    
    public void PlayBossBGM()
    {
        StopAllBGM();
        if (bossBGM.clip != null)
            bossBGM.Play();
    }
    
    public void PlayDeadSceneBGM()
    {
        StopAllBGM();
        if (deadSceneBGM.clip != null)
            deadSceneBGM.Play();
    }
    
    private void StopAllBGM()
    {
        if (startSceneBGM.isPlaying) startSceneBGM.Stop();
        if (bossBGM.isPlaying) bossBGM.Stop();
        if (deadSceneBGM.isPlaying) deadSceneBGM.Stop();
    }
    
    // 효과음 메서드 추가
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }
    
    // 일반 효과음 재생 메서드
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
    }
    
    // 볼륨 설정 메서드들
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        ApplyVolume();
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        ApplyVolume();
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
    
    public float GetMasterVolume()
    {
        return masterVolume;
    }
    
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}