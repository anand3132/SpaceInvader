using UnityEngine;
using System.Collections;

//class specifies enmy behaviour,Added with each enemy game object 
public class Enemy : MonoBehaviour
{
	public Sprite 	enemyNormalSprite 	= null;
	public Sprite	enemyHitSprite 		= null;
	private int 	enemyHealth			= 100;
	private float 	timeToGoNormal 		= 0.0f;		//in seconds
	private bool 	isHitHappened		= true;
	public float 	baseMargine			=-1;

	// Use this for initialization
	void Start ()
	{
		enemyNormalSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!GameController.instance.IsGamePlay())
			return;

		//Change the sprite to normal if a hit has happened in the past.
		if(isHitHappened)
		{
			timeToGoNormal-=Time.deltaTime;
			if(timeToGoNormal<=0.0f)
			{
				timeToGoNormal = 0.0f;
				isHitHappened = false;
				SpriteRenderer _spriteRenderer = (SpriteRenderer)gameObject.GetComponent<SpriteRenderer>();
				_spriteRenderer.sprite = enemyNormalSprite;
				_spriteRenderer.color = Color.white;
			}
		}
//		Vector3 _pos=new Vector3();
//		_pos=transform.position;
//		if(transform.position.y<baseMargine)
//		{
//			_pos.y=0;
//			transform.position=_pos;
//		}
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		//Check if enemy hit by player ammo 
		if(gameObject.tag=="Enemy" && _other.tag=="PlayerAmmo")
		{
			HitOnEnemy(50);	//reduce 50 life points when the player ammo hits a normal enemy.
		}
		//Check if BigBoss hit by player ammo 
		if(gameObject.tag=="BigBoss" && _other.tag=="PlayerAmmo")
		{
			HitOnEnemy(10);	//reduce 10 life points when the player ammo hits a BigBoss enemy.
		}
	}

	public void HitOnEnemy(int _lifeToReduce)
	{
		enemyHealth-=_lifeToReduce;
		GameController.instance.playerController.score+=10;
		
		if(enemyHealth<0)
		{
			//Enemy destroyed
			gameObject.SetActive(false);	
			GameObject.Destroy(gameObject);
			GameController.instance.checkWinCondition();
		}
		else
		{
			//change the sprite on hit
			SpriteRenderer _spriteRenderer = (SpriteRenderer)gameObject.GetComponent<SpriteRenderer>();
			_spriteRenderer.sprite = enemyHitSprite;
			_spriteRenderer.color = Color.red;
			timeToGoNormal = 0.15f;	//in seconds
			isHitHappened = true;
		}
	}// HitOnEnemy()
}
