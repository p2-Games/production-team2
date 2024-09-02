///<summary>
/// Author: Halen
///
/// Custom display for the Custom Editor Icons.
///
///</summary>

using System;
using UnityEditor;
using UnityEngine;

using EGL = UnityEditor.EditorGUILayout;

namespace Millivolt
{
    namespace Player
    {
        namespace UI
        {
            [CustomEditor(typeof(PlayerInputIcons))]
            public class PlayerInputIconsEditor : Editor
            {
                private SerializedProperty m_controlSchemes, m_inputIconGroups;

                private bool m_hasBeenChanged;

                private void OnEnable()
                {
                    GetSerializedProperties();
                }

                public override void OnInspectorGUI()
                {
                    serializedObject.Update();

                    m_hasBeenChanged = false;
                    EditorGUI.BeginChangeCheck();

                    OnGUI();

                    if (EditorGUI.EndChangeCheck() || m_hasBeenChanged)
                    {
                        EditorUtility.SetDirty(target);
                        serializedObject.ApplyModifiedProperties();
                        GetSerializedProperties();
                    }

                    //base.OnInspectorGUI();
                }

                protected virtual void OnGUI()
                {
                    using (new EditorGUI.DisabledScope(true))
                        EGL.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

                    // draw control scheme string array property
                    EGL.PropertyField(m_controlSchemes);

                    // create an input icon group for each control scheme
                    if (m_inputIconGroups.arraySize != m_controlSchemes.arraySize)
                    {
                        m_inputIconGroups.arraySize = m_controlSchemes.arraySize;
                    }

                    int numOfSchemes = m_inputIconGroups.arraySize;
                    int numOfInputs = (int)InputType.INPUT_COUNT;

                    EditorGUI.indentLevel++;
                    for (int s = 0; s < numOfSchemes; s++)
                    {
                        // ensure control scheme name is correct
                        SerializedProperty controlSchemeName = m_inputIconGroups.GetArrayElementAtIndex(s).FindPropertyRelative("m_controlScheme");
                        if (controlSchemeName.stringValue != m_controlSchemes.GetArrayElementAtIndex(s).stringValue)
                            controlSchemeName.stringValue = m_controlSchemes.GetArrayElementAtIndex(s).stringValue;

                        // check that the InputIcons have an entry for each input type
                        SerializedProperty inputIcons = m_inputIconGroups.GetArrayElementAtIndex(s).FindPropertyRelative("m_inputIcons");
                        if (inputIcons.arraySize != numOfInputs)
                        {
                            inputIcons.arraySize = numOfInputs;
                        }

                        // draw scheme icon group label
                        EGL.SelectableLabel(controlSchemeName.stringValue);

                        EditorGUI.indentLevel++;
                        for (int i = 0; i < numOfInputs; i++)
                        {
                            SerializedProperty inputIcon = inputIcons.GetArrayElementAtIndex(i);

                            // ensure the InputType for each entry is correct
                            SerializedProperty inputType = inputIcon.FindPropertyRelative("m_input");
                            if (inputType.enumValueIndex != i)
                            {
                                inputType.enumValueIndex = i;
                            }

                            EGL.BeginHorizontal();
                            // draw input type (name)
                            EGL.LabelField(Enum.GetName(typeof(InputType), inputType.enumValueIndex), GUILayout.Width(Screen.width * 0.3f));

                            // draw property field for sprite
                            EGL.PropertyField(inputIcon.FindPropertyRelative("m_icon"), GUIContent.none);
                            EGL.EndHorizontal();
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }

                protected void GetSerializedProperties()
                {
                    m_controlSchemes = serializedObject.FindProperty(nameof(m_controlSchemes));
                    m_inputIconGroups = serializedObject.FindProperty(nameof(m_inputIconGroups));
                }
            }
        }
    }
}
