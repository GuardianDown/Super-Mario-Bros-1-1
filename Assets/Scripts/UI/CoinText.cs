using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{
	private Text text;
	
	private void Awake()
	{
		text = GetComponent<Text>();
		text.text = "x" + CoinCounter.Coins.ToString("D2");
		GameManager.instance.CoinText = this;
	}
	
	public void UpdateText(int points)
	{
		CoinCounter.AddCoin();
		text.text = "x" + CoinCounter.Coins.ToString("D2");
	}
}
