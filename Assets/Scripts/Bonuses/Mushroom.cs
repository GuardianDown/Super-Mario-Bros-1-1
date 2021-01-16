using UnityEngine;
using UnityEngine.Events;

public class Mushroom : BaseMushroom
{
	public UnityEvent PlayerGetMushroomEvent;
	
	private void Awake()
	{
		PlayerGetMushroomEvent = new UnityEvent();
		GameManager.instance.Mushroom = this;
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(isMoving && collider.gameObject.tag == "Player")
		{
			PlayerGetMushroomEvent.Invoke();
			Destroy(gameObject);
		}
	}
}
