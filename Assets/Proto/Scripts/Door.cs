using UnityEngine;

public class Door : MonoBehaviour
{
	public KeyId requiredKey;

	public void OpenDoor()
	{
		//Encalpsular lógica de abrir la puerta en su propia función para posteriormente poder implementar ún comportamiento mas robusto (abrir con tween, rotación, etc.)
		gameObject.SetActive(false);
	}
}
