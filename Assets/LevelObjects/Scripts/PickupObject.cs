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
        public enum PickupType
        {
            Standard,
            Heavy,
            Immovable
        }

        [RequireComponent(typeof(Rigidbody), typeof(Collider))]
        public class PickupObject : LevelObject
        {
            [Header("Pickup Object Details")]
            [SerializeField] private PickupType m_type;
            public PickupType pickupType => m_type;
            public bool playerCanGrab => m_type != PickupType.Immovable;

            private Rigidbody m_rb;
            public Rigidbody rb => m_rb;

            private void Start()
            {
                m_rb = GetComponent<Rigidbody>();
            }
        }
    }
}