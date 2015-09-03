using UnityEngine;
using System.Collections;
//This class controls diffrent game states in the game
public class GameController : MonoBehaviour
{
	static public GameController instance = null;
	public PlayerController playerController = null;
	public EnemyController enemyController = null;
	public AudioManager audionager = null;
	public int gameLevel = 1;
	public GameState gameState = GameState.kMaxState;

	public enum GameState
	{
		kTapToContinue,
		kGamePlay,
		kGameWin,
		kGameLose,
		kPause,
		kMaxState
	}
	
	// Use this for initialization
	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exist
		if (instance==null)
		{
			//if not, set instance to this
			instance = this;
			//instance.respawnEnemies();
		}
		//If instance already exists and it's not this:
		else if (instance!=this)
		{	
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		}
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		SetState(GameState.kTapToContinue);
	}

	// Update is called once per frame
	void Update ()
	{
		switch (gameState)
		{
			case GameState.kTapToContinue:
			{
				if(Input.anyKeyDown)
				{
					RespawnEnemies();
					playerController.PlayerRespawn();
					SetState(GameState.kGamePlay);
				}
			}
				break;

			case GameState.kGamePlay:
				CheckForPauseKey();
				break;

			case GameState.kGameWin:
			{
				if(Input.GetKeyUp(KeyCode.Space))
				{
					SetState(GameState.kTapToContinue);
				}
				CheckForPauseKey();
			}
				break;

			case GameState.kGameLose:
			{
				playerController.score=0;
				if(Input.GetKeyUp(KeyCode.Space))
				{
					SetState(GameState.kTapToContinue);
				}
			}
				break;
			case GameState.kPause:
			{
				if(Input.GetKeyUp(KeyCode.Escape))
				{
					SetState(GameState.kGamePlay);
				}
			}
				break;
		}
	}//update

	public void checkWinCondition()
	{
		if(gameState!=GameState.kGameWin && enemyController.EnemyCount()==1)
		{
			SetState(GameState.kGameWin);
			gameLevel+=1;
		}
	}

	public bool IsPaused()
	{
		return gameState==GameState.kPause;
	}

	public bool IsGamePlay()
	{
		return (gameState==GameState.kGamePlay);
	}

	public void SetState(GameState state)
	{
		if(gameState==state)
			return;

		gameState = state;
	}

	void CheckForPauseKey()
	{
		//If paused go to pause state
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			SetState(GameState.kPause);
		}
	}

	//Enemy regenerator
	void RespawnEnemies()
	{
		if(enemyController!=null)
		{
			GameObject.Destroy(enemyController.gameObject);
		}
		Object _obj = Resources.Load("Prefab/EnemyController");
		GameObject _enemyObj = (GameObject)GameObject.Instantiate(_obj);
		enemyController = _enemyObj.GetComponent<EnemyController>();
	}

	void OnGUI()
	{
		switch (gameState)
		{
		case GameState.kTapToContinue:
		{
			GUI.Label(new Rect(Screen.width*0.4f,Screen.height*0.18f, 200, 50), "TAP TO CONTINUE.");
			GUI.Label(new Rect(Screen.width*0.4f,Screen.height*0.35f, 200, 50), "Ctrl / Mouse L button == FIRE");
			GUI.Label(new Rect(Screen.width*0.4f,Screen.height*0.38f, 200, 50), "Left / Right arrow keys == MOVE");
		}
			break;
		case GameState.kGamePlay:
			GUI.Label(new Rect(Screen.width*0.2f, Screen.height*0.18f, 200, 50), "Score " + playerController.score.ToString());
			GUI.Label(new Rect(Screen.width*0.4f, Screen.height*0.18f, 200, 50), "Health " + playerController.playerHealth.ToString());
			GUI.Label(new Rect(Screen.width*0.6f, Screen.height*0.18f, 200, 50), "Level " + gameLevel.ToString());
			//int nEnemies = GameController.instance.enemyController.EnemyCount();
			//GUI.Label(new Rect(Screen.width*0.6f, Screen.height*0.22f, 200, 50), "nEnemies " + nEnemies.ToString());
			break;
		case GameState.kGameWin:
		{
			GUI.Label(new Rect(Screen.width*0.4f, Screen.height*0.18f, 300, 50), "!!! YOU WON !!!. Press Space to continue.");
		}
			break;
		case GameState.kGameLose:
		{
			GUI.Label(new Rect(Screen.width*0.4f, Screen.height*0.18f, 300, 50), "!!! YOU LOSE !!!. Press Space to continue.");
		}
			break;
		case GameState.kPause:
		{
			GUI.Label(new Rect(Screen.width*0.4f, Screen.height*0.18f, 300, 50), "PAUSED, Press Escape to resume.");
		}
			break;
		}
	}
}
