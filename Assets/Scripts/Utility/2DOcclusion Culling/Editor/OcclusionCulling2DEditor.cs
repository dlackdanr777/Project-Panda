using UnityEditor;
using UnityEngine;

namespace Muks.OcclusionCulling2D
{
    [CustomEditor(typeof(OcclusionCulling2D))]
    public class OcclusionCulling2DEditor : Editor
    {
        private OcclusionCulling2D _oc;
        private SerializedObject _serialObj;
        private SerializedProperty _objectSettingList;

        private void OnEnable()
        {
            _oc = target as OcclusionCulling2D;
            _serialObj = new SerializedObject(_oc);
            _objectSettingList = _serialObj.FindProperty("_objectSettingList");
        }


        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.PropertyField(_objectSettingList, true);


            if(GUILayout.Button("Generate Culling List"))
            {
                _oc.GenerateCullingList();
                _serialObj.UpdateIfRequiredOrScript();
            }

            if (GUILayout.Button("Clear Culling List"))
            {
                _oc.Clear();
                _serialObj.UpdateIfRequiredOrScript();
            }


        }
    }

}

