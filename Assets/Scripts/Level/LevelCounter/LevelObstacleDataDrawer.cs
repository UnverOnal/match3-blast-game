// using GamePlay.CellManagement;
// using UnityEditor;
// using UnityEngine;
//
// namespace Level.LevelCounter
// {
//     [CustomPropertyDrawer(typeof(LevelObstacleData))]
//     public class LevelObstacleDataDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             // EditorGUI.BeginProperty(position, label, property);
//             //
//             // var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//             // var creationTypeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
//             // var locationRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2, position.width, EditorGUIUtility.singleLineHeight);
//             // var amountRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3, position.width, EditorGUIUtility.singleLineHeight);
//
//             // EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("cellType"));
//             // EditorGUI.PropertyField(creationTypeRect, property.FindPropertyRelative("creationType"));
//             // EditorGUI.PropertyField(locationRect, property.FindPropertyRelative("boardLocation"));
//             // EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"));
//             
//             // if (property.FindPropertyRelative("creationType").enumValueIndex != (int)CreationType.Random)
//             // {
//             //     EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"));
//             // }
//             //
//             // EditorGUI.EndProperty();
//         }
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
//         }
//     }
// }