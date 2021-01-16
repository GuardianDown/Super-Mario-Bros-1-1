using UnityEngine;
using UnityEngine.Events;

public class Mushroom1up : BaseMushroom
{
	public UnityEvent PlayerGetMushroom1upEvent;
	
	private void Awake()
	{
		PlayerGetMushroom1upEvent = new UnityEvent();
		GameManager.instance.Mushroom1up = this;
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(isMoving && collider.gameObject.tag == "Player")
		{
			PlayerGetMushroom1upEvent.Invoke();
			Destroy(gameObject);
		}
	}
}
