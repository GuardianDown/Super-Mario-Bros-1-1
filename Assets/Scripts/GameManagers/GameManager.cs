using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;

#region Serializable fields

	[SerializeField] private AudioSource levelThemeSound = null;
	[SerializeField] private AudioSource marioDieSound = null;
	[SerializeField] private GameObject pointsText = null;
	
#endregion	

#region Private fields

	private CoinText coinText = null;
	private ScoreText scoreText = null;
	private Coin coin = null;
	private Mushroom mushroom = null;
	private Mushroom1up mushroom1up = null;
	private Flower flower = null;
	private Star star = null;
	private Player player = null;
	private Enemy enemy = null;
	
#endregion	

#region Properties
	
	public CoinText CoinText
	{	
		set
		{
			coinText = value;
		}
	}
	
	public ScoreText ScoreText
	{
		set
		{
			scoreText = value;
		}
	}
	
	public Coin Coin
	{
		set
		{
			coin = value;
			if(coin != null && coinText != null && scoreText != null)
			{
				coin.CoinSpawnEvent.AddListener(coinText.UpdateText);
				coin.CoinSpawnEvent.AddListener(scoreText.UpdateText);
			}
		}
	}
	
	public Mushroom Mushroom
	{
		set
		{
			mushroom = value;
			if(mushroom != null && player != null)
				mushroom.PlayerGetMushroomEvent.AddListener(player.GetBigger);
		}
	}
	
	public Mushroom1up Mushroom1up
	{
		set
		{
			mushroom1up = value;
			if(mushroom1up != null && player != null)
				mushroom1up.PlayerGetMushroom1upEvent.AddListener(LifeCounter.AddLife);
		}
	}
	
	public Flower Flower
	{
		set
		{
			flower = value;
			if(flower != null && player != null)
				flower.PlayerGetFlowerEvent.AddListener(player.GetFireball);
		}
	}
	
	public Star Star
	{
		set
		{
			star = value;
			if(star != null && player != null)
				star.PlayerGetStarEvent.AddListener(player.GetStar);
		}
	}
	
	public Player Player
	{
		get
		{
			return player;
		}
		
		set
		{
			player = value;
			if(player != null)
			{
				player.PlayerDiedEvent.AddListener(PlayerDie);
				player.EnemyDieEvent.AddListener(EnemyDie);
				player.PlayerFallEvent.AddListener(PlayerDie);
			}
		}
	}
	
	public Enemy Enemy
	{
		get
		{
			return enemy;
		}
		
		set
		{
			enemy = value;
		}
	}
	
	public GameObject PointsText
	{
		get
		{
			return pointsText;
		}
	}
	
#endregion
	
#region Unity functions	
	
	private void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance == this)
			Destroy(gameObject);
	}
	
#endregion

#region Other functions	
	
	private void PlayerDie()
	{
		StartCoroutine("PlayerDeadGameOverCoroutine");
		if(enemy != null)
			enemy.Stop();
		LifeCounter.SubtractLife();
		levelThemeSound.Stop();
		marioDieSound.Play();
	}
	
	private IEnumerator PlayerDeadGameOverCoroutine()
	{
		yield return new WaitForSeconds(3.0f);
		if(LifeCounter.Lifes == 0)
			SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
		else
			SceneManager.LoadScene("Load", LoadSceneMode.Single);
	}
	
	private void EnemyDie(int points)
	{
		enemy.Die();
		player.ImpulseByEnemy();
		if(enemy.IsDead)
		{
			scoreText.UpdateText(points);
			pointsText.GetComponent<TextMesh>().text = enemy.Points.ToString();
			pointsText.GetComponent<MeshRenderer>().sortingOrder = 4;
			Instantiate(pointsText, enemy.transform.position + new Vector3(0, 1, 0), enemy.transform.rotation);
		}
	}
	
#endregion
	
}
