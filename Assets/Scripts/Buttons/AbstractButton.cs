using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class AbstractButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
	private Image image;
	
	private void Awake()
	{
		image = FindImageChild();
	}
	
 	public void OnSelect(BaseEventData eventData)
	{
		image.enabled = true;
	}
	
	public void OnDeselect(BaseEventData eventData)
	{
		image.enabled = false;
	}
	
	abstract public void OnSubmit(BaseEventData eventData);
	
	private Image FindImageChild()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			child.TryGetComponent(out Image component);
			if(component != null)
				return component;
		}
		return null;
	}
}
