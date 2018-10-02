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

        // private readonly string keyName;
        // private readonly string defaultValue;

        private readonly Selection keyNameSelection;
        private readonly Selection defaultValueSelection;

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

            TextField("Key name: ");
            TextField("Default value: ");
            InputKeysButton("sdfsdf","asdfasdf");

            serializedObject.ApplyModifiedProperties();
        }

        private void TextField(string label)
        {
            if (!Selection.activeGameObject)
            {
                return;
            }
            Selection.activeGameObject.name = EditorGUILayout.TextField(label, Selection.activeGameObject.name);
            this.Repaint();
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

