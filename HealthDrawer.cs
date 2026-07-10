#if UNITY_EDITOR
using Drafts.Rpg;
using UnityEditor;
using UnityEngine;

namespace Skydeck.Editor
{
    [CustomPropertyDrawer(typeof(Health), true)]
    public class HealthDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var curr = property.FindPropertyRelative("<Current>k__BackingField");
            var max = property.FindPropertyRelative("<Max>k__BackingField");
            var text = new GUIContent($"{curr.intValue} / {max.intValue}");
            var rect = EditorGUI.PrefixLabel(position, label);

            var width = rect.width / 2 - 12;
            rect.width = width;
            EditorGUI.PropertyField(rect, curr, GUIContent.none);

            rect.x += rect.width;
            rect.width = 12;
            EditorGUI.LabelField(rect, " /");

            rect.x += rect.width;
            rect.width = width;
            EditorGUI.PropertyField(rect, max, GUIContent.none);
        }
    }
}
#endif