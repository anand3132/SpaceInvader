using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
	public float maxSpeed 		= 1f;
	public float margin 		= 3.2f;
	private float timeToShoot	= 0.5f;
	private Rect enemyBound 	= new Rect();
	public float horzExtent;

	public enum State
	{
		kMoveLeft,
		kMoveRight,
		kMoveDown,
		kStateMax
	}

	private State state;

	void Start()
	{
		//initialize the state to moveleft
		state=State.kMoveLeft;
	}//Start

	void Update ()
	{
		if(!GameController.instance.IsGamePlay())
			return;

		//Horizontal extend of the screen.
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);

		switch(state)
		{
		case State.kMoveLeft:
			EnemyMoveLeft();
			EnemyShoot();
			break;

		case State.kMoveRight:
			EnemyMoveRight();
			EnemyShoot();
			break;
			
		case State.kMoveDown:
			EnemyShoot();
			break;
		}//switch case

		CheckEnemyCount();
	}//update

	private void EnemyMoveLeft()
	{
		CalculateEnemyBounds();

		if(enemyBound.xMin<-horzExtent+margin)
		{
			//for each direction change we move our enemy down for 0.1 units.
			transform.Translate(Vector3.down*0.1f);
			state = State.kMoveRight;
		}
		else
		{
			Vector3 pos = transform.position;
			pos.x-=maxSpeed*Time.deltaTime;
			transform.position = pos;
		}
	}//enemyMoveLeft

	private void EnemyMoveRight()
	{
		CalculateEnemyBounds();

		if(enemyBound.xMax>horzExtent-margin)
		{
			//for each direction change we move our enemy down for 0.1 units.
			transform.Translate(Vector3.down*0.1f);
			state=State.kMoveLeft;
		}
		else
		{
			Vector3 pos = transform.position;
			pos.x+=maxSpeed*Time.deltaTime;
			transform.position = pos;
		}
	}//enemyMoveRight()

	private void EnemyShoot()
	{
		timeToShoot-=Time.deltaTime;

		//check wether its time to shoot
		if(gameObject.transform.childCount==0 || timeToShoot>0.0f)
			return;

		//selecting  enemy from the list to fire in random mode
		GameObject _randomChild=gameObject.transform.GetChild(
			Random.Range(0, gameObject.transform.childCount)).gameObject;

		if(_randomChild!=null)
		{
			//get the random child pool(ammo) to fire
			ObjectPool _mypool = _randomChild.GetComponent<ObjectPool>();
			_mypool.GeneratePoolObject();
			GameController.instance.audionager.play("invaderkilled");
			timeToShoot = Random.Range(0.25f, 0.75f);	//random time between 0.25 - 0.75 seconds
		}
	}//enemyShoot

	//Increase the speed of the enemy after certain decrease in enemy count
	void CheckEnemyCount()
	{
		if(EnemyCount()<18)
			maxSpeed = 4;
		else if(EnemyCount()<10)
			maxSpeed = 6;
	}

	//return the no of enemy left 
	public int EnemyCount()
	{
		return gameObject.transform.childCount;
	}

	private void CalculateEnemyBounds()
	{
		enemyBound.xMax = float.MinValue;
		enemyBound.xMin = float.MaxValue;

		foreach(Transform _child in gameObject.transform)
		{
			if(_child.position.x < enemyBound.xMin)
				enemyBound.xMin = _child.position.x;
			if(_child.position.x > enemyBound.xMax)
				enemyBound.xMax = _child.position.x;
		}

	}//calculateEnemyBounds
}//MonoBehaviour