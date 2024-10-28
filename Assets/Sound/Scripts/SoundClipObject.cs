///<summary>
/// Author: Halen
///
/// Object for instantiating sounds in the scene.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    public class SoundClipObject : MonoBehaviour
    {
        private AudioSource m_source;
        private bool m_isPlaying = false;

        //Emily's work
        private float m_originalVolume;
        private int m_typeVal;

        public void Init(SFXController.SoundClip soundClip, SoundType type)
        {
            if (!m_source.enabled)
                return;

            // get source component
            m_source = GetComponent<AudioSource>();
            m_source.clip = soundClip.audioClip;
            m_source.playOnAwake = true;

            // set variables
            m_source.volume = soundClip.volume;
            m_source.maxDistance = soundClip.range;
            m_source.loop = soundClip.loop;

            m_originalVolume = m_source.volume;
            m_typeVal = (int)type;

            // global volume
            switch (type)
            {
                case SoundType.Effect:
                    m_source.volume *= PlayerSettings.Instance.sfxVolume;
                    break;
                case SoundType.Music:
                    m_source.spatialBlend = 0;
                    m_source.volume *= PlayerSettings.Instance.musicVolume;
                    break;
                case SoundType.Voice:
                    break;
            }

            // play clip
            m_source.Play();
            m_isPlaying = true;
        }

        public void Update()
        {
            // don't destroy the object if it should loop
            if (m_source.loop)
                return;

            VolumeCheck();
                
            // destroy SoundClipObject when clip finishes playing
            if (m_isPlaying && !m_source.isPlaying)
                Destroy(gameObject);
        }

        private void VolumeCheck()
        {
            switch(m_typeVal)
            {
                case 0:
                    m_source.volume = m_originalVolume * PlayerSettings.Instance.sfxVolume;
                    break;
                case 1:
                    m_source.volume = m_originalVolume * PlayerSettings.Instance.musicVolume;
                    break;
                case 2:
                    break;
            }
        }
    }

}
