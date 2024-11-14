///<summary>
/// Author: Halen
///
/// Instantiates a GameObject.
///
///</summary>

using UnityEngine;
 
namespace Millivolt
{
    using LevelObjects;
    using Millivolt.LevelObjects.PickupObjects;

    public class ObjectSpawner : MonoBehaviour
    {
        [Tooltip("The object to be spawned by the script.")]
        [SerializeField] private GameObject m_object;

        [Tooltip("The transform at which the object will be spawned")]
        [SerializeField] private Transform m_spawnPoint;

        [Tooltip("If spawned objects should have a random orientation when spawned.\n" +
            "Otherwise, will inherit the rotation of the spawn point Transform.")]
        [SerializeField] private bool m_giveRandomOrientation = false;

        [Tooltip("Whether or not the object can be spawned.")]
        [SerializeField] private bool m_objectCanSpawn = true;

        [Tooltip("If true, the object will be spawned as soon as possible." +
            "\nThis would be on Start if Object Can Spawn is true.")]
        [SerializeField] private bool m_spawnAtStart = false;

        [Tooltip("If the pickup should automatically respawn when destroyed.")]
        [SerializeField] private bool m_autoRespawn = false;

        private bool m_isSpawning = false;

        private GameObject m_spawnedObject;

        private void Start()
        {
            if (m_spawnAtStart)
                SpawnObject();
        }

        public void SetCanSpawnObject(bool value) => m_objectCanSpawn = value;

        public void AutoRespawn()
        {
            if (m_autoRespawn && !m_isSpawning)
                SpawnObject();
        }

        public void SpawnObject()
        {
            if (!m_objectCanSpawn)
                return;

            m_isSpawning = true;

            // destroy existing game object
            if (m_spawnedObject)
            {
                if (m_spawnedObject.TryGetComponent(out PickupObject pickup))
                {
                    m_spawnedObject = null;
                    pickup.Destroy();
                }
                else
                    Destroy(m_spawnedObject.gameObject);
            }

            // spawn the object
            m_spawnedObject = Instantiate(m_object, m_spawnPoint.position, m_giveRandomOrientation ? Random.rotation : m_spawnPoint.rotation);

            // if the spawned object is a LevelObject, set this MonoBehaviour to be its spawn parent
            if (m_spawnedObject.TryGetComponent(out LevelObject levelObj))
                levelObj.spawnParent = this;

            m_isSpawning = false;
        }
    }
}
