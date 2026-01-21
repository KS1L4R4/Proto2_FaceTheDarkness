using UnityEngine;

public class HealthManager : MonoBehaviour
{
    //Elementos de la escena
    public UIManager uIManager;
    public PlayerController playerController;

    //Enteros
    public int playerHealth;
    public int maxHealth;

    //Booleanos
    public bool isAlive;

    void Start()
    {
        maxHealth = 5;
        playerHealth = maxHealth;
        isAlive = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HarmPlayer();
        }
    }

    public void HarmPlayer()
    {
        playerHealth--;
        if(playerHealth <= 0)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        isAlive = false;
        playerController.rb.linearVelocity = Vector3.zero;
        uIManager.ShowDefeatView();
    }
}
