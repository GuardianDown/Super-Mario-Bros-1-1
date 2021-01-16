using UnityEngine;

public static class ScoreCounter
{
	private static int score = 0;
	
	public static int Score
	{
		get
		{
			return score;
		}
	}
	
	public static void AddScore(int points)
	{
		score += points;
	}
	
	public static void Reset()
	{
		score = 0;
	}
}
