using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    
    [Header("UI Sound Effects")]
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    
    private AudioSource musicSource;
    private AudioSource sfxSource;
    
    private void Awake()
    {
        // 배경음악과 효과음을 위한 오디오소스 생성
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        
        // 배경음악 설정
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        
        // 배경음악 재생
        musicSource.Play();
    }
    
    public void PlayButtonHoverSound()
    {
        sfxSource.PlayOneShot(buttonHover);
    }
    
    public void PlayButtonClickSound()
    {
        sfxSource.PlayOneShot(buttonClick);
    }
}