using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;

    public void OpenTestLevel()
    {
        SceneManager.LoadScene("TestLevel");
    }
}
