using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class Koopa : Enemy
{
	[SerializeField] private LayerMask playerLayer = 0;
	[SerializeField] private AudioSource kickSound = null;
	
	private bool IsBumped
	{
		get
		{
			return anim.GetBool("isBumped");
		}
		
		set
		{
			anim.SetBool("isBumped", value);
		}
	}
	
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		Skate();
	}
	
	protected override void Move()
	{
		if(isMoving && !IsBumped && !IsDead)
		{
			rb.MovePosition(rb.position + new Vector2(speed, 0));
		}
	}
	
	public override void Die()
	{
		if(!IsBumped)
		{
			IsBumped = true;
			bc2d.size = new Vector2(2, 1.75f);
			gameObject.transform.position = gameObject.transform.position - new Vector3(0, 0.625f, 0);
			stompSound.Play();
			StartCoroutine("BumpCoroutine");
		}
		else
		{
			if(transform.position.x > GameManager.instance.Player.transform.position.x)
					speed = 1;
			else
				speed = -1;
			IsDead = true;
			kickSound.Play();
		}
	}
	
	protected override void CheckCollision()
	{
		if(!IsDead)
			base.CheckCollision();
		else
		{
			RaycastHit2D raycastHitRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x + 0.1f, bc2d.bounds.center.y), Vector2.right, 0.1f, groundOrEnemyLayer);
			RaycastHit2D raycastHitLeft = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x - 0.1f, bc2d.bounds.center.y), Vector2.left, 0.1f, groundOrEnemyLayer);
			if(raycastHitLeft.collider != null)
			{
				if(raycastHitLeft.collider.gameObject.tag == "Ground")
					speed = - speed;
				else if(raycastHitLeft.collider.gameObject.tag == "Enemy")
					raycastHitLeft.collider.gameObject.GetComponent<Goomba>().DieByKoopa();
			}
			else if(raycastHitRight.collider != null)
			{
				if(raycastHitRight.collider.gameObject.tag == "Ground")
					speed = - speed;
				else if(raycastHitRight.collider.gameObject.tag == "Enemy")
					raycastHitRight.collider.gameObject.GetComponent<Goomba>().DieByKoopa();
			}
		}
	}
	
	private IEnumerator BumpCoroutine()
	{
		yield return new WaitForSeconds(6.0f);
		IsBumped = false;
		bc2d.size = new Vector2(2, 3);
	}
	
	private void Skate()
	{
		if(IsDead && IsBumped)
		{
			rb.MovePosition(rb.position + new Vector2(speed / 2, 0));
		}
	}
}
