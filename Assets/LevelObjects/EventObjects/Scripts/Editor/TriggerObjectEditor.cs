///<summary>
/// Author: Halen
///
/// Displays a warning for the TriggerObject.
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace EventObjects
        {
            [CustomEditor(typeof(TriggerObject))]
            public class TriggerObjectEditor : Editor
            {
                public override void OnInspectorGUI()
                {
                    base.OnInspectorGUI();

                    EditorGUILayout.SelectableLabel("!! WARNING !!\n" +
                        "Deactivate Events on this object will not occur.", EditorStyles.centeredGreyMiniLabel);
                }
            }
        }
    }
}
