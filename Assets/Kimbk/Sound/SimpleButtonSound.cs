using UnityEngine;
using UnityEngine.UI;

public class SimpleButtonSound : MonoBehaviour
{
    // Inspector에서 할당할 오디오 클립
    [SerializeField] private AudioClip clickSound;
    
    // 볼륨 설정
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;
    
    // AudioSource 컴포넌트
    private AudioSource audioSource;
    
    private void Awake()
    {
        // 오디오 소스 컴포넌트 추가
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }
    
    private void Start()
    {
        // 버튼 컴포넌트 가져오기
        Button button = GetComponent<Button>();
        
        // 버튼 클릭 이벤트에 사운드 재생 함수 추가
        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
        else
        {
            Debug.LogError("SimpleButtonSound: 버튼 컴포넌트를 찾을 수 없습니다!");
        }
    }
    
    // 클릭 사운드 재생 함수
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            // 사운드 재생
            audioSource.PlayOneShot(clickSound);
            Debug.Log("버튼 클릭 사운드 재생 시도");
        }
        else
        {
            Debug.LogWarning("SimpleButtonSound: 클릭 사운드 또는 AudioSource가 없습니다!");
        }
    }
}