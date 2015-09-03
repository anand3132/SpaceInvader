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
		//Check if GameController instance already exists
		if(!GameController.instance.IsGamePlay())
			return;
		
		transform.localPosition += (gameObject.transform.up * Time.deltaTime*2.0f);
	}

	void DestroyAmmo()
	{
		gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		//TODO: comment explaining about who collides with whome.
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
