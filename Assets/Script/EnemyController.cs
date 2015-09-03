using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
	public float maxSpeed 					= 1f;
	public float margin 					= 3.2f;
	private float timeToShoot				= 0.5f;
	private Rect enemyBound 				= new Rect();
	public float horzExtent;

	public enum State
	{
		MoveLeft,
		MoveRight,
		MoveDown,
		EnemyS,
	}
	private State state;

	void Start()
	{
		//initialize the state to moveleft
		state=State.MoveLeft;
	}//Start

	void Update ()
	{
		//Check if GameController instance already exists
		if(!GameController.instance.IsGamePlay())
			return;
		horzExtent = (Camera.main.orthographicSize * Screen.width / Screen.height);

		switch(state)
		{
		case State.MoveLeft:
			EnemyMoveLeft();
			EnemyShoot();
			break;

		case State.MoveRight:
			EnemyMoveRight();
			EnemyShoot();
			break;
			
		case State.MoveDown:
			EnemyShoot();
			break;
		}//switch case
	}//update

	private void EnemyMoveLeft()
	{
		CalculateEnemyBounds();

		if(enemyBound.xMin<-horzExtent+margin)
		{
			transform.Translate(Vector3.down*0.1f);
			state=State.MoveRight;
		}
		else
		{
			Vector3 pos = transform.position;
			pos.x-=maxSpeed*Time.deltaTime;
			transform.position=pos;
		}
	}//enemyMoveLeft

	private void EnemyMoveRight()
	{
		CalculateEnemyBounds();

		if(enemyBound.xMax>horzExtent-margin)
		{
			transform.Translate(Vector3.down*0.1f);
			state=State.MoveLeft;
		}
		else
		{
			Vector3 pos = transform.position;
			pos.x+=maxSpeed*Time.deltaTime;
			transform.position=pos;
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
			_mypool.Generator();
			GameController.instance.audionager.play("invaderkilled");
			timeToShoot = Random.Range(0.25f, 0.75f);//random time
		}
	}//enemyShoot
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