using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// インスペクターで編集不可にするための属性
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // ここで編集を無効化
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;  // 元に戻す
    }
}
#endif