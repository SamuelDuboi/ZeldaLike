
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterWindow : EditorWindow
{
#if Unity_Editor
    string characterName;
    Sprite characterSprite;
    Vector2 scroll;
    string introduction;
    bool option;
    int colorListNumber;
    List<Color> color = new List<Color>();
    List<string> nameForColor = new List<string>();
    List<bool> boolForColor = new List<bool>();
    int toglePositon;

    [MenuItem("Tool/CharacterWindow")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        CharacterWindow newCharacterWindow = (CharacterWindow)EditorWindow.GetWindow(typeof(CharacterWindow));
        Undo.RecordObject(newCharacterWindow, "window");
        newCharacterWindow.Show();
    }
    private void OnGUI()
    {

        // Creat the different characteristics of the new character.
        GUILayout.Label("New Character", EditorStyles.boldLabel);
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        EditorGUILayout.LabelField("Text Name", EditorStyles.boldLabel);
        characterName = EditorGUILayout.TextField(characterName);

        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        // Creat a texte area for the character description and add a scroll abr if needed.
        EditorGUILayout.LabelField("Character Introduction", EditorStyles.boldLabel);
        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Width(EditorGUIUtility.currentViewWidth - 10), GUILayout.Height(100));
        introduction = EditorGUILayout.TextArea(introduction, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
        GUILayout.Space(EditorGUIUtility.singleLineHeight);

        // Show the sprite, if the window is too small, only show the name of the sprite.
        if (EditorGUIUtility.currentViewWidth > 230)
        {
            toglePositon = 340;
            characterSprite = EditorGUILayout.ObjectField("Character Sprite", characterSprite, typeof(Sprite), true) as Sprite;
        }
        else
        {
            toglePositon = 315;
            EditorGUILayout.LabelField("Character Sprite", EditorStyles.boldLabel);
            characterSprite = EditorGUILayout.ObjectField(characterSprite, typeof(Sprite), true) as Sprite;
        }
        GUILayout.Space(EditorGUIUtility.singleLineHeight);


        //Set the optional settings, on able, the user can chose to change the appearance of some parts of the text.
        option = EditorGUILayout.BeginToggleGroup("Optional Settings", option);
        //Reset every list if the option toggle is false.
        if (!option)
        {
            nameForColor.Clear();
            boolForColor.Clear();
            color.Clear();
            colorListNumber = 0;

        }
        //Button to increment or decrement the nomber of words changed.
        if (GUI.Button(new Rect(EditorGUIUtility.currentViewWidth / 8, toglePositon, EditorGUIUtility.currentViewWidth / 8, 30), "+"))
        {
            colorListNumber++;
        }
        if (GUI.Button(new Rect(0, toglePositon, EditorGUIUtility.currentViewWidth / 8, 30), "-") && colorListNumber > 0)
        {
            colorListNumber--;
        }

        GUILayout.Space(EditorGUIUtility.singleLineHeight * 2);
        Rect r = EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Color", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.35f));
        EditorGUILayout.LabelField("Words", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.35f));
        EditorGUILayout.LabelField("Bold", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.3f));
        EditorGUILayout.EndHorizontal();

        //Manage the lists according colorListNumber. 
        if (colorListNumber > color.Count)
        {
            color.Add(Color.black);
            nameForColor.Add("New Word");
            boolForColor.Add(false);
        }
        else if (colorListNumber < color.Count && colorListNumber > 0)
        {
            color.RemoveAt(color.Count - 1);
            nameForColor.RemoveAt(color.Count - 1);
            boolForColor.RemoveAt(color.Count - 1);
        }
        //Creat a layout for each memeber of the list.
        for (int i = 0; i < colorListNumber; i++)
        {
            Rect rect = EditorGUILayout.BeginHorizontal();
            color[i] = EditorGUILayout.ColorField(color[i]);
            nameForColor[i] = EditorGUILayout.TextField(nameForColor[i]);
            boolForColor[i] = EditorGUILayout.Toggle(boolForColor[i]);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndToggleGroup();

        string buttontext;
        if (EditorGUIUtility.currentViewWidth > 300)
            buttontext = "Create new character";
        else
            buttontext = " Create";

        //On the button click, save the character in the folder CharacterFolder.
        if (GUI.Button(new Rect(EditorGUIUtility.currentViewWidth / 4, 600, EditorGUIUtility.currentViewWidth / 2, 30), buttontext))
        {
            //Change the value of the word selected in the optional settings.
            for (int i = 0; i < colorListNumber; i++)
            {
                string newvalue = "";
                if (nameForColor[i] != "New Words")
                {
                    if (!boolForColor[i])
                    {
                        newvalue = "<color=#" + ColorUtility.ToHtmlStringRGB(color[i]) + ">" + nameForColor[i] + "</color>";
                    }
                    else
                    {
                        newvalue = "<color=#" + ColorUtility.ToHtmlStringRGB(color[i]) + "><b>" + nameForColor[i] + "</b></color>";
                    }

                    if (introduction.Contains(nameForColor[i]) && !introduction.Contains(newvalue))
                    {
                        introduction = introduction.Replace(nameForColor[i], newvalue);
                        Debug.Log(introduction);
                    }
                }
            }
            //Creat the save.
            SD_TextScriptable newcharacter = CreateInstance<SD_TextScriptable>();
            newcharacter.text = introduction;
            newcharacter.ImageCharacter = characterSprite;
            string path;
            //if the character doesn't have a name, creat a default asset.
            if (characterName != "")
            {
                path = "Assets/Dialogue/" + characterName + ".asset";
            }
            else
            {
                path = "Assets/Dialogue/" + "EmptyText" + ".asset";
            }

            AssetDatabase.CreateAsset(newcharacter, path);
            AssetDatabase.SaveAssets();
            //reset the window after the save
            characterName = "";
            introduction = "";
            characterSprite = null;
            option = false;

        }

    }
#endif
}

