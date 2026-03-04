using UnityEngine;

public class HealthManager : MonoBehaviour
{
    //Elementos de la escena
    public UIManager uimanager;
    public PlayerController playerController;

    //Booleanos
    public bool isAlive;

    //Sanidad
    public float sanity;
    public float maxSanity = 10f;

    void Start()
    {
        uimanager = FindAnyObjectByType<UIManager>();
        playerController = FindAnyObjectByType<PlayerController>();

        isAlive = true;
        sanity = maxSanity;
    }
    public void KillPlayer()
    {
        /*
        isAlive = false;
        playerController.rb.linearVelocity = Vector3.zero;
        uIManager.ShowDefeatView();
        */
    }

    public void SanidadRes()
    {
        sanity -= Time.deltaTime;
        uimanager.UpdateSanityBar();
        if (sanity < 0)
        {
            sanity = 0;
            KillPlayer();
        }
        
    }
}
