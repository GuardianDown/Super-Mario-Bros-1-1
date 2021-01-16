using UnityEngine;

public class Fireball : MonoBehaviour
{
	
#region Serializable fields	
	
	[SerializeField] private float speedX = 30;
	[SerializeField] private float speedY = 55;
	[SerializeField] private float vectorYStartValue = -0.5f;
	[SerializeField] private float vectorYdown = -0.2f;
	[SerializeField] private float vectorYup = 0.2f;
	[SerializeField] private LayerMask groundLayer = 0;
	
#endregion

#region Components	
	
	private Rigidbody2D rb;
	private BoxCollider2D bc2d;
	
#endregion
	
#region Private fields
	
	private float vectorY;
	private float positionY;
	
#endregion

#region Unity functions	
	
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		bc2d = GetComponent<BoxCollider2D>();
		vectorY = vectorYStartValue;
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Ground")
		{
			vectorY = vectorYup;
			positionY = transform.position.y;
		}
	}
	
	private void Update()
	{
		DestroyFireball();
	}
	
	private void FixedUpdate()
	{
		rb.velocity = new Vector2(1.0f * speedX, vectorY * speedY);
		if(transform.position.y - positionY >= 1 && vectorY == vectorYup)
			vectorY = vectorYdown;
	}
	
#endregion

#region Other functions	
	
	private void DestroyFireball()
	{
		RaycastHit2D raycastRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.right, 0.1f, groundLayer);
		RaycastHit2D raycastLeft = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.left, 0.1f, groundLayer);
		if(raycastLeft.collider != null || raycastRight.collider != null)
		{
			GameManager.instance.Player.MaxFireballCount++;
			Destroy(gameObject);
		}
		if(Mathf.Abs(GameManager.instance.Player.transform.position.x - transform.position.x) > 26)
		{
			GameManager.instance.Player.MaxFireballCount++;
			Destroy(gameObject);
		}
	}
	
#endregion
	
}
