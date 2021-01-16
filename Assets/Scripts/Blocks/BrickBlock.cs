using UnityEngine;
using System.Collections;

public class BrickBlock : BaseBlock
{
	[SerializeField] private AudioSource smashSound = null;
	
	public override void Bump(Player player)
	{
		base.Bump(player);
		if(player.IsSmall)
			StartCoroutine("AnimationPlayCoroutine");
		else
		{
			smashSound.Play();
			Destroy(gameObject);
		}
	}
	
	private IEnumerator AnimationPlayCoroutine()
	{
		yield return new WaitForSeconds(0.01f);
		anim.SetBool("isBump", false);
	}		
}
