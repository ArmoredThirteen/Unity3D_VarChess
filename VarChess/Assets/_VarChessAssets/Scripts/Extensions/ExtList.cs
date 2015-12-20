using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ExtList
{
	public static void DebugLogAll<T> (this List<T> theList)
	{
		string log = "";

		theList.Count.LoopsOf (i =>
			log += theList[i].ToString () + "\r\n"
			);

		if (log == "")
			log = "Empty List";

		Debug.Log (log);
	}
}
