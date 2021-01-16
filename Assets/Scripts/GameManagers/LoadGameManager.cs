using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("LoadLevelScene");
    }
	
	private IEnumerator LoadLevelScene()
	{
		yield return new WaitForSeconds(3.0f);
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
}
