using UnityEngine;

public static class LifeCounter
{
	private static int lifes = 3;
	
	public static int Lifes
	{
		get
		{
			return lifes;
		}
	}
	
	public static void AddLife()
	{
		lifes++;
	}
	
	public static void SubtractLife()
	{
		lifes--;
	}
	
	public static void Reset()
	{
		lifes = 3;
	}
}
