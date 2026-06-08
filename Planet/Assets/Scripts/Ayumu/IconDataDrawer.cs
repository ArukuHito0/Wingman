//using UnityEditor;
//using UnityEngine;

//[CustomPropertyDrawer(typeof(IconData))]
//public class IconDataDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        // プロパティの開始を明示
//        EditorGUI.BeginProperty(position, label, property);

//        // 各子プロパティの取得
//        SerializedProperty nameProp = property.FindPropertyRelative("iconName");
//        SerializedProperty spriteProp = property.FindPropertyRelative("iconSprite");

//        float originalTotalLabelWidth = EditorGUIUtility.labelWidth;
//        EditorGUIUtility.labelWidth = 70f;

//        // ラベルを表示し、残りの領域を計算
//        position = EditorGUI.PrefixLabel(position, label);

//        // 元のインデントを退避させて一時的に0にする※横並びの崩れを防ぐ
//        int indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;

//        // 全体の幅を半分ずつに分ける
//        float halfWidth = (position.width - 5f) / 2f;

//        float internalLabelWidth = 40f;

//        // IconNameの配置
//        Rect nameRect = new Rect(position.x, position.y, halfWidth, position.height);

//        // ラベルの幅を固定して、入力欄が潰れないようにする
//        float originalLabelWidth = EditorGUIUtility.labelWidth;
//        EditorGUIUtility.labelWidth = internalLabelWidth;
//        EditorGUI.PropertyField(nameRect, nameProp, new GUIContent("Name"));

//        // IconSpriteの配置
//        Rect spriteRect = new Rect(position.x + halfWidth + 5f, position.y, halfWidth, position.height);
//        EditorGUIUtility.labelWidth = internalLabelWidth;
//        EditorGUI.PropertyField(spriteRect, spriteProp, new GUIContent("Sprite"));

//        // 元の設定に戻す
//        EditorGUIUtility.labelWidth = originalTotalLabelWidth;
//        EditorGUI.indentLevel = indent;

//        EditorGUI.EndProperty();
//    }
//}
