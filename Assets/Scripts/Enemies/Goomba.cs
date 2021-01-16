using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Goomba : Enemy
{		
	protected override void Move()
	{
		if(isMoving && !IsDead)
		{
			rb.MovePosition(rb.position + new Vector2(speed, 0));
		}
	}

	public override void Die()
	{
		IsDead = true;
		bc2d.size = new Vector2(2, 1);
		gameObject.transform.position = gameObject.transform.position - new Vector3(0, 0.5f, 0);
		stompSound.Play();
		StartCoroutine("DieCoroutine");
	}
	
	private IEnumerator DieCoroutine()
	{
		yield return new WaitForSeconds(0.2f);
		Destroy(gameObject);
	}
	
	public void DieByKoopa()
	{
		bc2d.enabled = false;
		IsDead = true;
		transform.rotation = new Quaternion(0, 0, 180, 0);
		rb.gravityScale = 17;
		rb.AddForce(new Vector2(0.0f, 40), ForceMode2D.Impulse);
	}
}
