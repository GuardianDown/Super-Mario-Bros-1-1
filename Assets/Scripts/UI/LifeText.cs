using UnityEngine;
using UnityEngine.UI;

public class LifeText : MonoBehaviour
{
	private Text text;
	
	private void Awake()
	{
		text = GetComponent<Text>();
		text.text = "x " + LifeCounter.Lifes;
	}
}
