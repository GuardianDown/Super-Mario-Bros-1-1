using UnityEngine;
using UnityEngine.Events;

public class Flower : Bonus
{
	public UnityEvent PlayerGetFlowerEvent;
	
	private void Awake()
	{
		PlayerGetFlowerEvent = new UnityEvent();
		GameManager.instance.Flower = this;
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			PlayerGetFlowerEvent.Invoke();
			Destroy(gameObject);
		}
	}
}
