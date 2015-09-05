using UnityEngine;
using System.Collections;

//class specify Ammo behaviour
public class Ammo : MonoBehaviour
{
	void OnEnable()
	{
		//Destroy the Ammo after every 4 seconds
		Invoke("DestroyAmmo", 4f);
	}

	void OnDisable()
	{
		CancelInvoke();
	}

	// Update is called once per frame
	void Update ()
	{
		if(!GameController.instance.IsGamePlay())
			return;
		//seed at which ammo to be propelled
		transform.localPosition += (gameObject.transform.up * Time.deltaTime*2.0f);
	}

	void DestroyAmmo()
	{
		gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		//check collision wether enemy/player collide with player/enemy ammo
		switch(_other.tag)
		{
		case "Enemy":
			if(gameObject.tag=="PlayerAmmo")
				gameObject.SetActive(false);
			break;
		case "BigBoss":
			if(gameObject.tag=="PlayerAmmo")
				gameObject.SetActive(false);
			break;
			
		case "Player":
		case "PlayerAmmo":
		case "EnemyAmmo":
			if(gameObject.tag=="EnemyAmmo"||gameObject.tag=="BigBossAmmo")
				gameObject.SetActive(false);
			break;
			
		}//switch case
	}//OnTriggerEnter2D

}//MonoBehaviour
