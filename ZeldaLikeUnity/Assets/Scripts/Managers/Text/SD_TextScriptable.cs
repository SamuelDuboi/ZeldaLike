using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CreateAssetMenu(menuName ="Text")]
public class SD_TextScriptable : ScriptableObject
{
    public Sprite ImageCharacter;
    [TextArea]
    public string text;
    public enum character { alyah1,alyah2,Henry1,Henry2,WindMother,Pepe,Note }
    public character pnj;
    //to make the creation prettier
    [CustomPropertyDrawer(typeof(Sprite))]
    public class SpriteDrawer : PropertyDrawer
    {

        private static GUIStyle s_TempStyle = new GUIStyle();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            var ident = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect spriteRect;
            //find the sprite and see if it isn't null
            Sprite character = property.objectReferenceValue as Sprite;
            if (character == null)
                spriteRect = new Rect(position.x, position.y, position.width, 100);
            else
                spriteRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.objectReferenceValue = EditorGUI.ObjectField(spriteRect, property.name, property.objectReferenceValue, typeof(Sprite), false);

            //if this is not a repain or the property is null exit now
            if (Event.current.type != EventType.Repaint || property.objectReferenceValue == null)
                return;

            //draw the sprite
            spriteRect.width = character.bounds.size.x * 50;
            spriteRect.height = character.bounds.size.y * 50;
            spriteRect.y += EditorGUIUtility.singleLineHeight + 4;
            spriteRect.x = (EditorGUIUtility.currentViewWidth - spriteRect.width) * 0.5f;

            s_TempStyle.normal.background = character.texture;
            s_TempStyle.Draw(spriteRect, GUIContent.none, false, false, false, false);

            EditorGUI.indentLevel = ident;


        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + 70f;
        }
    }

}
