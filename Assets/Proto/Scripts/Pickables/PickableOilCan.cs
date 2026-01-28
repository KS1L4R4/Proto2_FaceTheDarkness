using UnityEngine;

public class PickableOilCan : Pickable
{
	public override void PickupEffect()
	{
		//base.PickupEffect(playerController);
		PlayerController player = GameObject.FindAnyObjectByType<PlayerController>();
		if (player != null) 
		{
			player.oil = player.maxOil;
		}
	}
}
