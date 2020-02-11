#region System & Unity
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#endregion

public class Renamer : EditorWindow
{
	/*
		Ce script permet de renommer plusieurs objets en une seule fois.

		That script allows multi object name editing.
	*/

	#region Private Variables

	private int index = 0;
	private int offset;

	private string newName = "Enter new name";
	private string spacingString = " ";

	private bool useIndex = true;
	private bool orderIndex = true;
	private bool resetAtEnd = false;
	private bool useSpacingString = true;
	private bool setSpacingString = false;

	private GameObject[] selectionGOs;

	#endregion


	[MenuItem ("Tools/GameObjects/Renamer")]
	#region Private Static Methods
	private static void Init ()
	{
		Renamer window = (Renamer)GetWindow (typeof(Renamer), false, "Renamer");
		window.maxSize = new Vector2 (250, 165);
		window.minSize = window.maxSize;
		window.Show ();
	}

	#endregion

	#region Private Methods

	private void RenameSelection ()
	{
		Debug.Log ("Start renaming...");

		if (selectionGOs.Length == 0 || selectionGOs == null) {
			Debug.LogWarning ("There is no selected object to rename!");
			return;
		}

		foreach (GameObject obj in selectionGOs) {
			if (useIndex) {
				if (useSpacingString) {
					if (orderIndex) {						
						obj.name = newName + spacingString + (Array.IndexOf (selectionGOs, obj) + index).ToString ();
					} else
						obj.name = newName + spacingString + index.ToString ();
				} else {
					if (orderIndex)
						obj.name = newName + index.ToString ();
					else
						obj.name = newName + (Array.IndexOf (selectionGOs, obj) + index).ToString ();
				}

				if (!orderIndex)
					index++;				
			} else
				obj.name = newName;
		}
		if (resetAtEnd)
			index = 0;
		Debug.Log ("Renaming done.");
	}

	#endregion

	#region Methods

	void Awake ()
	{
		selectionGOs = Selection.gameObjects;

		int[] tempInt = new int[selectionGOs.Length];
		for (int i = 0; i < selectionGOs.Length; i++) {
			tempInt [i] = selectionGOs [i].transform.GetSiblingIndex ();
		}
		Array.Sort (tempInt, selectionGOs);
	}

	void OnGUI ()
	{
		#region Name
		GUILayout.Label ("New name", EditorStyles.boldLabel);
		newName = GUILayout.TextField (newName);
		#endregion

		GUILayout.Space (5);

		#region Options Toggle
		GUILayout.BeginHorizontal ();
		useIndex = GUILayout.Toggle (useIndex, "  Add index");
		if (useIndex) {
			useSpacingString = GUILayout.Toggle (useSpacingString, "  Use a spacing string");
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		if (useIndex)
			orderIndex = GUILayout.Toggle (orderIndex, "  Order by index");
		if (useSpacingString)
			setSpacingString = GUILayout.Toggle (setSpacingString, "  Use custom string");
		GUILayout.EndHorizontal ();
		#endregion

		GUILayout.Space (5);

		#region Options Fields
		if (useIndex) {
			EditorGUIUtility.labelWidth = 90;
			GUILayout.BeginHorizontal ();
			index = EditorGUILayout.IntField ("Start index", index, GUILayout.MaxWidth (110));
			GUILayout.Space (5);
			resetAtEnd = GUILayout.Toggle (resetAtEnd, "  Reset when done");
			GUILayout.EndHorizontal ();
		}
		GUILayout.Space (5);
		if (setSpacingString && useSpacingString) {
			EditorGUIUtility.labelWidth = 90;
			spacingString = EditorGUILayout.TextField ("Spacing string", spacingString);
		} else {
			spacingString = " ";
		}
		#endregion

		GUILayout.FlexibleSpace ();

		#region Button
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUI.contentColor = new Color (.6f, 1, 0);
		if (GUILayout.Button ("Rename", GUILayout.MaxWidth (150)))
			RenameSelection ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		#endregion

		GUILayout.Space (10);
	}

	void OnSelectionChange ()
	{
		selectionGOs = Selection.gameObjects;

		int[] tempInt = new int[selectionGOs.Length];
		for (int i = 0; i < selectionGOs.Length; i++) {
			tempInt [i] = selectionGOs [i].transform.GetSiblingIndex ();
		}
		Array.Sort (tempInt, selectionGOs);
	}

	void Update ()
	{
		Repaint ();
	}

	#endregion
}