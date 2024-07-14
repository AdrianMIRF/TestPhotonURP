using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class ObjectPool : MonoBehaviour
{
	[Serializable]
	public class StartupPool
	{
		public int size;
		public GameObject prefab;
	}

	public StartupPool[] startupPools;
	public static ObjectPool Instance;

	private Dictionary<GameObject, List<GameObject>> _pooledObjects = new Dictionary<GameObject, List<GameObject>>();
	private Dictionary<GameObject, GameObject> _spawnedObjects = new Dictionary<GameObject, GameObject>();

	void Start()
	{
		Instance = this;
		CreateStartupPools();
	}

	//create initial pooled objects
	public static void CreateStartupPools()
	{
		StartupPool[] pools = Instance.startupPools;
		if (pools != null && pools.Length > 0)
		{
			for (int i = 0; i < pools.Length; ++i)
			{
				CreatePool(pools[i].prefab, pools[i].size);
			}
		}
	}

	//instantiate the game objects and set them to inactive
	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (prefab != null && Instance._pooledObjects.ContainsKey(prefab) == false)
		{
			List<GameObject> list = new List<GameObject>();
			Instance._pooledObjects.Add(prefab, list);

			if (initialPoolSize > 0)
			{
				bool active = prefab.activeSelf;
				prefab.SetActive(false);
				Transform parent = Instance.transform;

				while (list.Count < initialPoolSize)
				{
					GameObject obj = Instantiate(prefab);
					obj.transform.SetParent(parent);
					list.Add(obj);
				}

				prefab.SetActive(active);
			}
		}
	}

	//create or activate pooled object
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rot)
	{
		List<GameObject> list;
		Transform trans;
		GameObject obj;

		//check if the game object is instantiated, if not instantiate it 
		if (Instance._pooledObjects.TryGetValue(prefab, out list))
		{
			obj = null;

			if (list.Count > 0)
			{
				//preform clean up
				while (obj == null && list.Count > 0)
				{
					obj = list[0];
					list.RemoveAt(0);
				}

				//activate game object
				if (obj != null)
				{
					trans = obj.transform;

					if (parent != null)
					{
						trans.SetParent(parent);
					}

					trans.localPosition = position;
					trans.rotation = rot;

					obj.SetActive(true);

					Instance._spawnedObjects.Add(obj, prefab);

					return obj;
				}
			}

			//if there is no game object in list instantiate and add to spawned list
			obj = Instantiate(prefab);

			trans = obj.transform;

			if (parent != null)
			{
				trans.SetParent(parent);
			}

			trans.localPosition = position;
			trans.rotation = rot;

			Instance._spawnedObjects.Add(obj, prefab);

			return obj;
		}
		else
		{
			//Debug.LogError(Time.time + " obj to Spawn not in pooled list -> " + prefab);

			obj = Instantiate(prefab);

			trans = obj.GetComponent<Transform>();

			if (parent != null)
			{
				trans.SetParent(parent);
			}

			trans.localPosition = position;
			trans.rotation = rot;

			return obj;
		}
	}

	//recycle object or destroy it if dictionary ref is gone
	public static void Recycle(GameObject obj)
	{
		GameObject prefab;

		if (Instance._spawnedObjects.TryGetValue(obj, out prefab))
		{
			Recycle(obj, prefab);
		}
		else
		{
			//Debug.Log(Time.time + " obj not in pooled list -> " + obj);
			Destroy(obj);
		}
	}

	private static void Recycle(GameObject obj, GameObject prefab)
	{
		Instance._pooledObjects[prefab].Add(obj);
		Instance._spawnedObjects.Remove(obj);

		obj.transform.SetParent(Instance.transform);
		obj.SetActive(false);
	}
}