///<summary>
/// Author: Emily McDonald & Halen
///
/// Editor Script for assigning scenes for level data
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
	namespace Utilities
	{
		using EGL = EditorGUILayout;

		[CustomEditor(typeof(LevelManager))]
		public class LevelManagerEditor : Editor
		{
			private SceneAsset m_prev, m_next;
			
			protected SerializedProperty m_currentCheckpoint, m_levelCheckpoints, m_prevLevelName, m_nextLevelName, m_autoAddCheckpoints, m_lvlData, m_spawnScreen;

			protected bool hasBeenChanged;

            private void OnEnable()
            {
				GetSerializedProperties();
            }

            public override void OnInspectorGUI()
            {
				serializedObject.Update();

				hasBeenChanged = false;
				EditorGUI.BeginChangeCheck();

				OnGUI();

				if (EditorGUI.EndChangeCheck() || hasBeenChanged)
				{
					EditorUtility.SetDirty(target);
					serializedObject.ApplyModifiedProperties();
					GetSerializedProperties();
				}
            }

			protected virtual void OnGUI()
			{
				using (new EditorGUI.DisabledScope(true)) 
					EGL.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

                EGL.BeginHorizontal();
				EGL.LabelField(new GUIContent("Previous Scene"), EditorStyles.boldLabel);
                EGL.LabelField(new GUIContent("Next Scene"), EditorStyles.boldLabel);
				EGL.EndHorizontal();

                // check if previous scene value has been changed
                EGL.BeginHorizontal();
				SceneAsset prev = m_prev;
				m_prev = EGL.ObjectField(m_prev, typeof(SceneAsset), false) as SceneAsset;
				if (prev != m_prev)
				{
					m_prevLevelName.stringValue = m_prev.name;
				}

				// check if next scene value has been changed
				SceneAsset next = m_next;
				m_next = EGL.ObjectField(m_next, typeof(SceneAsset), false) as SceneAsset;
				if (next != m_next)
				{
					m_nextLevelName.stringValue = m_next.name;
				}

                EGL.EndHorizontal();

				EGL.BeginHorizontal();
				EGL.LabelField(m_prevLevelName.stringValue);
                EGL.LabelField(m_nextLevelName.stringValue);
				EGL.EndHorizontal();

                //Drag in Level checkpoints
                EGL.BeginVertical();
                //EditorGUILayout.LabelField(new GUIContent("Level Checkpoints"), EditorStyles.boldLabel);
                EGL.PropertyField(m_levelCheckpoints);
				EGL.PropertyField(m_autoAddCheckpoints);
                EGL.EndVertical();

                EGL.Space(10, false);

				EGL.PropertyField(m_lvlData);

                EGL.Space(10, false);

                EGL.PropertyField(m_spawnScreen);
            }

            protected void GetSerializedProperties()
			{
				//m_nextSceneAsset = serializedObject.FindProperty("prevLevel");
				//m_prevSceneAsset = serializedObject.FindProperty("nextLevel");

				m_currentCheckpoint = serializedObject.FindProperty("currentCheckpoint");
				m_levelCheckpoints = serializedObject.FindProperty("m_levelCheckpoints");
				m_prevLevelName = serializedObject.FindProperty("m_prevLevelName");
				m_nextLevelName = serializedObject.FindProperty("m_nextLevelName");
				m_autoAddCheckpoints = serializedObject.FindProperty("m_autoAddCheckpoints");
				m_lvlData = serializedObject.FindProperty("m_levelData");
				m_spawnScreen = serializedObject.FindProperty("m_spawnScreen");

            }
        }
	}
}
