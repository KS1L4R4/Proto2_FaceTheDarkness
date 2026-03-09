using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<Image> oilIcons;
    [SerializeField] private List<Image> lavenderIcons;
    [SerializeField] private Image sanityBar;

    private PlayerInventory inventory;
    private HealthManager healthManager;

    private GameObject mainMenuScreen;
    private GameObject chapterSelectionScreen;
    private GameObject levelSelectionScreen;
    private GameObject pauseMenuScreenBack;
    private GameObject pauseScreen;
    private GameObject defeatScreen;
    private GameObject winScreen;
    private GameObject victoryScreen;
    private GameObject exitWarningScreen;
    private GameObject characterSelectionScreen;
    private GameObject storeScreen;
    private GameObject previousScreen;
    private GameObject currentScreen;
    private GameObject settingsCreen;
    private GameObject HUD;
    private GameObject tutorialScreen;
    private GameObject sanityDeathScreen;
    private GameObject popupWindow;
    private GameObject lightDropPurchaseWindow;
    private GameObject lightDropPurchaseScreen;
    private GameObject errorMessage;
    private GameObject characterOptions;
    private GameObject storyScreen;
    private GameObject cosmeticsScreen;
    private GameObject leftButton;
    private GameObject rightButton;
    private GameObject warningLeft;
    private GameObject warningRight;

    public Image purchaseImage;
    public Image activePortrait;

    public Image leftport;
    public Image rightport;

    public TMP_Text purchaseText;

    public string[] itemDescriptions;
    public Sprite[] itemImage;

    public Image transitionImage;

    private bool portraitChanged;
    public bool pause;
    public bool playerCaught;

    private bool isWindowed;

    void Start()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();
        healthManager = FindAnyObjectByType<HealthManager>();

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
        winScreen = GameObject.Find("UIManager/UI_WinScreen");
        HUD = GameObject.Find("UIManager/UI_HUD");
        tutorialScreen = GameObject.Find("UIManager/UI_TutorialScreen");
        sanityDeathScreen = GameObject.Find("UIManager/UI_SanityDeathScreen");

        popupWindow = GameObject.Find("UIManager/UI_StoreScreen/PurchaseWindow");
        lightDropPurchaseWindow = GameObject.Find("UIManager/UI_StoreScreen/LightDropPurchaseWindow");
        lightDropPurchaseScreen = GameObject.Find("UIManager/UI_LightDropPurchaseScreen");
        errorMessage = GameObject.Find("UIManager/UI_LightDropPurchaseScreen/ErrorMessage");
        characterOptions = GameObject.Find("UIManager/UI_CharacterSelectionScreen/Options");
        storyScreen = GameObject.Find("UIManager/UI_StoryScreen");
        cosmeticsScreen = GameObject.Find("UIManager/UI_Cosmetics");
        leftButton = GameObject.Find("UIManager/UI_Cosmetics/LeftButton");
        rightButton = GameObject.Find("UIManager/UI_Cosmetics/RightButton");
        warningLeft = GameObject.Find("UIManager/UI_Cosmetics/WarningLeft");
        warningRight = GameObject.Find("UIManager/UI_Cosmetics/WarningRight");


        HUD.SetActive(false);
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            mainMenuScreen.SetActive(false);
            HUD.SetActive(true);
        }
        exitWarningScreen.SetActive(false);
        chapterSelectionScreen.SetActive(false);
        levelSelectionScreen.SetActive(false);
        characterSelectionScreen.SetActive(false);
        storeScreen.SetActive(false);
        pauseMenuScreenBack.SetActive(false);
        pauseScreen.SetActive(false);
        defeatScreen.SetActive(false);
        settingsCreen.SetActive(false);
        winScreen.SetActive(false);
        tutorialScreen.SetActive(false);
        sanityDeathScreen.SetActive(false);
        popupWindow.SetActive(false);
        lightDropPurchaseWindow.SetActive(false);
        lightDropPurchaseScreen.SetActive(false);
        errorMessage.SetActive(false);
        characterOptions.SetActive(false);
        storyScreen.SetActive(false);
        cosmeticsScreen.SetActive(false);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        warningLeft.SetActive(false);
        warningRight.SetActive(false);

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

        UpdateOilIconUI();
        UpdateLavenderIconUI();
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

    //Screen transition and screen flow
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
        if (chapterSelectionScreen != null && levelSelectionScreen != null)
        {
            currentScreen = levelSelectionScreen;
            previousScreen = chapterSelectionScreen;
            TransitionScreenEffect(chapterSelectionScreen, levelSelectionScreen);
        }
    }
    public void ShowTutorialsScreen()
    {
        if (mainMenuScreen != null && tutorialScreen != null)
        {
            currentScreen = tutorialScreen;
            previousScreen = mainMenuScreen;
            TransitionScreenEffect(mainMenuScreen, tutorialScreen);
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
    public void ShowChapterSelectionScreem()
    {
        currentScreen = chapterSelectionScreen;
        previousScreen = mainMenuScreen;
        TransitionScreenEffect(mainMenuScreen, chapterSelectionScreen);
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
        winScreen.SetActive(true);
        Cursor.visible = true;
        pause = true;
    }
    public void ShowMenuScreen()
    {
        pauseMenuScreenBack.SetActive(true);
        pauseScreen.SetActive(true);
        pause = true;
    }
    public void ShowSanityDeathScreen()
    {
        sanityDeathScreen.SetActive(true);
        pause = true;
        Cursor.visible = true;
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
        if (currentScreen != mainMenuScreen)
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
            if (currentScreen == storeScreen)
            {
                previousScreen = mainMenuScreen;
                popupWindow.SetActive(false);
                lightDropPurchaseWindow.SetActive(false);
                lightDropPurchaseScreen.SetActive(false);
            }
            if (currentScreen == characterSelectionScreen)
            {
                previousScreen = mainMenuScreen;
                characterOptions.SetActive(false);
            }
        }
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowPurchaseWindow(int itemID)
    {
        popupWindow.SetActive(true);
        switch (itemID)
        {
            case 0:
                purchaseImage.sprite = itemImage[itemID];
                purchaseText.text = itemDescriptions[itemID];
                break;

            case 1:
                purchaseImage.sprite = itemImage[itemID];
                purchaseText.text = itemDescriptions[itemID];
                break;

            case 2:
                purchaseImage.sprite = itemImage[itemID];
                purchaseText.text = itemDescriptions[itemID];
                break;
        }
    }
    public void CancelPurchase()
    {
        popupWindow.SetActive(false);
    }
    public void ShowLightDropPurchaseWindow()
    {
        lightDropPurchaseWindow.SetActive(true);
    }
    public void HideLightDropPurchaseWindow()
    {
        lightDropPurchaseWindow.SetActive(false);
    }
    public void ShowLightDropPurchaseScreen()
    {
        currentScreen = lightDropPurchaseScreen;
        previousScreen = storeScreen;
        TransitionScreenEffect(storeScreen, lightDropPurchaseScreen);
    }
    public void ShowErrorMessage()
    {
        errorMessage.SetActive(true);
    }
    public void HideErrorMessage()
    {
        errorMessage.SetActive(false);
    }
    public void ShowCharacterOptions()
    {
        characterOptions.SetActive(false);
        characterOptions.SetActive(true);
    }
    public void ShowStory()
    {
        currentScreen = storyScreen;
        previousScreen = characterSelectionScreen;
        TransitionScreenEffect(characterSelectionScreen, storyScreen);
    }
    public void ShowCosmetics()
    {
        currentScreen = cosmeticsScreen;
        previousScreen = characterSelectionScreen;
        TransitionScreenEffect(characterSelectionScreen, cosmeticsScreen);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
    }

    public void ShowChangeButton(int id)
    {
        if(id == 0)
        {
            leftButton.SetActive(true);
            rightButton.SetActive(false);
            rightport.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            leftport.color = new Color(1f, 1f, 1f, 1f);
        }
        if(id == 1)
        {
            rightButton.SetActive(true);
            leftButton.SetActive(false);
            leftport.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            rightport.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void ChangePortrait(int value)
    {
        activePortrait.sprite = itemImage[value];
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        warningLeft.SetActive(false);
        warningRight.SetActive(false);
    }
    public void ShowLeft()
    {
        warningLeft.SetActive(true);
    }
    public void ShowRigth()
    {
        warningRight.SetActive(true);
    }
    public void cancelChange()
    {
        warningLeft.SetActive(false);
        warningRight.SetActive(false);
    }

    //HUD Elements
    public void UpdateOilIconUI()
    {
        for (int i = 0; i < oilIcons.Count; i++)
        {
            if (inventory != null)
            {
                oilIcons[i].enabled = i < inventory.oilCounter;
            }
        }
    }
    public void UpdateLavenderIconUI()
    {
        for (int i = 0; i < lavenderIcons.Count; i++)
        {
            if (inventory != null)
            {
                lavenderIcons[i].enabled = i < inventory.lavenderCounter;
            }   
        }
    }
    public void UpdateSanityBar()
    {
        float normalized = (float) healthManager.sanity / healthManager.maxSanity; //Hace falta definir una cantidad maxima de sanidad
        sanityBar.fillAmount = normalized;
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
