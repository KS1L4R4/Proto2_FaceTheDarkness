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
    private GameObject pauseMenuScreenBack;
    private GameObject pauseScreen;
    private GameObject defeatScreen;
    private GameObject victoryScreen;
    private GameObject exitWarningScreen;
    private GameObject characterSelectionScreen;
    private GameObject storeScreen;

    private GameObject previousScreen;
    private GameObject currentScreen;

    public Image transitionImage;

    public bool pause;

    private void Awake()
    {

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
        pauseMenuScreenBack = GameObject.Find("UIManager/UI_PauseMenuScreenBGND");
        pauseScreen = GameObject.Find("UIManager/UI_PauseMenuScreen");

        exitWarningScreen.SetActive(false);
        chapterSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        characterSelectionScreen.SetActive(false);
        storeScreen.SetActive(false);
        pauseMenuScreenBack.SetActive(false);
        pauseScreen.SetActive(false);
        pause = false;

        transitionImage.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(pause == false)
                {
                    ShowMenuScreen();
                }
                else
                {
                    HideMenuScreen();
                }
            }
        }
    }

    public void TransitionScreenEffect(GameObject screenToHide, GameObject screenToShow)
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().blocksRaycasts = true);
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().DOFade(1, 0.5f));
        s.AppendInterval(0.5f);
        s.AppendCallback(() => screenToHide.SetActive(false));
        s.AppendCallback(() => screenToShow.SetActive(true));
        s.AppendInterval(0.5f);
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().DOFade(0, 0.7f));
        s.AppendCallback(() => transitionImage.GetComponent<CanvasGroup>().blocksRaycasts = false);
    }

    public void OpenMainMenuScene() //Abrir la escena del menú principal
    {
        SceneManager.LoadScene("MainMenu");

        exitWarningScreen.SetActive(false);
        chapterSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        characterSelectionScreen.SetActive(false);
        storeScreen.SetActive(false);
        pauseMenuScreenBack.SetActive(false);
        pauseScreen.SetActive(false);
        pause = false;
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
    }

    /*public void ShowChapterSelectionScreen()
    {
        if (mainMenuScreen != null && chapterSelectionScreen != null)
        {
            currentScreen = chapterSelectionScreen;
            previousScreen = mainMenuScreen;
            TransitionScreenEffect(mainMenuScreen, chapterSelectionScreen);
        }
    }*/

    public void ShowLevelSelectionScreen()
    {
        if (mainMenuScreen != null && levelSelectionScreen != null)
        {
            currentScreen = levelSelectionScreen;
            previousScreen = mainMenuScreen;
            TransitionScreenEffect(mainMenuScreen, levelSelectionScreen);
        }
    }

    public void ShowCharacterSelectionScreen()
    {
        if (mainMenuScreen != null && characterSelectionScreen != null)
        {
            currentScreen = characterSelectionScreen;
            previousScreen = mainMenuScreen;
            TransitionScreenEffect(mainMenuScreen, characterSelectionScreen);
        }
    }

    public void ShowStoreScreen()
    {
        if (mainMenuScreen != null && storeScreen != null)
        {
            currentScreen = storeScreen;
            previousScreen = mainMenuScreen;
            TransitionScreenEffect(mainMenuScreen, storeScreen);
        }
    }

    public void OpenFirstLevel()
    {
        levelSelectionScreen.SetActive(false);
        SceneManager.LoadScene("Level1_Library");
    }

    public void OpenSecondLevel()
    {
        levelSelectionScreen.SetActive(false);
        SceneManager.LoadScene("Level 2");
    }

    public void ShowDefeatScreen()
    {
        
    }

    public void ShowVictoryScreen()
    {
        
    }

    public void ShowMenuScreen()
    {
        pauseMenuScreenBack.SetActive(true);
        pauseScreen.SetActive(true);
        pause = true;
    }

    public void HideMenuScreen()
    {
        pauseMenuScreenBack.SetActive(false);
        pauseScreen.SetActive(false);
        pause = false;
    }

    public void GoBack()
    {
        TransitionScreenEffect(currentScreen, previousScreen);
        UpdateGoBack();
    }

    public void UpdateGoBack()
    {
        if(currentScreen != mainMenuScreen)
        {
            currentScreen = previousScreen;
            if (currentScreen == levelSelectionScreen)
            {
                previousScreen = chapterSelectionScreen;
            }
            if (currentScreen == chapterSelectionScreen)
            {
                previousScreen = mainMenuScreen;
            }
        }
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
