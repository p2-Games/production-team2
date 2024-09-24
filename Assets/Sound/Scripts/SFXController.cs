///<summary>
/// Author: Halen
///
/// Singleton sound manager class.
///
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Millivolt.SFXController;

namespace Millivolt
{
    public class SFXController : MonoBehaviour
    {
        [System.Serializable]
        public class SoundClipCollection
        {
            [SerializeField] private string m_collectionName;
            [SerializeField] private List<SoundClip> m_soundClips;
            public string collectionName { get { return m_collectionName; } }
            public List<SoundClip> soundClips { get { return m_soundClips; } }
        }

        [System.Serializable]
        public class SoundClip
        {
            [Tooltip("The name of the sound.")]
            [SerializeField] private string m_clipName;
            [Tooltip("The audio file.")]
            [SerializeField] private AudioClip m_audioClip;
            [Tooltip("The volume of the sound.")]
            [SerializeField, Range(0f, 1f)] private float m_volume = 1;
            [Tooltip("How far the sound attenuates.")]
            [SerializeField] private float m_range = 500f;
            [Tooltip("If the sound plays continuously or stops after it has played once.")]
            [SerializeField] private bool m_loop = false;

            public string clipName => m_clipName;
            public AudioClip audioClip => m_audioClip;
            public float volume => m_volume;
            public float range => m_range;
            public bool loop => m_loop;
        }

        #region Singleton
        public static SFXController Instance;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
        #endregion

        [SerializeField] private SoundClipObject m_soundClipObjectPrefab;
        [SerializeField] private List<SoundClipCollection> m_clipCollections;

        private void PlayAudioClipObject(SoundClip soundClip, Transform obj)
        {           
            SoundClipObject newSoundClipObject = Instantiate(m_soundClipObjectPrefab, obj);
            newSoundClipObject.name = soundClip.clipName;
            newSoundClipObject.Init(soundClip);
        }

        private void PlayAudioClipPosition(SoundClip soundClip, Vector3 position)
        {
            SoundClipObject newSoundClipObject = Instantiate(m_soundClipObjectPrefab, position, Quaternion.identity);
            newSoundClipObject.name = soundClip.clipName;
            newSoundClipObject.Init(soundClip);
        }

        /// <summary>
        /// Play a specified sound clip from a collection on a GameObject.
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="clipName"></param>
        public void PlaySoundClip(string collectionName, string clipName, Transform obj)
        {
            // check if transform is null
            if (obj == null)
            {
                LogMissingTransformError(collectionName, clipName);
                return;
            }

            // check if soundclip is null
            SoundClip soundClip = m_clipCollections.Find(collection => collection.collectionName == collectionName).soundClips.Find(clip => clip.clipName == clipName);
            if (soundClip == null)
            {
                LogMissingClipError(collectionName, clipName);
                return;
            }

            PlayAudioClipObject(soundClip, obj);
        }

        public void PlaySoundClip(string collectionName, string clipName, Vector3 position)
        {
            SoundClip soundClip = m_clipCollections.Find(collection => collection.collectionName == collectionName).soundClips.Find(clip => clip.clipName == clipName);
            if (soundClip == null)
            {
                LogMissingClipError(collectionName, clipName);
                return;
            }

            PlayAudioClipPosition(soundClip, position);
        }

        /// <summary>
        /// Play a random sound clip from a specified collection on a GameObject.
        /// </summary>
        public void PlayRandomSoundClip(string collectionName, Transform obj)
        {
            SoundClipCollection collection = m_clipCollections.Find(collection => collection.collectionName == collectionName);
            if (collection == null)
            {
                LogMissingCollectionError(collectionName);
                return;
            }

            List<SoundClip> soundClips = collection.soundClips;
            if (soundClips.Count == 0)
            {
                LogMissingSoundClipsError(collectionName);
                return;
            }

            PlayAudioClipObject(soundClips[Random.Range(0, soundClips.Count)], obj);
        }

        /// <summary>
        /// Plays a random sound clip from a specified collection at a point in world space.
        /// </summary>
        public void PlayRandomSoundClip(string collectionName, Vector3 position)
        {
            SoundClipCollection collection = m_clipCollections.Find(collection => collection.collectionName == collectionName);
            if (collection == null)
            {
                LogMissingCollectionError(collectionName);
                return;
            }

            List<SoundClip> soundClips = collection.soundClips;
            if (soundClips.Count == 0)
            {
                LogMissingSoundClipsError(collectionName);
                return;
            }

            PlayAudioClipPosition(soundClips[Random.Range(0, soundClips.Count)], position);
        }

        private void LogMissingCollectionError(string collectionName)
        {
            Debug.LogError("The provided SoundClipCollection does not exist. Collection: " + collectionName + ".");
        }

        private void LogMissingClipError(string collectionName, string clipName)
        {
            Debug.LogError("The provided SoundClip or SoundClipCollection does not exist. Collection: " + collectionName + ", Clip: " + clipName + ".");
        }

        private void LogMissingSoundClipsError(string collectionName)
        {
            Debug.LogError("The provided SoundClipCollection does not contain any SoundClips. Collection: " + collectionName + ".");
        }

        private void LogMissingTransformError(string collectionName, string clipName)
        {
            Debug.LogError("The provided Transform is null. Collection: " + collectionName + ", Clip: " + clipName + ".");
        }
    }
}