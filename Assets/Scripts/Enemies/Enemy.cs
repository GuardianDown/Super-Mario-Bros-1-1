using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Enemy : MonoBehaviour
{

#region Serializable fields	
	
	[SerializeField] protected float speed = 0;
	[SerializeField] protected LayerMask groundOrEnemyLayer = 0;
	[SerializeField] private Vector2 moveTriggerColliderPosition = new Vector2(0, 0);
	[SerializeField] protected int points = 0;
	[SerializeField] protected AudioSource stompSound = null;
	
#endregion

#region Components

	protected Animator anim;
	protected BoxCollider2D bc2d;
	protected Rigidbody2D rb;
	private EdgeCollider2D ec2d;
	protected MeshRenderer pointsText;
	
#endregion

#region Private fields

	protected bool isMoving = false;
	private bool leftCollision = true;
	
#endregion

#region Properties
	
	public bool IsDead
	{
		get
		{
			return anim.GetBool("isDead");
		}
		
		protected set
		{
			anim.SetBool("isDead", value);
		}
	}
	
	public int Points
	{
		get
		{
			return points;
		}
	}
	
#endregion

#region Unity functions
	
	private void Awake()
	{
		anim = GetComponent<Animator>();
		bc2d = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		ec2d = GetComponentInChildren<EdgeCollider2D>();
		ec2d.transform.localPosition = moveTriggerColliderPosition;
	}
	
	private void Update()
	{
		CheckCollision();
		Destroy();
	}
	
	protected virtual void FixedUpdate()
	{
		Move();
	}
	
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			isMoving = true;
		}
	}
	
#endregion

#region Other functions
	
	public void Stop()
	{
		isMoving = false;
		anim.enabled = false;
	}
	
	protected virtual void CheckCollision()
	{
		if(isMoving)
		{
			if(!leftCollision)
			{
				RaycastHit2D raycastHitRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x + 0.1f, bc2d.bounds.center.y), Vector2.right, 0.1f, groundOrEnemyLayer);
				if(raycastHitRight.collider != null)
				{
					speed = -speed;
					leftCollision = !leftCollision;
				}
			}
			else
			{
				RaycastHit2D raycastHitLeft = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x - 0.1f, bc2d.bounds.center.y), Vector2.left, 0.1f, groundOrEnemyLayer);
				if(raycastHitLeft.collider != null)
				{
					speed = -speed;
					leftCollision = !leftCollision;
				}
			}
		}
	}
	
	private void Destroy()
	{
		if(isMoving)
		{
			if(Mathf.Abs(transform.position.x - GameManager.instance.Player.transform.position.x) > 32)
				Destroy(gameObject);
		}
	}
	
	public void DieByStar()
	{
		bc2d.enabled = false;
		IsDead = true;
		transform.rotation = new Quaternion(0, 0, 180, 0);
		rb.gravityScale = 17;
		rb.AddForce(new Vector2(0.0f, 40), ForceMode2D.Impulse);
	}
	
#endregion

#region Abstract functions

	abstract public void Die();
	
	abstract protected void Move();
	
#endregion

}
