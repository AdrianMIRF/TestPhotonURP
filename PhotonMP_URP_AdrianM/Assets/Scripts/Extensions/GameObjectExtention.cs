using UnityEngine;

public static class GameObjectExtention
{
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rot)
	{
		return ObjectPool.Spawn(prefab, parent, position, rot);
	}

	public static void Recycle(this GameObject obj)
	{
		ObjectPool.Recycle(obj);
	}
}