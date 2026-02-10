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
    public List<InteractuableDesignator> pickablesList;
    public int oilCounter = 0;
    public int lavenderCounter = 0;

    int maxCapacity = 3;

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
                        Debug.Log("You can't pick any more oil");
                        return;
                    }
                oilCounter++;
                Debug.Log("Your current oil count is: " + oilCounter);
            break;

            case InteractuableID.Lavender:
                if(lavenderCounter >= maxCapacity)
                    {
                        Debug.Log("You can't pick any more lavender");
                        return;
                    }
                lavenderCounter++;
                Debug.Log("Your current lavender count is: " + lavenderCounter);
            break;

            default:
            //
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
