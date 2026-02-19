using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private GameObject mainMenuScreen;
    private GameObject chapterSelectionScreen;
    private GameObject levelSelectionScreen;
    private GameObject pauseMenuScreen;
    private GameObject defeatScreen;
    private GameObject victoryScreen;

    public Image transitionImage;

    void Start()
    {
        DontDestroyOnLoad(this);
        mainMenuScreen = GameObject.Find("UIManager/UI_MainMenu");
        chapterSelectionScreen = GameObject.Find("UIManager/UI_ChapterSelectionScreen");
        levelSelectionScreen = GameObject.Find("UIManager/UI_LevelSelectionScreen");
        transitionImage.GetComponent<CanvasGroup>().alpha = 0f;
        transitionImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void TransitionScreenEffect()
    {
        Sequence s = DOTween.Sequence();
        transitionImage.GetComponent<CanvasGroup>().blocksRaycasts = true;
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f));
        s.AppendInterval(1f);
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().DOFade(0, 0.7f));
        transitionImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OpenMainMenuScene() //Abrir la escena del menú principal
    {
        mainMenuScreen = GameObject.Find("UIManager/UI_MainMenu");
        chapterSelectionScreen = GameObject.Find("UIManager/UI_ChapterSelectionScreen");
        levelSelectionScreen = GameObject.Find("UIManager/UI_LevelSelectionScreen");
    }

    public void OpenMainScreen() //Abrir la pantalla (UI) del menú principal
    {
        
    }

    public void ShowExitWarning()
    {
        
    }

    public void OpenChapterSelectionScreen()
    {
        if(mainMenuScreen != null)
        {
            
        }
        if(chapterSelectionScreen != null)
        {
            
        }
    }

    public void OpenLevelSelectionScreen()
    {

    }

    public void ShowDefeatScreen()
    {
        
    }

    public void ShowVictoryScreen()
    {
        
    }

    public void ShowMenuScreen()
    {
        
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
