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
    public TextMeshProUGUI sanity;

    public static UIManager instance;

	private void Awake()
	{
        instance = this;    
	}

	void Start()
    {
        hudUi.SetActive(true);
        defeatViewUi.SetActive(false);
    }

    void Update()
    {
        //Mover este codigo del Update a funciones individuales
        //bulletsAmmountTxt.text = "Bullets: " + playerController.bulletsLeft.ToString();
        //healthAmmountTxt.text = "Health: " + healthManager.playerHealth.ToString();
        //oilAmmount.text = "Battery: " + playerController.oil.ToString();
        //sanity.text = "Sanidad: " + healthManager.sanidad.ToString();
    }

    //Tener las funciones de "reflejo" de UI en funciones separadas, llamarlas cuando se cumpla la condición en la que se actualice su stat
    //Por ejemplo solo actualizar la salud cuando el jugador reciba o tome daño
    public void UpdateBullets(int amount)
    {
        bulletsAmmountTxt.text = $"Bullets: {amount}";
	}

    public void UpdateHealth(int amount)
    {
		healthAmmountTxt.text = $"Health: {amount}";
	}

	public void UpdateOil(float amount)
	{
		oilAmmount.text = $"Battery: {amount}";
	}

	public void UpdateSanity(float amount)
    {
        if (sanity != null)
        {
            sanity.text = $"Sanity: {amount}";
        }
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
