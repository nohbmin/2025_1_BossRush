using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    // 볼륨 설정을 저장할 키
    private const string VolumeKey = "GameVolume";
    
    // 초기 볼륨 설정
    private void Start()
    {
        // 이전에 저장된 볼륨 설정이 있으면 불러오기
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.75f);
        AudioListener.volume = savedVolume;
    }
    
    // 볼륨 설정을 저장하는 메서드 (StartScreenManager의 SetVolume에서 호출)
    public static void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }
}