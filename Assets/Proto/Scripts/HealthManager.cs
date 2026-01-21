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

    //Sanidad
    public float sanidad = 5f;

    void Start()
    {
        maxHealth = 5;
        playerHealth = maxHealth;
        isAlive = true;
        sanidad = 5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HarmPlayer();
        }

        if (Input.GetKey(KeyCode.T))
        {
            sanidad -= Time.deltaTime;
            if (sanidad < 0)
            {
                sanidad = 0;
                KillPlayer();
            }
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
