using UnityEngine;
using UnityEngine.Events;

public class Coin : Bonus
{
	public MyIntEvent CoinSpawnEvent;
	
	private void Awake()
	{
		CoinSpawnEvent = new MyIntEvent();
		GameManager.instance.Coin = this;
		UpdateScore();
	}
	
	private void Destroy()
	{
		Destroy(gameObject);
	}
	
	private void UpdateScore()
	{
		CoinSpawnEvent.Invoke(points);
	}
}
