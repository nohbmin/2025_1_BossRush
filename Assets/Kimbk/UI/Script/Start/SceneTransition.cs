using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    public Image fadePanel;
    public float transitionDuration = 1f;
    
    private static SceneTransition instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 초기 설정
            fadePanel.color = new Color(0, 0, 0, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static void LoadScene(string sceneName)
    {
        instance.StartCoroutine(instance.LoadSceneWithTransition(sceneName));
    }
    
    private IEnumerator LoadSceneWithTransition(string sceneName)
    {
        // 화면을 검은색으로 페이드
        fadePanel.DOFade(1f, transitionDuration);
        
        yield return new WaitForSeconds(transitionDuration);
        
        // 씬 로드
        SceneManager.LoadScene(sceneName);
        
        // 화면 페이드 인
        fadePanel.DOFade(0f, transitionDuration);
    }
}