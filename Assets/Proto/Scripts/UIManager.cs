using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //Ya esta el singletonto >:D
    private GameObject mainMenuScreen;
    private GameObject chapterSelectionScreen;
    private GameObject levelSelectionScreen;
    private GameObject pauseMenuScreen;
    private GameObject defeatScreen;
    private GameObject victoryScreen;

    public Image transitionImage;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        mainMenuScreen = GameObject.Find("UIManager/UI_MainMenuScreen");
        chapterSelectionScreen = GameObject.Find("UIManager/UI_ChapterSelectionScreen");
        levelSelectionScreen = GameObject.Find("UIManager/UI_LevelSelectionScreen");

        chapterSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);

        transitionImage.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public void TransitionScreenEffect(GameObject screenToHide, GameObject screenToShow)
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f));
        s.AppendInterval(0.5f);
        s.AppendCallback(() => screenToHide.SetActive(false));
        s.AppendCallback(() => screenToShow.SetActive(true));
        s.AppendInterval(0.5f);
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().DOFade(0, 0.7f));
        transitionImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OpenMainMenuScene() //Abrir la escena del menú principal
    {
        
    }

    public void OpenMainScreen() //Abrir la pantalla (UI) del menú principal
    {
        
    }

    public void ShowExitWarning()
    {
        
    }

    public void OpenChapterSelectionScreen()
    {
        if (mainMenuScreen != null && chapterSelectionScreen != null)
        {
            TransitionScreenEffect(mainMenuScreen, chapterSelectionScreen);
        }
    }

    public void OpenLevelSelectionScreen()
    {
        if (chapterSelectionScreen != null && levelSelectionScreen != null)
        {
            TransitionScreenEffect(chapterSelectionScreen, levelSelectionScreen);
        }
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
