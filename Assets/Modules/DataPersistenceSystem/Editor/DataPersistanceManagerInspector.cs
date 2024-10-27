using UnityEditor;
using UnityEngine;

namespace DataPersistance
{
    [CustomEditor(typeof(DataPersistanceManager))]
    public class DataPersistanceManagerInspector : Editor
    {
        private DataPersistanceManager Target => target as DataPersistanceManager;
        private Vector2 _scrollPosition;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Save"))
            {
                Target.Save();
            }
            if (GUILayout.Button("Load"))
            {
                Target.Load();
            }

            GUILayout.BeginHorizontal();
            Target.CurrentPath = GUILayout.TextArea(Target.CurrentPath, GUILayout.Height(32));
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_FolderEmpty On Icon"), GUILayout.Width(32), GUILayout.Height(32)))
            {
                Target.CurrentPath = EditorUtility.OpenFilePanel(
                    "Seleccionar archivo",
                    Application.persistentDataPath,
                    DataPersistanceManager.EXTESION_DATA_NAME
                );
            }
            GUILayout.EndHorizontal();
        }
    }
}
