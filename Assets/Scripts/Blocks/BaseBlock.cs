using UnityEngine;

public abstract class BaseBlock : MonoBehaviour
{

#region Serializable fields
	
	[SerializeField] private Bonus bonusIfSmall = null;
	[SerializeField] private Bonus bonusIfBig = null;
	[SerializeField] private int bonusCount = 0;
	[SerializeField] private AudioSource bumpSound = null;
	
#endregion
	
#region Private fields
	
	protected Animator anim;
	private GameObject enemyOnBlock = null;
	
#endregion
	
#region Unity functions
	
	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
	
	private void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Enemy")
			enemyOnBlock = collider.gameObject;
	}
	
	private void OnCollisionExit2D(Collision2D collider)
	{
		if(collider.gameObject.tag == "Enemy")
			enemyOnBlock = null;
	}
	
#endregion	
	
#region Other functions	
	
	public virtual void Bump(Player player)
	{
		bumpSound.Play();
		InstantiateBonus(player);
		StartAnimation();
		if(enemyOnBlock != null)
				Destroy(enemyOnBlock);
	}
	
	private void StartAnimation()
	{
		anim.SetBool("isBump", true);
	}
	
	private void InstantiateBonus(Player player)
	{
		if(bonusIfSmall != null && bonusCount > 0)
		{
			Bonus bonus = bonusIfSmall;
			if(!player.IsSmall && bonusIfBig != null)
				bonus = bonusIfBig;
			Instantiate(bonus, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation, transform);
			bonusCount--;
		}
	}

#endregion	
	
}
