using UnityEngine;

public static class FloatExtensions
{
    /// <summary>
    /// Remap la valeur actuelle dans un nouvel intervalle. genre 0.5 est entre 0 et 1 puis il passe de 0 a 100
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from1"></param>
    /// <param name="to1"></param>
    /// <param name="from2"></param>
    /// <param name="to2"></param>
    /// <returns></returns>
	public static float Remap (this float value, float from1, float to1, float from2, float to2)
	{
		return Mathf.Clamp ((value - from1) / (to1 - from1) * (to2 - from2) + from2, Mathf.Min (from2, to2), Mathf.Max (from2, to2));
	}

    /// <summary>
    /// Remap la valeur actuelle en pourcentage
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from1"></param>
    /// <param name="to1"></param>
    /// <returns></returns>
	public static float RemapPercent (this float value, float from1, float to1)
	{
		return  Mathf.Clamp ((value - from1) / (to1 - from1) * 100, 0, 100);
	}

    /// <summary>
    /// Permet de normaliser une rotation en degrés et éviter de dépasser les 360 degrés
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
	public static float rotationNormalizedDeg (this float rotation)
	{
		rotation = rotation % 360f;
		if (rotation < 0)
			rotation += 360f;
		return rotation;
	}

    /// <summary>
    /// Permet de normaliser une rotation en radiants et éviter de dépasser PI
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
	public static float rotationNormalizedRad (this float rotation)
	{
		rotation = rotation % Mathf.PI;
		if (rotation < 0)
			rotation += Mathf.PI;
		return rotation;
	}

}
