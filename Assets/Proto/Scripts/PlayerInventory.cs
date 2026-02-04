using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using System.Xml.Linq;

public class PlayerInventory : MonoBehaviour
{
    public List<InteractuableDesignator> keyList;

    private void OnTriggerEnter(Collider other)
    {
        InteractuableDesignator key = other.GetComponent<InteractuableDesignator>();
        if (key != null)
        {
            keyList.Add(key);
            other.gameObject.SetActive(false);
        }
    }
    public void RemoveKey(InteractuableDesignator keyToRemove)
    {
        if (keyList.Contains(keyToRemove))
        {
            keyList.Remove(keyToRemove);
        }
    }
}
