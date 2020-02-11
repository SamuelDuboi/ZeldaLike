using UnityEngine;
using System.Collections;

/// <summary>
/// Toutes les fonctions de MathX (ou presque)
/// </summary>
public static class MathX
{
	public const float TwoPI = Mathf.PI * 2.0f;

    /// <summary>
    /// renvoie un angle a 360 genre 371 passe a 11
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
	public static float ClampAngle (float angle, float min, float max)
	{

		if (angle < 90 || angle > 270)
		{       // if angle in the critic region...
			if (angle > 180)
				angle -= 360;  // convert all angles to -180..+180
			if (max > 180)
				max -= 360;
			if (min > 180)
				min -= 360;
		}    
		angle = Mathf.Clamp (angle, min, max);
		if (angle < 0)
			angle += 360;  // if angle negative, convert to 0..360
		return angle;
	}

/// <summary>
/// courbe qui rebondit
/// </summary>
/// <param name="x"></param>
/// <returns></returns>
	public static float Bounce (float x)
	{
		return Mathf.Abs (Mathf.Sin (6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	// test for value that is near specified float (due to floating point inprecision)
	// all thanks to Opless for this!
	public static bool Approx (float val, float about, float range)
	{
		return ((Mathf.Abs (val - about) < range));
	}


    /// <summary>
    /// ptite courbe de Gauss
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="inRadius"></param>
    /// <returns></returns>
	public static float GaussFalloff (float distance, float inRadius)
	{
		return Mathf.Clamp01 (Mathf.Pow (360f, -Mathf.Pow (distance / inRadius, 2.5f) - 0.01f));
	}



	public static string ConvertMinToSeconds (int time)
	{
		var _minutes = (int)(time / 60);
		string minutes = "";
		int _seconds = (time % 60);
		string seconds = "";
		if (_minutes > 0)
			minutes = _minutes + " minute" + (_minutes > 1 ? "s " : " ");
		if (_seconds > 0)
			seconds = _seconds + " second" + (_seconds > 1 ? "s " : " ");
		return (minutes + seconds).Substring (0, (minutes + seconds).Length - 1);
	}


	/// <summary>
	/// is this float within range of other
	/// </summary>
	/// <param name="x"></param>
	/// <param name="other"></param>
	/// <param name="delta"></param>
	/// <returns></returns>
	public static bool Approximately (this float x, float other, float delta)
	{
		return Mathf.Abs (x - other) < delta;
	}
}



