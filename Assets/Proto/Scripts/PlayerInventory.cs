using System.Collections.Generic; //Librería que contiene la clase List
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Key> keyList;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Key key = other.GetComponent<Key>();
		if (key != null)
		{
            keyList.Add(key);
            other.gameObject.SetActive(false);
		}
	}

    public void RemoveKey(Key keyToRemove)
    {
        if (keyList.Contains(keyToRemove))
        {
            keyList.Remove(keyToRemove);
        }
    }
}
