using UnityEngine;
using System.Collections.Generic;




//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class GameObjectExtensions
{
    /// <summary>
    /// Change le layer du gameObject et de tous ses enfants
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="layer"></param>
	public static void SetLayerRecursively (this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		foreach (Transform t in gameObject.transform)
			t.gameObject.SetLayerRecursively (layer);
	}

	/// <summary>
    /// Renvoie tous les components souhaités des enfants du gameObject ayant un tag spécifique
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
	public static T[] GetComponentsInChildrenWithTag<T> (this GameObject gameObject, string tag) where T: Component
	{
		List<T> results = new List<T> ();

		if (gameObject.CompareTag (tag))
			results.Add (gameObject.GetComponent<T> ());

		foreach (Transform t in gameObject.transform)
			results.AddRange (t.gameObject.GetComponentsInChildrenWithTag<T> (tag));

		return results.ToArray ();
	}


	/// <summary>
	/// Get An Object’s Collision Mask
	/// </summary>
	/// <returns>The collision mask.</returns>
	/// <param name="gameObject">Game object.</param>
	/// <param name="layer">Layer.</param>
	public static int GetCollisionMask (this GameObject gameObject, int layer = -1)
	{
		if (layer == -1)
			layer = gameObject.layer;

		int mask = 0;
		for (int i = 0; i < 32; i++)
			mask |= (Physics.GetIgnoreLayerCollision (layer, i) ? 0 : 1) << i;

		return mask;
	}

	/// <summary>
	/// Gets the child with tag.
	/// </summary>
	/// <returns>The child with tag.</returns>
	/// <param name="self">Self.</param>
	/// <param name="tag">Tag.</param>
	public static GameObject GetChildWithTag (this GameObject self, string tag)
	{
		foreach (Transform t in self.transform)
		{
			if (t.gameObject.CompareTag (tag))
			{
				return t.gameObject;
			}
			GameObject temp = t.gameObject.GetChildWithTag (tag);
			if (temp != null)
				return temp;
		}

		return null;
	}


	/// <summary>
	/// Gets the child  with name.
	/// </summary>
	/// <returns>The child with tag.</returns>
	/// <param name="self">Self.</param>
	/// <param name="tag">Tag.</param>
	public static GameObject GetChildNamed (this GameObject self, string name)
	{
		foreach (Transform t in self.transform)
		{
			if (t.gameObject.name == name)
			{
				return t.gameObject;
			}
			GameObject temp = t.gameObject.GetChildNamed (name);
			if (temp != null)
				return temp;
		}

		return null;
	}


	/// <summary>
	/// Gets the children (au pluriel) with tag.
	/// </summary>
	/// <returns>The children with tag.</returns>
	/// <param name="self">Self.</param>
	/// <param name="tag">Tag.</param>
	public static GameObject[] GetChildrenWithTag (this GameObject self, string tag)
	{
		List<GameObject> results = new List<GameObject> ();

		foreach (Transform t in self.transform)
		{
			if (t.gameObject.CompareTag (tag))
			{
				results.Add (t.gameObject);
			}
			results.AddRange (t.gameObject.GetChildrenWithTag (tag));
		}

		return results.ToArray ();
	}


    /// <summary>
    /// Gets the children (au pluriel)  with name.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="name"></param>
    /// <returns></returns>
	public static GameObject[] GetChildrenNamed (this GameObject self, string name)
	{
		List<GameObject> results = new List<GameObject> ();

		foreach (Transform t in self.transform)
		{
			if (t.gameObject.name == name)
			{
				results.Add (t.gameObject);
			}
			results.AddRange (t.gameObject.GetChildrenNamed (name));
		}

		return results.ToArray ();
	}

	/// <summary>
	/// Gets the parent with tag.
	/// </summary>
	/// <returns>The parent with tag.</returns>
	/// <param name="self">Self.</param>
	/// <param name="tag">Tag.</param>
	public static GameObject GetParentWithTag (this GameObject self, string tag)
	{
		Transform t = self.transform;
		for (; t != null; t = t.parent)
		{
			if (t.CompareTag (tag))
			{
				return t.gameObject;
			}
		}

		return null;
	}


    /// <summary>
    /// Gets the parent with name.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="name"></param>
    /// <returns></returns>
	public static GameObject GetParentWithName (this GameObject self, string name)
	{
		Transform t = self.transform;
		for (; t != null; t = t.parent)
		{
			if (t.name == name)
			{
				return t.gameObject;
			}
		}

		return null;
	}


	/// <summary>
	/// Gets all monobehaviours in children that implement the class of type T (casted to T)
	/// works with interfaces
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="gObj"></param>
	/// <returns></returns>
	public static T[] GetClassesInChildren<T> (this GameObject gObj) where T : class
	{
		var ts = gObj.GetComponentsInChildren (typeof(T));

		var ret = new T[ts.Length];
		for (int i = 0; i < ts.Length; i++)
		{
			ret [i] = ts [i] as T;
		}
		return ret;
	}

	/// <summary>
	/// 
	/// Returns the first instance of the monobehaviour that is of the class type T (casted to T)
	/// works with interfaces
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="gObj"></param>
	/// <returns></returns>
	public static T GetClassInChildren<T> (this GameObject gObj) where T : class
	{
		return gObj.GetComponentInChildren (typeof(T)) as T;
	}


}