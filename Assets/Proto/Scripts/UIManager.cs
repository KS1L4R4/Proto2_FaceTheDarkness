using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; //Ya esta el singletonto >:D
    private GameObject mainMenuScreen;
    private GameObject chapterSelectionScreen;
    private GameObject levelSelectionScreen;
    private GameObject pauseMenuScreen;
    private GameObject defeatScreen;
    private GameObject victoryScreen;
    private GameObject exitWarningScreen;
    private GameObject characterSelectionScreen;
    private GameObject storeScreen;

    private GameObject previousScreen;
    private GameObject currentScreen;

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
        exitWarningScreen = GameObject.Find("UIManager/UI_ExitWarningScreen");
        characterSelectionScreen = GameObject.Find("UIManager/UI_CharacterSelectionScreen");
        storeScreen = GameObject.Find("UIManager/UI_StoreScreen");

        exitWarningScreen.SetActive(false);
        chapterSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        characterSelectionScreen.SetActive(false);
        storeScreen.SetActive(false);

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
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowMainScreen() //Abrir la pantalla (UI) del menú principal
    {
        
    }

    public void ShowExitWarning()
    {
        exitWarningScreen.SetActive(true);
    }

    public void HideExitWarning()
    {
        exitWarningScreen.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Game was closed");
    }

    public void ShowChapterSelectionScreen()
    {
        if (mainMenuScreen != null && chapterSelectionScreen != null)
        {
            TransitionScreenEffect(mainMenuScreen, chapterSelectionScreen);
        }
        UpdateGoBack(chapterSelectionScreen, mainMenuScreen);
    }

    public void ShowLevelSelectionScreen()
    {
        if (chapterSelectionScreen != null && levelSelectionScreen != null)
        {
            TransitionScreenEffect(chapterSelectionScreen, levelSelectionScreen);
        }
        UpdateGoBack(levelSelectionScreen, chapterSelectionScreen);
    }

    public void ShowCharacterSelectionScreen()
    {
        if (mainMenuScreen != null && characterSelectionScreen != null)
        {
            TransitionScreenEffect(mainMenuScreen, characterSelectionScreen);
        }
        UpdateGoBack(characterSelectionScreen, mainMenuScreen);
    }

    public void ShowStoreScreen()
    {
        if (mainMenuScreen != null && storeScreen != null)
        {
            TransitionScreenEffect(mainMenuScreen, storeScreen);
        }
        UpdateGoBack(storeScreen, mainMenuScreen);
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

    public void GoBack()
    {
        TransitionScreenEffect(currentScreen, previousScreen);
    }

    public void UpdateGoBack(GameObject current, GameObject previous)
    {
        currentScreen = current;
        previousScreen = previous;
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
