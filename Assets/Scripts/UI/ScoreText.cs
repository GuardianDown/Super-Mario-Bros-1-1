using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
	private Text text;
	
	private void Awake()
	{
		text = GetComponent<Text>();
		text.text = ScoreCounter.Score.ToString("D6");
		GameManager.instance.ScoreText = this;
	}
	
	public void UpdateText(int points)
	{
		ScoreCounter.AddScore(points);
		text.text = ScoreCounter.Score.ToString("D6");
	}
}
