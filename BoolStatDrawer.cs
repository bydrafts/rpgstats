using Drafts.Rpg;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DefaultNamespace
{
    [CustomPropertyDrawer(typeof(BoolStat))]
    public class BoolStatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var count = property.FindPropertyRelative("count");
            position = EditorGUI.PrefixLabel(position, label);
            
            var width = position.width;
            position.width = 40;
            EditorGUI.LabelField(position, (count.intValue > 0).ToString());

            position.x += 40;
            position.width = width - 40;
            EditorGUI.PropertyField(position, count, GUIContent.none);
        }
    }
}
#endif