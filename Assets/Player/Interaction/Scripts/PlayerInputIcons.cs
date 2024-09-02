///<summary>
/// Author: Halen
///
/// Slots for getting the input icons for Interaction system.
///
///</summary>

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {       
        namespace UI
        {
            public class PlayerInputIcons : MonoBehaviour
            {
                public static PlayerInputIcons Instance { get; private set; }
                private void Awake()
                {
                    if (!Instance)
                        Instance = this;
                    else if (Instance != this)
                        Destroy(gameObject);

                    DontDestroyOnLoad(gameObject);
                }

                [Serializable]
                public class InputIcon
                {
                    [SerializeField] private InputType m_input;
                    [SerializeField] private Sprite m_icon;

                    public InputType inputType => m_input;
                    public Sprite icon => m_icon;
                }
                
                [Serializable]
                public class SchemeIconGroup
                {
                    [SerializeField] private string m_controlScheme;
                    [SerializeField] private InputIcon[] m_inputIcons;

                    public string controlScheme => m_controlScheme;

                    public InputIcon GetInputIcon(InputType input) => Array.Find(m_inputIcons, inputIcon => inputIcon.inputType == input);
                }

                [SerializeField] private string[] m_controlSchemes = { "Keyboard&Mouse", "Gamepad" };

                [SerializeField] private SchemeIconGroup[] m_inputIconGroups;

                private string m_currentControlScheme;

                private SchemeIconGroup GetIconGroup(string controlScheme) => Array.Find(m_inputIconGroups, iconGroup => iconGroup.controlScheme == controlScheme);

                public Sprite GetInputIcon(InputType input)
                {
                    return GetIconGroup(m_currentControlScheme).GetInputIcon(input).icon;
                }

                public void SetCurrentControlScheme(PlayerInput input)
                {
                    m_currentControlScheme = input.currentControlScheme;
                    Debug.Log(input.currentControlScheme);
                }
            }
        }
    }
}
