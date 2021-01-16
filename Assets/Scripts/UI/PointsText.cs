using System.Collections;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    private void FixedUpdate()
	{
		transform.position += new Vector3(0, 0.1f, 0);
		StartCoroutine("Destroy");
	}
	
	private IEnumerator Destroy()
	{
		yield return new WaitForSeconds(1.0f);
		Destroy(gameObject);
	}
}
