///<summary>
/// Author: Halen
///
/// Spawn objects when event is called.
///
///</summary>

using UnityEngine;
 
namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class SpawnObjectEvent : Event
            {
                [Tooltip("The object to be spawned by the script.")]
                [SerializeField] private LevelObject m_object;

                [Tooltip("The transform at which the object will be spawned. Also inherits rotation.")]
                [SerializeField] private Transform m_spawnPoint;

                [Tooltip("Whether or not the object can be spawned by the script." +
                    "\nChanged when the object it spawns is accepted into a receptacle, or by being activated by another Level Object.")]
                [SerializeField] private bool m_objectCanSpawn = true;

                [Tooltip("If true, the object will be spawned as soon as possible." +
                    "\nThis would be on Start if Object Can Spawn is true by default.")]
                [SerializeField] private bool m_spawnAtStart = false;

                private LevelObject m_spawnedObject;

                private void Start()
                {
                    if (m_spawnAtStart && m_objectCanSpawn)
                        SpawnObject();
                }

                public void SetCanSpawnObject(bool value)
                {
                    m_objectCanSpawn = value;

                    // if spawn at start and the object can now spawn and there is not already an object spawned,
                    // then spawn the object
                    if (m_spawnAtStart && value && !m_spawnedObject)
                        SpawnObject();
                }

                public override void DoEvent(bool value)
                {
                    // check if the object can be spawned
                    if (!m_objectCanSpawn)
                        return;

                    SpawnObject();
                }

                private void SpawnObject()
                {
                    // destroy existing game object
                    if (m_spawnedObject)
                        Destroy(m_spawnedObject.gameObject);

                    // spawn the object
                    m_spawnedObject = Instantiate(m_object, m_spawnPoint.position, m_spawnPoint.rotation);
                    m_spawnedObject.spawnParent = this;
                }
            }
        }
    }
}
