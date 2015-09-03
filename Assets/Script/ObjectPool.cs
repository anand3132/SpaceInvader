using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Object pool pattern used in player and enemy ammo generation 
public class ObjectPool : MonoBehaviour
{
	public List<GameObject>		pooledObjectList;
	public GameObject 			pooledObject;
	public int					poolGenerationSize	=	20;
	public bool 				willGrow			=	true;
	private GameObject			poolParent;

	// Use this for initialization
	void Start ()
	{
		poolParent = new GameObject("PoolParent");
		pooledObjectList=new List<GameObject>();
		//Loop to fill the pool List with poolobject and set each object active state to false   
		for(int i=0;i<poolGenerationSize;i++)
		{
			GameObject _obj=(GameObject)Instantiate(pooledObject);
			_obj.SetActive(false);
			_obj.transform.SetParent(poolParent.transform);
			pooledObjectList.Add(_obj);
		}
	}//Start

	//Returns active poolobject from the pool List
	public GameObject GetNextActivePooledObject()
	{
		for(int i=0;i<pooledObjectList.Count;i++)
		{
			if(!pooledObjectList[i].activeInHierarchy)
			{
				pooledObjectList[i].SetActive(true);
				return pooledObjectList[i];
			}
		}
		//If active poolobject not found in the pool list create new one
		if(willGrow)
		{
			GameObject _firstObject=null;
			for(int x=0;x<poolGenerationSize;x++)
			{
				GameObject obj =(GameObject)Instantiate(pooledObject);
				pooledObjectList.Add(obj);
				obj.SetActive(false);
				obj.transform.SetParent(poolParent.transform);
				if(_firstObject==null)
				{
					_firstObject=obj;
				}
			}

			_firstObject.SetActive(true);
			//return newly created active game object
			return _firstObject;
		}
		//If can't able to find pooledobject in the list and can't able to create new return null
		return null;
	}//GetNextActivePooledObject

	//Called when ever an object is needed from the pool 
	public GameObject Generator()
	{
		//getting object from the pool
		GameObject _obj=GetNextActivePooledObject();
		if(_obj==null)
		{
			return null;
		}
		// position were to instantiate over here
		_obj.transform.position = transform.position;
		_obj.transform.rotation = transform.rotation;

		return _obj;
	}//Generator
	
	void OnDestroy()
	{
		GameObject.Destroy(poolParent);
	}
	
}
