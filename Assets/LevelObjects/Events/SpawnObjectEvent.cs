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
                [SerializeField] private Transform m_spawnPoint;
                
                [SerializeField] private GameObject[] m_objectsToSpawn;

                public override void DoEvent(bool value)
                {
                    foreach (GameObject obj in m_objectsToSpawn)
                        Instantiate(obj);
                }
            }
        }
    }
}
