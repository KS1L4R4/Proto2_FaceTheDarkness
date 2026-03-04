using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using System.Xml.Linq;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{
    //public List<InteractuableDesignator> keyList;

    //public List<InteractuableDesignator> oilList;
    private UIManager uimanager;
    public List<InteractuableDesignator> pickablesList;
    public int oilCounter = 0;
    public int lavenderCounter = 0;

    int maxCapacity = 3;

    private void Start()
    {
        uimanager = FindAnyObjectByType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractuableDesignator pickable = other.GetComponent<InteractuableDesignator>();

        if(pickable == null)
        {
            return;
        }

        switch (pickable.Designation)
        {
            case InteractuableID.OilCan:
                if(oilCounter >= maxCapacity)
                    {
                        return;
                    }
                oilCounter++;
                uimanager.UpdateOilIconUI();
            break;

            case InteractuableID.Lavender:
                if(lavenderCounter >= maxCapacity)
                    {
                        return;
                    }
                lavenderCounter++;
                uimanager.UpdateLavenderIconUI();
            break;

            default:

            break;

        }


        pickablesList.Add(pickable);
        other.gameObject.SetActive(false);

    }
    public void RemovePickable(InteractuableDesignator itemToRemove)
    {
        if (pickablesList.Contains(itemToRemove))
        {
            pickablesList.Remove(itemToRemove);
        }
    }
}
