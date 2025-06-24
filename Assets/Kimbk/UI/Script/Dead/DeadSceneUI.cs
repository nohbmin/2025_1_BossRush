using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadSceneUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private string startSceneName = "StartScene";

    private void Start()
    {
        // 버튼에 클릭 이벤트 추가
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }
    }

    // 재시작 버튼 클릭 시 StartScene으로 이동
    public void OnRestartButtonClick()
    {
        try
        {
            // 메소드 1: 일반적인 방법
            SceneManager.LoadScene(startSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("씬 로드 실패. 대체 방법 시도중: " + e.Message);
            
            // 메소드 2: 빌드 인덱스로 로드 시도
            try
            {
                // 씬의 빌드 인덱스를 찾아서 로드 
                // (StartScene이 첫 번째 씬이라면 인덱스는 0)
                SceneManager.LoadScene(0);
            }
            catch
            {
                Debug.LogError("빌드 인덱스로도 씬 로드 실패");
                
                // 메소드 3: 비동기 로드 시도
                LoadSceneAsync();
            }
        }
    }
    
    private void LoadSceneAsync()
    {
        try
        {
            // 씬을 비동기적으로 로드 (부하 감소 및 로딩화면 구현시 유용)
            SceneManager.LoadSceneAsync(startSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("비동기 씬 로드도 실패: " + e.Message);
        }
    }
}