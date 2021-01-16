using UnityEngine;

public class Camera : MonoBehaviour
{
	[SerializeField] private Player player = null;
	
	private void Update()
	{
		Move();
	}
	
	private void Move()
	{
		if(player.transform.position.x >= transform.position.x - 5)
			transform.position = new Vector3(player.transform.position.x + 5.0f, transform.position.y, -10.0f);
	}
}
