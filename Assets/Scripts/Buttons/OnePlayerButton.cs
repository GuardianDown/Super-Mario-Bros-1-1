using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnePlayerButton : AbstractButton
{
	public override void OnSubmit(BaseEventData eventData)
	{
		SceneManager.LoadScene("Load", LoadSceneMode.Single);
	}
}
