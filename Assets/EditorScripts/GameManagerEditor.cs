using Assets.Scripts.UI;
using UnityEditor;
using UnityEngine;

namespace Assets.EditorScripts
{
    [CustomEditor(typeof(GameManager))]
    [CanEditMultipleObjects]
    public class GameManagerEditor : Editor
    {
        [SerializeField] private readonly string keyName;
        [SerializeField] private readonly string defaultInputValue;

        private SerializedProperty inputKeys;

        private void OnEnable()
        {
            // Setup the SerializedProperties.
            inputKeys = serializedObject.FindProperty("InputKeys");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager myScript = (GameManager)target;
            if (GUILayout.Button("Add InputKey"))
            {
                //     myScript.AddInputKey(keyName, defaultInputValue);
            }
        }
    }
}
