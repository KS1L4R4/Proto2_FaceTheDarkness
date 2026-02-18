using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController;
    public HealthManager healthManager;
    public GameObject hudUi;
    public GameObject defeatViewUi;
    public GameObject mainMenuUi;
    public TextMeshProUGUI oilAmmount;

    void Start()
    {
        hudUi.SetActive(true);
        defeatViewUi.SetActive(false);
    }

    public void ShowDefeatView()
    {
        hudUi.SetActive(false);
        defeatViewUi.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
