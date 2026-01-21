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
    public TextMeshProUGUI bulletsAmmountTxt;
    public TextMeshProUGUI healthAmmountTxt;
    public TextMeshProUGUI oilAmmount;

    void Start()
    {
        hudUi.SetActive(true);
        defeatViewUi.SetActive(false);
    }

    void Update()
    {
        //Mover este codigo del Update a funciones individuales
        bulletsAmmountTxt.text = "Bullets: " + playerController.bulletsLeft.ToString();
        healthAmmountTxt.text = "Health: " + healthManager.playerHealth.ToString();
        oilAmmount.text = "Battery: " + playerController.oil.ToString();
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
