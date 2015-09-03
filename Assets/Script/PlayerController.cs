using UnityEngine;
using System.Collections;

//class specify player behaviour
public class PlayerController : MonoBehaviour
{
	public int playerHealth  	= 100;
	public int score 			= 0;
	private float maxSpeed 		= 4f;
	private float margin 		= 3.2f;
	private float 	timeToShoot = 0.0f;
	public ObjectPool poolInstance;

	void Start()
	{
		PlayerRespawn();
	}

	// Update is called once per frame
	void Update ()
	{
		//Check if GameController instance already exists
		if(!GameController.instance.IsGamePlay())
			return;

		//Move left and right when left and right key pressed
		Vector3 _pos =transform.position;
		_pos.x+=Input.GetAxis("Horizontal")*maxSpeed*Time.deltaTime;

		//Horizontal boundaries for player movement
		float _horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);

		//fire ammo when left and right arrow keys pressed 
		if (Input.GetButtonDown("Fire1"))
		{
			timeToShoot-=Time.deltaTime;
			if(timeToShoot<=0.0f)
			{
				//time delay on each shoot in seconds
				timeToShoot=0.03f;
				poolInstance.Generator();
				GameController.instance.audionager.play("shoot");
			}
		}
		//check if player reach the horizontal boundary
		if(_pos.x<-_horzExtent+margin)
		{
			_pos.x=-_horzExtent+margin;
		}
		else if(_pos.x>_horzExtent-margin)
		{
			_pos.x=_horzExtent-margin;
		}

		transform.position=_pos;
	}//Update

	//Player regenerator
	public void PlayerRespawn()
	{
		playerHealth = 100;
		gameObject.SetActive(true);
		gameObject.transform.position=new Vector3(0.0f, -2.5f, 0.0f);
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		//check enemy/ammo collide with player
		if(_other.tag=="EnemyAmmo"||_other.tag=="BigBossAmmo")
		{
			HitOnPlayer(25);
		}
		if(_other.tag=="Enemy"||_other.tag=="BigBoss")
		{
			HitOnPlayer(25);
		}
	}

	public void HitOnPlayer(int _lifeToReduce)
	{
		playerHealth-=_lifeToReduce;
		//check for Lose condition
		if(playerHealth<=0)
		{
			playerHealth=0;
			GameController.instance.SetState(GameController.GameState.kGameLose);
			gameObject.SetActive(false);	//player destroyed
		}
	}//HitOnPlayer()
}
