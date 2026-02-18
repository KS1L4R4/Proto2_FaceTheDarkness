using UnityEngine;
using DG.Tweening;

public class Tablas : MonoBehaviour
{
	public float rotation = 0f;
	public Transform hinge;
	int once = 0;

	//Usar Colision Enter si la intención es solamente activar esta lógica una sola vez.
	//Quitar el rigidbody, causa conflictos de colisión.
	public void OnCollisionEnter(Collision collision)
	{
		if (once == 0)
		{
			if (collision.gameObject.CompareTag("Player"))
			{
				once++;
				//transform.RotateAround(hinge.position, hinge.forward, rotation);

				//Rotar localmente el hinge en lugar de la tabla, añadir un suavizado bounce para simular física.
				hinge.DOLocalRotate(new Vector3(0, 0, 90), 0.5f).SetEase(Ease.OutBounce);
			}
		}

	}
}
