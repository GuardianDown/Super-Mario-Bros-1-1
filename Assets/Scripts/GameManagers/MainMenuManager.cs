using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
	private GameObject lastSelect;
	
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Screen.SetResolution(256, 224, true);
	}
	
	private void Update()
	{
		ReturnSelectOnMenu();
	}
	
	private void ReturnSelectOnMenu()
	{
		if(EventSystem.current.currentSelectedGameObject == null)
			EventSystem.current.SetSelectedGameObject(lastSelect);
		else
			lastSelect = EventSystem.current.currentSelectedGameObject;
	}
}
