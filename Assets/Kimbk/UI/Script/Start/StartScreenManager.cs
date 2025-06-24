using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // 오디오 믹서 사용을 위해 추가

public class StartScreenManager : MonoBehaviour
{
    // UI 요소들
    public Button startButton;
    public Button optionsButton;
    public Button exitButton;
    public Button closeOptionsButton;
    
    public GameObject optionsPanel; // 옵션 패널
    public Slider volumeSlider; // 볼륨 슬라이더
    
    public AudioMixer audioMixer; // 오디오 믹서 참조 (있을 경우)
    
    private void Start()
    {
        // 버튼 리스너 설정
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        exitButton.onClick.AddListener(ExitGame);
        closeOptionsButton.onClick.AddListener(CloseOptions);
        
        // 옵션 패널 초기 설정 - 비활성화
        optionsPanel.SetActive(false);
        
        // 볼륨 슬라이더 설정
        volumeSlider.onValueChanged.AddListener(SetVolume);
        
        // 초기 볼륨값 설정 (0에서 1 사이, 기본 0.75)
        volumeSlider.value = 0.75f;
    }
    
    // 게임 시작 버튼 클릭 시 호출
    void StartGame()
    {
        SceneManager.LoadScene("SampleScene"); // SampleScene으로 이동
    }
    
    // 옵션 버튼 클릭 시 호출
    void OpenOptions()
    {
        optionsPanel.SetActive(true); // 옵션 패널 표시
    }
    
    // 옵션 패널 닫기 버튼 클릭 시 호출
    void CloseOptions()
    {
        optionsPanel.SetActive(false); // 옵션 패널 숨김
    }
    
    // 볼륨 슬라이더 값 변경 시 호출
    void SetVolume(float volume)
    {
        // 볼륨 설정 - 일반적인 방법
        AudioListener.volume = volume;
    
        // 또는 오디오 믹서를 사용하는 경우 (음량을 로그 스케일로 변환)
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
    
        // 볼륨 설정 저장 (VolumeSettings 클래스 사용)
        VolumeSettings.SaveVolume(volume);
    }
    
    // 종료 버튼 클릭 시 호출
    void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
        #else
            Application.Quit(); // 빌드된 게임 종료
        #endif
    }
}