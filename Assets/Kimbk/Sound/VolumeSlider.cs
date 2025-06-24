using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    
    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        
        if (volumeSlider == null)
        {
            Debug.LogError("VolumeSlider: 슬라이더 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        
        // AudioManager 인스턴스 확인 - 여기서 오류 발생 가능성 높음
        if (AudioManager.instance != null)
        {
            // 슬라이더 초기값 설정
            volumeSlider.value = AudioManager.instance.GetMasterVolume();
            
            // 슬라이더 이벤트 리스너 등록
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogWarning("VolumeSlider: AudioManager 인스턴스를 찾을 수 없습니다.");
            
            // AudioManager가 없을 경우 기본값으로 설정
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            
            // AudioManager가 나중에 로드될 가능성을 고려하여 이벤트 리스너는 여전히 등록
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }
    
    public void OnSliderValueChanged(float value)
    {
        // AudioManager 존재 확인 후 볼륨 설정
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(value);
        }
        else
        {
            // AudioManager가 없을 경우 PlayerPrefs에 직접 저장
            PlayerPrefs.SetFloat("MasterVolume", value);
            PlayerPrefs.Save();
        }
    }
}