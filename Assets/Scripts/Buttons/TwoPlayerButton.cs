using UnityEngine;
using UnityEngine.EventSystems;

public class TwoPlayerButton : AbstractButton
{
	public override void OnSubmit(BaseEventData eventData)
	{
		Debug.Log("2");
	}
}
