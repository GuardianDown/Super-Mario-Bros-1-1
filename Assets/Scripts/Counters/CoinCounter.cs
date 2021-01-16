using UnityEngine;

public static class CoinCounter
{
	private static int coins = 0;
	
	public static int Coins
	{
		get
		{
			return coins;
		}
	}
	
	public static void AddCoin()
	{
		coins++;
	}
	
	public static void Reset()
	{
		coins = 0;
	}
}
