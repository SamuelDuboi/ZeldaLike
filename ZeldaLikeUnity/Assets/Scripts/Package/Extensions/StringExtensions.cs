using System.Text.RegularExpressions;
using System;
using System.Linq;

//pour cryptographie
using System.Text;
using System.Security.Cryptography;

//pour compression gzip
using System.IO;
using System.IO.Compression;

//using System;
using System.Reflection;
using UnityEngine;


public static class StringExtensions
{
    
	/// <summary>
	///  count the Words.
	/// </summary>
	/// <returns>The count.</returns>
	public static int WordCount (this string str)
	{
		return str.Split (new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
	}

	/// <summary>
	/// Raises the digits event.
	/// </summary>
	/// <param name="value">Value.</param>
	/// example 
	/// var ssn = "123-12-1234";
	/// ssn = ssn.OnlyDigits(); // "123121234"
	/// 
	public static string OnlyDigits (this string value)
	{
		return new string (value.Where (c => char.IsDigit (c)).ToArray ());
	}



	/////////cryptographie///////////////
	/// <summary>
	/// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
	/// </summary>
	/// <param name="stringToEncrypt">String that must be encrypted.</param>
	/// <param name="key">Encryptionkey.</param>
	/// <returns>A string representing a byte array separated by a minus sign.</returns>
	/// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
	public static string Encrypt (this string stringToEncrypt, string key)
	{
		if (string.IsNullOrEmpty (stringToEncrypt))
		{
			throw new ArgumentException ("An empty string value cannot be encrypted.");
		}
		
		if (string.IsNullOrEmpty (key))
		{
			throw new ArgumentException ("Cannot encrypt using an empty key. Please supply an encryption key.");
		}
		
		CspParameters cspp = new CspParameters ();
		cspp.KeyContainerName = key;
		
		RSACryptoServiceProvider rsa = new RSACryptoServiceProvider (cspp);
		rsa.PersistKeyInCsp = true;
		
		byte[] bytes = rsa.Encrypt (System.Text.UTF8Encoding.UTF8.GetBytes (stringToEncrypt), true);
		
		return BitConverter.ToString (bytes);
	}




	/// <summary>
	/// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
	/// </summary>
	/// <param name="stringToDecrypt">String that must be decrypted.</param>
	/// <param name="key">Decryptionkey.</param>
	/// <returns>The decrypted string or null if decryption failed.</returns>
	/// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
	public static string Decrypt (this string stringToDecrypt, string key)
	{
		string result = null;
		
		if (string.IsNullOrEmpty (stringToDecrypt))
		{
			throw new ArgumentException ("An empty string value cannot be encrypted.");
		}
		
		if (string.IsNullOrEmpty (key))
		{
			throw new ArgumentException ("Cannot decrypt using an empty key. Please supply a decryption key.");
		}
		
		try
		{
			CspParameters cspp = new CspParameters ();
			cspp.KeyContainerName = key;
			
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider (cspp);
			rsa.PersistKeyInCsp = true;
			
			string[] decryptArray = stringToDecrypt.Split (new string[] { "-" }, StringSplitOptions.None);
			byte[] decryptByteArray = Array.ConvertAll<string, byte> (decryptArray, (s => Convert.ToByte (byte.Parse (s, System.Globalization.NumberStyles.HexNumber))));
			
			
			byte[] bytes = rsa.Decrypt (decryptByteArray, true);
			
			result = System.Text.UTF8Encoding.UTF8.GetString (bytes);
			
		}
		finally
		{
			// no need for further processing
		}
		
		return result;
	}



	/// <summary>
	/// Converts string to enum object
	/// </summary>
	/// <typeparam name="T">Type of enum</typeparam>
	/// <param name="value">String value to convert</param>
	/// <returns>Returns enum object</returns>
	public static T ToEnum<T> (this string value)
		where T : struct
	{
		return (T)System.Enum.Parse (typeof(T), value, true);
	}

	/// <summary>
	/// Returns characters from right of specified length
	/// </summary>
	/// <param name="value">String value</param>
	/// <param name="length">Max number of charaters to return</param>
	/// <returns>Returns string from right</returns>
	public static string Right (this string value, int length)
	{
		return value != null && value.Length > length ? value.Substring (value.Length - length) : value;
	}

	/// <summary>
	/// Returns characters from left of specified length
	/// </summary>
	/// <param name="value">String value</param>
	/// <param name="length">Max number of charaters to return</param>
	/// <returns>Returns string from left</returns>
	public static string Left (this string value, int length)
	{
		return value != null && value.Length > length ? value.Substring (0, length) : value;
	}


    /// <summary>
    /// Supprime tous les espaces
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
	public static string RemoveWhitespace (this string str)
	{
		var len = str.Length;
		var src = str.ToCharArray ();
		int dstIdx = 0;
		for (int i = 0; i < len; i++)
		{
			var ch = src [i];
			switch (ch)
			{
			case '\u0020':
			case '\u00A0':
			case '\u1680':
			case '\u2000':
			case '\u2001': 
			case '\u2002':
			case '\u2003':
			case '\u2004':
			case '\u2005':
			case '\u2006': 
			case '\u2007':
			case '\u2008':
			case '\u2009':
			case '\u200A':
			case '\u202F': 
			case '\u205F':
			case '\u3000':
			case '\u2028':
			case '\u2029':
			case '\u0009': 
			case '\u000A':
			case '\u000B':
			case '\u000C':
			case '\u000D':
			case '\u0085':
				continue;
			default:
				src [dstIdx++] = ch;
				break;
			}
		}
		return new string (src, 0, dstIdx);
	}






	/// <summary>
	/// Converts a string to a boolean value if possible or throws an exception
	/// </summary>
	/// <returns><c>true</c>, if boolean was ased, <c>false</c> otherwise.</returns>
	/// <param name="value">Value.</param>
	 static bool IsBoolean (this string value)
	{
		var val = value.ToLower ().Trim ();
		switch (val)
		{
		case 	"false":
		case 	"f":
		case 	"no":
		case 	"n":
		case 	"true":
		case 	"t":
		case 	"yes":
		case 	"y":
			
			return true;


		}

		
		return false;
	}

    /// <summary>
    /// Transforme une string en boolean
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
	public static bool AsBoolean (this string value)
	{
		var val = value.ToLower ().Trim ();
		switch (val)
		{
		case 	"false":
		case 	"f":
		case 	"no":
		case 	"n":
			return false;

		case 	"true":
		case 	"t":
		case 	"yes":
		case 	"y":
			
			return true;
			
			
		}
		throw new ArgumentException ("Value is not a boolean value.");
	}











	/// <summary>
	/// Rajoute ... a la place d'un texte a parti d'un nombre de lettre
	/// </summary>
	/// <param name="text">string that will be truncated</param>
	/// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
	/// <returns>truncated string</returns>
	/// 
	/// 
	/// example : string newText = "this is the palce i want to be, Cindys is the place to be!";
	/// Console.WriteLine("New Text: {0}", newText.Truncate(40));
	/// 
	/// 
	public static string Truncate (this string text, int maxLength)
	{
		// replaces the truncated string to a ...
		const string suffix = "...";
		string truncatedString = text;
		
		if (maxLength <= 0)
			return truncatedString;
		int strLength = maxLength - suffix.Length;
		
		if (strLength <= 0)
			return truncatedString;
		
		if (text == null || text.Length <= maxLength)
			return truncatedString;
		
		truncatedString = text.Substring (0, strLength);
		truncatedString = truncatedString.TrimEnd ();
		truncatedString += suffix;
		return truncatedString;
	}




	/// <summary>
	/// Determines if is numeric the specified theValue.
	/// </summary>
	/// <returns><c>true</c> if is numeric the specified theValue; otherwise, <c>false</c>.</returns>
	/// <param name="theValue">The value.</param>
	/// 
	/// Example:
	/// 		string value = "abc";
	/// bool isnumeric = value.IsNumeric();// Will return false;
	/// value = "11";
	/// isnumeric = value.IsNumeric();// Will return true;
	/// 
	public static bool IsNumeric (this string theValue)
	{
		long retNum;
		return long.TryParse (theValue, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
	}




	/// <summary>
	/// Compresses the string.
	/// if you want to lost wight of string , you can use gzip
	/// </summary>
	/// <returns>The string.</returns>
	/// <param name="text">Text.</param>
	/// 
	/// string zipstring = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".CompressString();
	/// string unzipstring = zipstring.DecompressString();
	/// 
	public static string CompressString (this string text)
	{
		byte[] buffer = Encoding.UTF8.GetBytes (text);
		var memoryStream = new MemoryStream ();
		using (var gZipStream = new GZipStream (memoryStream, CompressionMode.Compress, true))
		{
			gZipStream.Write (buffer, 0, buffer.Length);
		}
		
		memoryStream.Position = 0;
		
		var compressedData = new byte[memoryStream.Length];
		memoryStream.Read (compressedData, 0, compressedData.Length);
		
		var gZipBuffer = new byte[compressedData.Length + 4];
		Buffer.BlockCopy (compressedData, 0, gZipBuffer, 4, compressedData.Length);
		Buffer.BlockCopy (BitConverter.GetBytes (buffer.Length), 0, gZipBuffer, 0, 4);
		return Convert.ToBase64String (gZipBuffer);
	}

	public static string DecompressString (string compressedText)
	{
		byte[] gZipBuffer = Convert.FromBase64String (compressedText);
		using (var memoryStream = new MemoryStream ())
		{
			int dataLength = BitConverter.ToInt32 (gZipBuffer, 0);
			memoryStream.Write (gZipBuffer, 4, gZipBuffer.Length - 4);
			
			var buffer = new byte[dataLength];
			
			memoryStream.Position = 0;
			using (var gZipStream = new GZipStream (memoryStream, CompressionMode.Decompress))
			{
				gZipStream.Read (buffer, 0, buffer.Length);
			}
			
			return Encoding.UTF8.GetString (buffer);
		}
	}








	/// <summary>
	/// Returns the remaining characters in a target string, 
	/// starting from a search string. 
	/// If the search string is not found in the target, it returns the full target string.
	/// </summary>
	/// <param name="s">The string to search.</param>
	/// <param name="searchFor">The string to search for.</param>
	/// <returns></returns>
	/// 
	/// Example : 
	/// string s = "abcde";
	/// Console.WriteLine (s.TakeFrom("d"));   // "de"
	/// 
	/// 
	public static string TakeFrom (this string s, string searchFor)
	{
		if (s.Contains (searchFor))
		{
			int length = Math.Max (s.Length, 0);
			
			int index = s.IndexOf (searchFor);
			
			return s.Substring (index, length - index);
		}
		
		return s;
	}

	
	
	
	
	/// <summary>
	/// Determines whether two String objects have the same value. 
	/// Null and String.Empty are considered equal values.
	/// </summary>
	/// <returns><c>true</c>, if by value was equalsed, <c>false</c> otherwise.</returns>
	/// <param name="inString">In string.</param>
	/// <param name="compared">Compared.</param>
	/// 
	/// bool areEqual = a.EqualsByValue(b)
	/// 
	public static bool EqualsByValue (this string inString, string compared)
	{
		if (string.IsNullOrEmpty (inString) && string.IsNullOrEmpty (compared))
			return true;
		
		// If we get here, then "compared" necessarily contains data and therefore, strings are not equal.
		if (inString == null)
			return false;
		
		// Turn down to standard equality check.
		return inString.Equals (compared);
	}

	
	/// <summary>
	/// Checks if a string contains no spaces
	/// </summary>
	/// <returns><c>true</c>, if no spaces was containsed, <c>false</c> otherwise.</returns>
	/// <param name="s">S.</param>
	/// 
	/// 	if (!textBoxUserIdNew.Text.Trim().ContainsNoSpaces())
	/// 
	public static bool ContainsNoSpaces (this string s)
	{
		var regex = new Regex (@"^[a-zA-Z0-9]+$");
		return regex.IsMatch (s);
	}


	public static string Reverse (this string s)
	{
		char[] c = s.ToCharArray ();
		Array.Reverse (c);
		return new string (c);
	}

	public static string ReverseWords (this string text)
	{
		string[] wordsList = text.Split (new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Reverse ().ToArray ();
		return string.Join (" ", wordsList);
	}



	// <summary>
	/// Parses a string into an Enum
	/// </summary>
	/// <typeparam name="T">The type of the Enum</typeparam>
	/// <param name="value">String value to parse</param>
	/// <returns>The Enum corresponding to the stringExtensions</returns>
	/// 
	/// Useful to parse a string into an Enum.
	/// 
	/// public enum TestEnum
	/// {
	/// 	Bar,
	/// 	Test
	/// }
	/// 
	/// public class Test
	/// {
	/// 		public void Test()
	/// 		{
	///             TestEnum foo = "Test".EnumParse<TestEnum>();
	/// 		}
	/// }
	public static T EnumParse<T> (this string value)
	{
		return StringExtensions.EnumParse<T> (value, false);
	}

	public static T EnumParse<T> (this string value, bool ignorecase)
	{

		if (value == null)
		{
			throw new ArgumentNullException ("value");
		}

		value = value.Trim ();

		if (value.Length == 0)
		{
			throw new ArgumentException ("Must specify valid information for parsing in the string.", "value");
		}

		Type t = typeof(T);

		if (!t.IsEnum)
		{
			throw new ArgumentException ("Type provided must be an Enum.", "T");
		}

		return (T)Enum.Parse (t, value, ignorecase);
	}




	///	And here is the extension method to do this simply
	//


	/// <summary>
	/// Match the specified value and pattern.
	/// This extension method is for pattern matching in any string using Regex. This is how you generally use it
	///
	///	Regex regex = new Regex("[0-9]");
	///	if (regex.IsMatch(myData))
	///	{
	///		// do someting
	///	}
	/// 
	/// After this look at the code below. It became much simpler than the traditional approach:
	/// 
	/// if (myData.Match("[0-9]"))
	/// {
	/// 	// do someting
	/// }
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="pattern">Pattern.</param>
	public static bool Match (this string value, string pattern)
	{
		Regex regex = new Regex (pattern);
		return regex.IsMatch (value);
	}
        

	public static bool IsNullOrWhiteSpace (this string s)
	{
		foreach (char c in s)
		{
			if (c != ' ')
				return false;
		}
		return true;

	}  
    
	/// <summary>
	/// More meaningful way to read if statements (if string.hasValue) rather than (if not string.IsNullOr*)
	/// </summary>
	/// <param name="checkWhiteSpace">Include whitespace check?</param>
	/// <returns>Returns opposite of string.IsNullOr*()</returns>
	public static bool HasValue (this string s, bool checkWhiteSpace = true)
	{
		return  checkWhiteSpace ? !s.IsNullOrWhiteSpace () : !string.IsNullOrEmpty (s);
	}





	// Named format strings from object attributes. Eg:
	// string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
	// From: http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791
	public static string ToString (this object anObject, string aFormat)
	{
		return ToString (anObject, aFormat, null);
	}

	public static string ToString (this object anObject, string aFormat, IFormatProvider formatProvider)
	{
		StringBuilder sb = new StringBuilder ();
		Type type = anObject.GetType ();
		Regex reg = new Regex (@"({)([^}]+)(})", RegexOptions.IgnoreCase);
		MatchCollection mc = reg.Matches (aFormat);
		int startIndex = 0;
		foreach (Match m in mc)
		{
			Group g = m.Groups [2]; //it's second in the match between { and }
			int length = g.Index - startIndex - 1;
			sb.Append (aFormat.Substring (startIndex, length));

			string toGet = string.Empty;
			string toFormat = string.Empty;
			int formatIndex = g.Value.IndexOf (":"); //formatting would be to the right of a :
			if (formatIndex == -1) //no formatting, no worries
			{
				toGet = g.Value;
			}
			else //pickup the formatting
			{
				toGet = g.Value.Substring (0, formatIndex);
				toFormat = g.Value.Substring (formatIndex + 1);
			}

			//first try properties
			PropertyInfo retrievedProperty = type.GetProperty (toGet);
			Type retrievedType = null;
			object retrievedObject = null;
			if (retrievedProperty != null)
			{
				retrievedType = retrievedProperty.PropertyType;
				retrievedObject = retrievedProperty.GetValue (anObject, null);
			}
			else //try fields
			{
				FieldInfo retrievedField = type.GetField (toGet);
				if (retrievedField != null)
				{
					retrievedType = retrievedField.FieldType;
					retrievedObject = retrievedField.GetValue (anObject);
				}
			}

			if (retrievedType != null) //Cool, we found something
			{
				string result = string.Empty;
				if (toFormat == string.Empty) //no format info
				{
					result = retrievedType.InvokeMember ("ToString",
						BindingFlags.Public | BindingFlags.NonPublic |
						BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
						, null, retrievedObject, null) as string;
				}
				else //format info
				{
					result = retrievedType.InvokeMember ("ToString",
						BindingFlags.Public | BindingFlags.NonPublic |
						BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
						, null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
				}
				sb.Append (result);
			}
			else //didn't find a property with that name, so be gracious and put it back
			{
				sb.Append ("{");
				sb.Append (g.Value);
				sb.Append ("}");
			}
			startIndex = g.Index + g.Length + 1;
		}
		if (startIndex < aFormat.Length) //include the rest (end) of the string
		{
			sb.Append (aFormat.Substring (startIndex));
		}
		return sb.ToString ();
	}



	public static bool IsSame (this string s, string otherS)
	{
		return string.Equals (s, otherS, System.StringComparison.CurrentCultureIgnoreCase);
	}

	public static bool IdCompare (this string s, string otherS)
	{
		int hash1 = Animator.StringToHash (s);
		int hash2 = Animator.StringToHash (otherS);
		return hash1 == hash2;

	}


	/// <summary>
	/// Insert the specified character into the string every n characters
	/// </summary>
	/// <param name="input"></param>
	/// <param name="insertCharacter"></param>
	/// <param name="n"></param>
	/// <param name="charsInserted"></param>
	/// <returns></returns>
	public static string InsertCharEveryNChars (this string input, char insertCharacter, int n, out int charsInserted)
	{
		charsInserted = 0;
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < input.Length; i++)
		{
			if (i % n == 0)
			{
				sb.Append (insertCharacter);
				++charsInserted;
			}
			if (input [i] == insertCharacter)
				++charsInserted;
			sb.Append (input [i]);
		}
		return sb.ToString ();
	}
}

