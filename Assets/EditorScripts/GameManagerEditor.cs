using Assets.Scripts.UI;
using UnityEditor;
using UnityEngine;

namespace Assets.EditorScripts
{
    [CustomEditor(typeof(GameManager))]
    [CanEditMultipleObjects]
    public class GameManagerEditor : Editor
    {
        public GameManager GameManagerScript;
        public SerializedProperty InputKeys;

        public string KeyName;
        public string DefaultValue;

        private readonly GUIStyle style;
        private void OnEnable()
        {
            InputKeys = serializedObject.FindProperty("InputKeys");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            GameManagerScript = (GameManager)target;

            //KeyName = TextField(KeyName, "Key name: ");
            KeyName = EditorGUILayout.TextField("Key name: ", KeyName);
            //DefaultValue = TextField(DefaultValue, "Default value: ");
            DefaultValue = EditorGUILayout.TextField("Default value: ", DefaultValue);
            InputKeysButton(KeyName, DefaultValue);

            serializedObject.ApplyModifiedProperties();
        }

        private string TextField(string filed, string label)
        {
            filed = EditorGUILayout.TextField(label, Selection.activeGameObject.name);
            this.Repaint();
            return filed;
        }

        private void InputKeysButton(string keyName, string defaultValue)
        {
            if (GUILayout.Button("Add new Input Keys"))
            {
                GameManagerScript.AddInputKey(keyName, defaultValue);
            }
        }

    }
}

