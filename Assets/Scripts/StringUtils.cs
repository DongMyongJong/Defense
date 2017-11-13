using UnityEngine;
using System.Collections;

public class StringUtils {
	public static string pascalCasing(string src) {
		string first = src.Substring (0, 1);
		return first.ToUpper () + src.Substring (1);
	}
	public static string camelCasing(string src) {
		string first = src.Substring (0, 1);
		return first.ToLower() + src.Substring (1);
	}
}
