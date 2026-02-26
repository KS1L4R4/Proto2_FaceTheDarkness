using UnityEngine;
using UnityEngine.Events; //Importar eventos
using System.Collections;
using System.Collections.Generic;

public class InterObjects : MonoBehaviour
{
    // The base class for all interactuable objects
    protected PlayerController playerCtrl;
    protected Collider interactableCollider;
    public UnityEvent onInteractEvent; //crear el evento

    [SerializeField] protected GameObject interactableCanvas;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactableCanvas.SetActive(true);
            playerCtrl = other.GetComponent<PlayerController>();
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                print("jghfgljfrjk");
                Interact(playerCtrl);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            interactableCanvas.SetActive(false);
        }
    }

    protected virtual void Interact(PlayerController player)
    {
        Debug.Log("You interacted");
        onInteractEvent.Invoke();  //Invocar el evento
        gameObject.SetActive(false);
    }
}
