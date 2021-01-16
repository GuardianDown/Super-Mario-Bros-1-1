using UnityEngine;
using UnityEngine.Events;

public class Star : Bonus
{
	
#region Serializable fields
	
	[SerializeField] private float speed = 0;
	[SerializeField] private float height = 5.0f;
	[SerializeField] private float width = 3;
	[SerializeField] private float x = 0.9f;
	[SerializeField] private float y = -2.5f;
	[SerializeField] private float newHeight = 4;
	[SerializeField] private float newWidth = 2;
	[SerializeField] private float newX = -1.5f;
	[SerializeField] private float newY = -5;
	[SerializeField] private LayerMask groundLayer = 0;
	
#endregion

#region Components	
	
	private Rigidbody2D rb;
	private BoxCollider2D bc2d;
	private Animator anim;
	
#endregion

#region Private fields
	
	private bool isMoving = false;
	private bool changeGraph = false;
	
#endregion

#region Events
	
	public UnityEvent PlayerGetStarEvent;
	
#endregion

#region Unity functions
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		bc2d = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
		PlayerGetStarEvent = new UnityEvent();
		GameManager.instance.Star = this;
	}
	
	private void Update()
	{
		ChangeDirection();
	}
	
	private void FixedUpdate()
	{
		Move();
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			PlayerGetStarEvent.Invoke();
			Destroy(gameObject);
		}
		if(isMoving && !changeGraph && collider.gameObject.tag == "Ground")
			changeGraph = true;
	}
	
#endregion

#region Other functions
	
	private void Move()
	{
		if(isMoving)
			rb.MovePosition(Graphic());
	}
	
	private void ChangeState()
	{
		anim.enabled = false;
		isMoving = true;
	}
	
	private Vector2 Graphic()
	{
		if(!changeGraph)
			return new Vector2(transform.position.x + speed, height * Mathf.Sin((transform.localPosition.x + speed) / width + x) + y);
		else
			return new Vector2(transform.position.x + speed, Mathf.Abs(newHeight * Mathf.Sin(transform.position.x / newWidth + newX)) + newY);
	}
	
	private void ChangeDirection()
	{
		RaycastHit2D raycastRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.right, 0.1f, groundLayer);
		if(isMoving && raycastRight.collider != null)
			speed = -speed;
	}
	
#endregion

}