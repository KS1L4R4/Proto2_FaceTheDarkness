using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
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
    private GameObject settingsCreen;

    public Image transitionImage;

    public bool pause;
    public bool playerCaught;

    private bool isWindowed;

    void Start()
    {
        mainMenuScreen = GameObject.Find("UIManager/UI_MainMenuScreen");
        chapterSelectionScreen = GameObject.Find("UIManager/UI_ChapterSelectionScreen");
        levelSelectionScreen = GameObject.Find("UIManager/UI_LevelSelectionScreen");
        exitWarningScreen = GameObject.Find("UIManager/UI_ExitWarningScreen");
        characterSelectionScreen = GameObject.Find("UIManager/UI_CharacterSelectionScreen");
        storeScreen = GameObject.Find("UIManager/UI_StoreScreen");
        pauseMenuScreenBack = GameObject.Find("UIManager/UI_PauseMenuScreenBGND");
        pauseScreen = GameObject.Find("UIManager/UI_PauseMenuScreen");
        defeatScreen = GameObject.Find("UIManager/UI_DefeatScreen");
        settingsCreen = GameObject.Find("UIManager/UI_SettingsScreen");

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            mainMenuScreen.SetActive(false);
        }
        exitWarningScreen.SetActive(false);
        if (chapterSelectionScreen != null)
        {
            chapterSelectionScreen.SetActive(false);
        }
        levelSelectionScreen.SetActive(false);
        characterSelectionScreen.SetActive(false);
        storeScreen.SetActive(false);
        pauseMenuScreenBack.SetActive(false);
        pauseScreen.SetActive(false);
        defeatScreen.SetActive(false);
        settingsCreen.SetActive(false);

        pause = false;
        playerCaught = false;

        if (transitionImage != null)
        {
            if (transitionImage.GetComponent<CanvasGroup>() != null)
            {
                transitionImage.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }

        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            isWindowed = true;
        }
        else
        {
            isWindowed = false;
        }

            SetFramerateTo60();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pause == false)
                {
                    ShowMenuScreen();
                    Cursor.visible = true;
                }
                else
                {
                    HideMenuScreen();
                    Cursor.visible = false;
                }
            }
        }
    }

    public void TransitionScreenEffect(GameObject screenToHide, GameObject screenToShow)
    {
        if (transitionImage.GetComponent<CanvasGroup>() != null)
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

    public void ShowSettingsScreen()
    {
        currentScreen = settingsCreen;
        previousScreen = mainMenuScreen;
        TransitionScreenEffect(mainMenuScreen, settingsCreen);
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
        defeatScreen.SetActive(true);
        Cursor.visible = true;
        playerCaught = true;
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

    //Framerates
    public void SetFramerateTo30()
    {
        Application.targetFrameRate = 30;
    }
    public void SetFramerateTo60()
    {
        Application.targetFrameRate = 60;
    }
    public void SetFramerateTo90()
    {
        Application.targetFrameRate = 90;
    }
    public void SetFramerateTo120()
    {
        Application.targetFrameRate = 120;
    }
    public void SetFramerateTo144()
    {
        Application.targetFrameRate = 144;
    }
    public void SetFramerateTo165()
    {
        Application.targetFrameRate = 165;
    }

    //Display modes
    public void SetWindowedDisplay()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        isWindowed = false;
    }
    public void SetFullscreenDisplay()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        isWindowed = true;
    }

    //Resolutions
    public void SetResolution720()
    {
        Screen.SetResolution(1280, 720, isWindowed);
    }
    public void SetResolution1080()
    {
        Screen.SetResolution(1920, 1080, isWindowed);
    }
    public void SetResolution1440()
    {
        Screen.SetResolution(2560, 1440, isWindowed);
    }
}
