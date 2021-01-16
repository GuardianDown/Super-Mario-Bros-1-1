using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Player : MonoBehaviour
{

#region Serializable fields

	[SerializeField] private float speed = 1.0f;
	[SerializeField] private float jumpImpulse = 1.0f;
	[SerializeField] private float jumpForce = 1.0f;
	[SerializeField] private float jumpImpulseByEnemy = 1.0f;
	[SerializeField] private LayerMask groundLayer = 0;
	[SerializeField] private LayerMask enemyLayer = 0;
	[SerializeField] private LayerMask playerLayer = 0;
	[SerializeField] private float jumpTime = 0;
	[SerializeField] private AudioSource jumpSoundSmall = null;
	[SerializeField] private AudioSource jumpSoundBig = null;
	[SerializeField] private AudioSource powerUpSound = null;
	[SerializeField] private AudioSource powerDownSound = null;
	[SerializeField] private Fireball fireball = null;
	[SerializeField] private int maxFireballCount = 2;
	
#endregion
	
#region Components	

	private Rigidbody2D rb;
	private Animator anim;
	private BoxCollider2D bc2d;
	private SpriteRenderer sr;
	
#endregion
	
#region Private fields

	private float jumpTimeCounter = 0;
	private bool fireAxisUse = false;
	private bool jumpAxisUse = false;
	private bool isStar = false;
	private bool hasHitCeiling = false;
	private bool isFall = false;
	
#endregion
	
#region Events

	public UnityEvent PlayerDiedEvent;
	public UnityEvent PlayerFallEvent;
	public MyIntEvent EnemyDieEvent;
	
#endregion
	
#region Properties
	
	public bool IsSmall
	{
		get
		{
			return anim.GetBool("isSmall");
		}
		set
		{
			anim.SetBool("isSmall", value);
		}
	}
	
	private	bool IsFireball
	{
		get
		{
			return anim.GetBool("isFireball");
		}
		set
		{
			anim.SetBool("isFireball", value);
		}
	}
	
	private bool IsStar
	{
		get
		{
			return isStar;
		}
		set
		{
			isStar = value;
		}
	}
	
	private bool IsDead
	{
		get
		{
			return anim.GetBool("isDead");
		}
		
		set
		{
			anim.SetBool("isDead", value);
		}
	}
	
	private bool IsJump
	{
		get
		{
			return anim.GetBool("isJump");
		}
		
		set
		{
			anim.SetBool("isJump", value);
		}
	}
	
	private bool IsFinish
	{
		get
		{
			return anim.GetBool("isFinish");
		}
		
		set
		{
			anim.SetBool("isFinish", value);
		}
	}
	
	private bool IsRun
	{
		set
		{
			anim.SetBool("isRun", value);
		}
	}
	
	private bool IsTimeStop
	{
		set
		{
			anim.SetBool("isTimeStop", value);
		}
	}
	
	public int MaxFireballCount
	{
		get
		{
			return maxFireballCount;
		}
		set
		{
			maxFireballCount = value;
		}
	}

	private bool Immortal
	{
		get;
		set;
	}
	
#endregion
	
#region Unity functions

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		bc2d = GetComponent<BoxCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		PlayerDiedEvent = new UnityEvent();
		PlayerDiedEvent.AddListener(Die);
		PlayerFallEvent = new UnityEvent();
		EnemyDieEvent = new MyIntEvent();
		GameManager.instance.Player = this;
	}
	
	private void Update()
	{
		Fire();
		Fall();
		OnEnemyEnter();
		OnGroundOrEnemy();
		//DrawRays();
	}
	
	private void FixedUpdate()
	{
		Move();
		Jump();
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Finish")
		{
			rb.simulated = false;
			IsFinish = true;
		}
	}

#endregion

#region Action functions

	private void Move()
	{
		if(Input.GetAxis("Horizontal") != 0 && !IsFinish && !IsDead)
		{
			if(Input.GetAxis("Horizontal") < 0)
			{
				transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
			}
			else if(Input.GetAxis("Horizontal") > 0)
			{
				transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
			}
			rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, 0.0f));
		}
		if(!IsJump && (rb.velocity.x > 0.1f || rb.velocity.x < -0.1f))
			IsRun = true;
		else
			IsRun = false;
	}
	
	private void Jump()
	{
		if(Input.GetAxisRaw("Jump") != 0 && !IsJump && !jumpAxisUse)
		{
			hasHitCeiling = false;
			jumpAxisUse = true;
			jumpTimeCounter = jumpTime;
			rb.AddForce(new Vector2(0.0f, jumpImpulse), ForceMode2D.Impulse);
			if(IsSmall)
				jumpSoundSmall.Play();
			else
				jumpSoundBig.Play();
		}
		if(Input.GetAxisRaw("Jump") == 0)
			jumpAxisUse = false;
		if(Input.GetAxisRaw("Jump") != 0 && jumpTimeCounter > 0 && IsJump && !hasHitCeiling && jumpAxisUse)
		{
			rb.AddForce(new Vector2(0.0f, jumpForce));
			jumpTimeCounter -= Time.deltaTime;
		}
		if(IsJump && !hasHitCeiling)
			hasHitCeiling = IsCeiling();
	}
	
	private void Fire()
	{
		if(IsFireball && Input.GetAxisRaw("Fire1") != 0 && maxFireballCount > 0 && !fireAxisUse && !IsDead)
		{
			fireAxisUse = true;
			Instantiate(fireball, new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), transform.rotation);
			maxFireballCount--;
		}
		if(Input.GetAxisRaw("Fire1") == 0)
			fireAxisUse = false;
	}
	
#endregion

#region Event functions
	
	private void OnEnemyEnter()
	{
		RaycastHit2D boxcastLeft = Physics2D.BoxCast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y), new Vector2(0.1f, bc2d.bounds.extents.y), 0, Vector2.left, 0.1f, enemyLayer);
		RaycastHit2D boxcastRight = Physics2D.BoxCast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), new Vector2(0.1f, bc2d.bounds.extents.y), 0, Vector2.right, 0.1f, enemyLayer);
		RaycastHit2D boxcastUp = Physics2D.BoxCast(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y + bc2d.bounds.extents.y), new Vector2(bc2d.bounds.extents.x, 0.1f), 0, Vector2.up, 0.1f, enemyLayer);
		Enemy enemy = null;
		if(boxcastLeft.collider != null)
		{
			enemy = boxcastLeft.collider.gameObject.GetComponent<Enemy>();
		}
		else if(boxcastRight.collider != null)
		{
			enemy = boxcastRight.collider.gameObject.GetComponent<Enemy>();
		}
		else if(boxcastUp.collider != null)
		{
			enemy = boxcastUp.collider.gameObject.GetComponent<Enemy>();
		}
		if(enemy != null && !IsDead && !Immortal)
		{
			if(IsStar)
			{
				enemy.GetComponent<Enemy>().DieByStar();
			}
			else if(IsSmall)
			{
				GameManager.instance.Enemy = enemy;
				PlayerDiedEvent.Invoke();
			}
			else
			{
				GetSmaller();
				StartCoroutine("GetImmortal");
			}
		}
	}
	
	private void OnGroundOrEnemy()
	{
		Enemy enemy = null;
		RaycastHit2D raycastHitBottomLeft = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.down, 0.1f, groundLayer);
		RaycastHit2D raycastHitBottomRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.down, 0.1f, groundLayer);
		if(raycastHitBottomLeft.collider != null)
		{
			IsJump = false;
			if(raycastHitBottomLeft.collider.gameObject.tag == "Enemy")
			{
				enemy = raycastHitBottomLeft.collider.gameObject.GetComponent<Enemy>();
			}
		}
		else if(raycastHitBottomRight.collider != null)
		{
			IsJump = false;
			if(raycastHitBottomRight.collider.gameObject.tag == "Enemy")
			{
				enemy = raycastHitBottomRight.collider.gameObject.GetComponent<Enemy>();
			}
		}
		else
		{
			IsJump = true;
		}
		if(enemy != null && !enemy.IsDead && !Immortal)
		{
			GameManager.instance.Enemy = enemy;
			EnemyDieEvent.Invoke(enemy.Points);
		}
	}
	
	private bool IsCeiling()
	{
		if(!IsDead)
		{
			RaycastHit2D raycastHitTopMiddle = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y + bc2d.bounds.extents.y), Vector2.up, 0.4f, groundLayer);
			RaycastHit2D raycastHitTopLeft = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y + bc2d.bounds.extents.y), Vector2.up, 0.4f, groundLayer);
			RaycastHit2D raycastHitTopRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y + bc2d.bounds.extents.y), Vector2.up, 0.4f, groundLayer);
			if(raycastHitTopMiddle.collider != null)
			{
				BaseBlock block = raycastHitTopMiddle.collider.gameObject.GetComponent<BaseBlock>();
				block.Bump(this);
			}
			if(raycastHitTopLeft.collider != null && raycastHitTopMiddle.collider == null)
			{
				transform.position = new Vector3(transform.position.x + 0.2f + (raycastHitTopLeft.collider.bounds.center.x + raycastHitTopLeft.collider.bounds.extents.x) - raycastHitTopLeft.point.x, transform.position.y, transform.position.z);
			}
			if(raycastHitTopRight.collider != null && raycastHitTopMiddle.collider == null)
			{
				transform.position = new Vector3(transform.position.x - 0.2f - (raycastHitTopRight.point.x - (raycastHitTopRight.collider.bounds.center.x - raycastHitTopRight.collider.bounds.extents.x)), transform.position.y, transform.position.z);
			}
			return raycastHitTopMiddle.collider != null;
		}
		else
			return false;
	}
	
#endregion
	
#region State functions
	
	public void GetBigger()
	{
		IsSmall = false;
		powerUpSound.Play();
		StartPause();
	}
	
	private void GetSmaller()
	{
		Physics2D.IgnoreLayerCollision(9, 11, true);
		IsSmall = true;
		StartPause();
		powerDownSound.Play();
	}
	
	public void GetFireball()
	{
		if(!IsSmall)
		{
			IsFireball = true;
			powerUpSound.Play();
			StartPause();
		}
		else
			GetBigger();
	}
	
	public void GetStar()
	{
		IsStar = true;
		powerUpSound.Play();
		StartCoroutine("StarAnimation");
	}
	
	private void Fall()
	{
		if(!isFall && !IsDead)
			if(transform.position.y <= - 13.0f)
			{
				isFall = true;
				PlayerFallEvent.Invoke();
			}
	}
	
	private void Die()
	{
		bc2d.enabled = false;
		IsDead = true;
		rb.velocity = new Vector2(0, 0);
		rb.AddForce(new Vector2(0.0f, jumpImpulse), ForceMode2D.Impulse);
	}
	
	private IEnumerator GetImmortal()
	{
		float immortalTime = 0;
		Immortal = true;
		yield return new WaitForSeconds(0.1f);
		while(immortalTime < 3.0f)
		{
			if(sr.enabled == true)
				sr.enabled = false;
			else
				sr.enabled = true;
			immortalTime += 0.05f;
			yield return new WaitForSeconds(0.05f);
		}
		sr.enabled = true;
		Immortal = false;
		Physics2D.IgnoreLayerCollision(9, 11, false);
	}

#endregion	

#region Other functions
	
	private void ChangeColliderSize()
	{
		if(!IsSmall)
		{
			transform.position += new Vector3(0, 0.4f, 0);
			bc2d.size = new Vector2(2, 4);
		}
		else
			bc2d.size = new Vector2(1.625f, 2);
		StopPause();
	}
	
	private void StartPause()
	{
		IsTimeStop = true;
		Time.timeScale = 0;
	}
	
	private void StopPause()
	{
		IsTimeStop = false;
		Time.timeScale = 1;
	}
	
	private void ChangePositionUp()
	{
		transform.position += new Vector3(0, 0.3f, 0);
	}
	
	private void ChangePositionDown()
	{
		transform.position += new Vector3(0, -0.3f, 0);
	}
	
	private IEnumerator StarAnimation()
	{
		float timer = 0;
		while(timer < 15)
		{
			switch(sr.color.ToString())
			{
				case "RGBA(1.000, 1.000, 1.000, 1.000)":
					sr.color = Color.green;
					break;
				case "RGBA(0.000, 1.000, 0.000, 1.000)":
					sr.color = Color.red;
					break;
				case "RGBA(1.000, 0.000, 0.000, 1.000)":
					sr.color = Color.white;
					break;
			}
			timer += 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		sr.color = Color.white;
		IsStar = false;
	}
	
	public void ImpulseByEnemy()
	{
		rb.AddForce(new Vector2(0.0f, jumpImpulseByEnemy), ForceMode2D.Impulse);
	}
	
	/* 	private void DrawRays()
	{
		Debug.DrawRay(new Vector3(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y - bc2d.bounds.extents.y, 0), new Vector3(0, -0.4f, 0), Color.red);
		Debug.DrawRay(new Vector3(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y - bc2d.bounds.extents.y, 0), new Vector3(0, -0.4f, 0), Color.red);
		Debug.DrawRay(new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y + bc2d.bounds.extents.y, 0), new Vector3(0, 0.4f, 0), Color.red);
		Debug.DrawRay(new Vector3(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y + bc2d.bounds.extents.y, 0), new Vector3(0, 0.4f, 0), Color.red);
		Debug.DrawRay(new Vector3(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y + bc2d.bounds.extents.y, 0), new Vector3(0, 0.4f, 0), Color.red);
	} */
	
#endregion	

}
