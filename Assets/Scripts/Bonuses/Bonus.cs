using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
	[SerializeField] protected int points = 100;
	
	private void OnDestroy()
	{
		GameManager.instance.PointsText.GetComponent<TextMesh>().text = points.ToString();
		Instantiate(GameManager.instance.PointsText, transform.position + new Vector3(0, 1, 0), transform.rotation);
	}
}
