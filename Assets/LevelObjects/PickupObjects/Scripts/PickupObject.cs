///<summary>
/// Author: Halen
///
/// Script for indentifying objects that the player can pick up.
///
///</summary>

using System;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace PickupObjects
        {
            public enum PickupType
            {
                Standard,
                Heavy,
                Immovable
            }

            [RequireComponent(typeof(Rigidbody), typeof(Collider))]
            public class PickupObject : LevelObject
            {
                [Header("Pickup Object Details"), Tooltip("The keyword for the type of pickup.")]
                [SerializeField] private string m_keyword;
                public string keyword => m_keyword;

                [Tooltip("The maximum speed the object can travel at when following the player's cursor while picked up.")]
                [SerializeField] private float m_followMaxSpeed;
                public float followMaxSpeed => m_followMaxSpeed;

                [Tooltip("The acceleration of the object when following the player's cursor while picked up.")]
                [SerializeField] private float m_followAcceleration;
                public float followAcceleration => m_followAcceleration;

                [SerializeField] private PickupType m_type;
                public PickupType pickupType => m_type;
                public bool playerCanGrab => m_type == PickupType.Immovable;

                public Vector3 velocity
                {
                    get => m_rb.velocity;
                    set => m_rb.velocity = value;
                }

                public bool useGravity
                {
                    get => m_rb.useGravity;
                    set => m_rb.useGravity = value;
                }

                private Rigidbody m_rb;

                private void Start()
                {
                    m_rb = GetComponent<Rigidbody>();
                }
            }
        }
    }
}
