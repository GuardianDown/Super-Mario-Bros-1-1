using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : AbstractButton
{
	public override void OnSubmit(BaseEventData eventData)
	{
		Application.Quit();
	}
}
