using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverGameManager : MonoBehaviour
{ 
    private void Start()
	{
		StartCoroutine("GameOverCoroutine");
		LifeCounter.Reset();
		CoinCounter.Reset();
		ScoreCounter.Reset();
	}
	
	private IEnumerator GameOverCoroutine()
	{
		yield return new WaitForSeconds(5.0f);
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}
}
