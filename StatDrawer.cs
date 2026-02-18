#if UNITY_EDITOR
using Drafts.Rpg;
using UnityEditor;
using UnityEngine;

namespace Skydeck.Editor
{
    [CustomPropertyDrawer(typeof(Stat))]
    public class StatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var total = property.FindPropertyRelative("<Total>k__BackingField");
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, total, label);
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif