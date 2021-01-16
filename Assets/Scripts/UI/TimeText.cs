using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    [SerializeField] private int timeCounter = 400;
	
	private float deltaTimeCounter = 0;
	
	private Text text;
	
	private void Awake()
	{
		text = GetComponent<Text>();
	}
	
	private void Update()
	{
		UpdateTime();
	}
	
	private void UpdateTime()
	{
		deltaTimeCounter += Time.deltaTime * 2;
		if(deltaTimeCounter >= 1)
		{
			timeCounter--;
			text.text = "TIME\n" + timeCounter.ToString();
			deltaTimeCounter = 0;
		}
	}
}
