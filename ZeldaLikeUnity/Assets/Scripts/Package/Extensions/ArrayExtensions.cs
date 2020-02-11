
using System.Collections.Generic;

public static class ArrayExtensions
{
      
	/// <summary>
    /// Permet de transférer tous les éléments d'un array dans une liste
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
	public static List<T> ToList<T> (this T[] array)
	{
		
		return new List<T> (array);
	}
    
}