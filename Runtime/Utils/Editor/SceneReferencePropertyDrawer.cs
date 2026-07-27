using UnityEngine;

#if UNITY_EDITOR
using LuckiusDev.Utils.Data;
using UnityEditor;
#endif

namespace LuckiusDev.Utils.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty sceneAsset = property.FindPropertyRelative("m_sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("m_sceneName");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Draw the object field
            EditorGUI.BeginChangeCheck();
            SceneAsset newAsset = (SceneAsset)EditorGUI.ObjectField(
                position,
                sceneAsset.objectReferenceValue,
                typeof(SceneAsset),
                false
            );
            if (EditorGUI.EndChangeCheck())
            {
                sceneAsset.objectReferenceValue = newAsset;
                sceneName.stringValue = newAsset != null ? newAsset.name : string.Empty;
            }

            // Recovery logic: if asset is missing but name is stored, try to find it.
            if (sceneAsset.objectReferenceValue == null && !string.IsNullOrEmpty(sceneName.stringValue))
            {
                string targetName = sceneName.stringValue;
                string[] guids = AssetDatabase.FindAssets("t:SceneAsset");
                SceneAsset foundAsset = null;

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    if (asset != null && asset.name == targetName)
                    {
                        if (foundAsset == null)
                        {
                            foundAsset = asset;
                        }
                        else
                        {
                            Debug.LogWarning(
                                $"Multiple SceneAssets found with name '{targetName}'. " +
                                "Cannot automatically recover the reference."
                            );
                            foundAsset = null;
                            break;
                        }
                    }
                }

                if (foundAsset != null)
                {
                    sceneAsset.objectReferenceValue = foundAsset;
                }
                else
                {
                    Debug.LogWarning(
                        $"SceneAsset '{targetName}' not found. Clearing the reference."
                    );
                    sceneName.stringValue = string.Empty;
                }
            }

            EditorGUI.EndProperty();
        }
    }
#endif
}