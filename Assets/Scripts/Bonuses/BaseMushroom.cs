using UnityEngine;

public abstract class BaseMushroom : Bonus
{
	
#region Serializable fields

	[SerializeField] private float speed = 0;
	[SerializeField] private LayerMask groundLayer = 0;
	
#endregion

#region Private fields
	
	protected bool isMoving = false;
	private bool collisionChecked = false;
	
#endregion

#region Components
	
	private Animator anim;
	private Rigidbody2D rb;
	private BoxCollider2D bc2d;
	
#endregion

#region Unity functions
	
	private void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		bc2d = GetComponent<BoxCollider2D>();
	}
	
	private void FixedUpdate()
	{
		Move();
		CheckCollision();
	}
	
#endregion

#region Other functions	
	
	private void Move()
	{
		if(isMoving)
			rb.velocity = new Vector2(speed, 0);
	}
	
	private void ChangeState()
	{
		anim.enabled = false;
		isMoving = true;
	}
	
	private void CheckCollision()
	{
		if(isMoving && !collisionChecked)
		{
			RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.right, 0.1f, groundLayer);
			if(raycastHit.collider != null)
			{
				speed = -speed;
				collisionChecked = true;
			}
		}
	}
	
#endregion

}
