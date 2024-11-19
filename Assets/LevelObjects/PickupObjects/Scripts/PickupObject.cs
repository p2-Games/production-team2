///<summary>
/// Author: Halen
///
/// Script for indentifying objects that the player can pick up.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using Player;

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
                [SerializeField] private PickupType m_type;

                private bool m_isDissolving = false;

                public PickupType pickupType => m_type;
                public bool canInteract
                {
                    get
                    {
                        if (m_type == PickupType.Immovable || m_isDissolving)
                            return false;
                        return true;
                    }
                }

                private Rigidbody m_rb; public Rigidbody rb => m_rb;
                private Collider m_collider; public new Collider collider => m_collider;
                protected virtual void Start()
                {
                    m_rb = GetComponent<Rigidbody>();
                    m_collider = m_rb.GetComponent<Collider>();
                }

                private void OnCollisionEnter(Collision collision)
                {
                    // if this object is being held, don't play sounds
                    if (!GameManager.Player || gameObject == GameManager.Player.Interaction.heldObject)
                        return;

                    // if the other colliding object is the player, don't play sounds
                    if (collision.gameObject.GetComponent<PlayerModel>())
                        return;

                    // play a sound effect
                    SFXController.Instance.PlayRandomSoundClip("ScrewDrop", transform);
                }

                public void Destroy()
                {
                    // if screw is already being destroyed, don't try to destroy it again
                    if (m_isDissolving)
                        return;
                    
                    m_isDissolving = true;

                    // if this is the held object, then drop it
                    if (GameManager.Player && GameManager.Player.Interaction.heldObject == gameObject)
                        GameManager.Player.Interaction.DropObject();

                    // check if spawn parent should auto respawn this object
                    if (spawnParent)
                    {
                        spawnParent.AutoRespawn();
                    }

                    GetComponent<MeshDissolver>().Dissolve();
                }
            }
        }
    }
}