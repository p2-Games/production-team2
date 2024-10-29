///<summary>
/// Author: Halen
///
/// Script for indentifying objects that the player can pick up.
///
///</summary>

using Pixelplacement;
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
                Immovable
            }

            [RequireComponent(typeof(Rigidbody))]
            public class PickupObject : LevelObject
            {
                [Header("Pickup Object Details")]
                [SerializeField] protected PickupType m_type;

                private bool m_isDissolving = false;

                public PickupType pickupType => m_type;
                public bool canInteract => m_type != PickupType.Immovable || !m_isDissolving;

                protected Rigidbody m_rb; public Rigidbody rb => m_rb;

                protected virtual void Start()
                {
                    m_rb = GetComponent<Rigidbody>();
                }

                private void OnCollisionEnter(Collision collision)
                {
                    if (GameManager.PlayerInteraction && gameObject != GameManager.PlayerInteraction.heldObject)
                        SFXController.Instance.PlayRandomSoundClip("ScrewDrop", transform);
                }

                public void Destroy()
                {
                    m_isDissolving = true;

                    // if this is the held object, then drop it
                    GameManager.PlayerInteraction.DropObject();

                    // check if spawn parent should auto respawn this object
                    if (spawnParent)
                        spawnParent.AutoRespawn();

                    GetComponent<MeshDissolver>().Dissolve();
                }
            }
        }
    }
}