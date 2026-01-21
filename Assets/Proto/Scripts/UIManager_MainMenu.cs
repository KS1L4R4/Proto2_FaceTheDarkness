using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    //Elementos de la escena
    public GameObject mainMenuUI;

    public void OpenTestLevel()
    {
        SceneManager.LoadScene("TestLevel");
    }
}
