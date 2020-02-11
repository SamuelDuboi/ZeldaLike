#region System & Unity
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Collections;

#endregion

public class ColorChanger : EditorWindow
{

	/*
		Ce script permet de changer la couleur de plusieurs images ou de plusieurs materiaux en une seule fois.

		That script allows multi Image color or multi Material color editing.
	*/

	#region Private Variables

	private bool imageComponent = false;
	private bool randomize = false;

	private Color newColor = Color.white;

	private Color[] newColors = new Color[5];

	private GameObject[] selectionGOs;

	#endregion

	[MenuItem ("Tools/GameObjects/Color Changer")]
	#region Private Static Methods
	private static void Init ()
	{
		ColorChanger window = (ColorChanger)GetWindow (typeof(ColorChanger), false, "Color Changer");
		window.maxSize = new Vector2 (250, 165);
		window.minSize = window.maxSize;
		window.Show ();
	}

	#endregion

	#region Private Methods

	private void ChangeColor ()
	{
		Debug.Log ("Start changing color...");

		if (selectionGOs.Length == 0 || selectionGOs == null) {
			Debug.LogWarning ("There is no selected object to change color!");
			return;
		}

		foreach (GameObject obj in selectionGOs) {
			if (randomize) {
				if (imageComponent)
					obj.GetComponent<Image> ().color = newColors [UnityEngine.Random.Range (0, newColors.Length)];
				else {
					Material tempMat;
					tempMat = new Material (obj.GetComponent<Renderer> ().sharedMaterial);
					tempMat.color = newColors [UnityEngine.Random.Range (0, newColors.Length)];
					obj.GetComponent<Renderer> ().material = tempMat;
				}
			} else {
				if (imageComponent)
					obj.GetComponent<Image> ().color = newColor;
				else {
					Material tempMat;
					tempMat = new Material (obj.GetComponent<Renderer> ().sharedMaterial);
					tempMat.color = newColor;
					obj.GetComponent<Renderer> ().material = tempMat;
				}
			}
		}

		Debug.Log ("Color change done.");
	}

	#endregion

	#region Methods

	void Awake ()
	{
		selectionGOs = Selection.gameObjects;
	}

	void OnGUI ()
	{
		#region Option Toggle
		imageComponent = GUILayout.Toggle (imageComponent, "Change Image component color");
		if (imageComponent) {
			GUI.contentColor = new Color (1, .6f, 0);
			GUILayout.Label ("Please make sure there is an Image\ncomponent on object!");
			GUI.contentColor = Color.white;
		}
		GUILayout.Space (5);
		randomize = GUILayout.Toggle (randomize, "Chose random color");
		#endregion

		GUILayout.Space (5);

		#region Color Fields
		GUILayout.Label ("New color", EditorStyles.boldLabel);
		if (randomize) {
			GUILayout.BeginHorizontal ();
			for (int i = 0; i < newColors.Length; i++) {
				newColors [i] = EditorGUILayout.ColorField (newColors [i], GUILayout.MaxWidth (45));
			}
			GUILayout.EndHorizontal ();
		} else {
			for (int i = 0; i < newColors.Length; i++) {
				newColors [i] = Color.white;
			}
			newColor = EditorGUILayout.ColorField (newColor);
		}
		#endregion

		GUILayout.FlexibleSpace ();

		#region Button
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUI.contentColor = new Color (.6f, 1, 0);
		if (GUILayout.Button ("Change color", GUILayout.MaxWidth (150)))
			ChangeColor ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		#endregion

		GUILayout.Space (10);
	}

	void OnSelectionChange ()
	{
		selectionGOs = Selection.gameObjects;
	}

	void Update ()
	{
		Repaint ();
	}

	#endregion
}
