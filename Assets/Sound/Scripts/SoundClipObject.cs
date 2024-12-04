///<summary>
/// Author: Halen
///
/// Object for instantiating sounds in the scene.
///
///</summary>

using UnityEngine;

namespace Millivolt.Sound
{
    public class SoundClipObject : MonoBehaviour
    {
        private AudioSource m_source;
        private bool m_isPlaying = false;

        //Emily's work
        private float m_originalVolume;
        private SoundType m_type;

        public void Init(SFXController.SoundClip soundClip, SoundType type)
        {
            // get source component
            m_source = GetComponent<AudioSource>();
            if (!m_source.enabled)
                return;

            m_source.clip = soundClip.audioClip;

            m_type = type;
            m_source.loop = soundClip.loop;

            // set range/if sound is 2D
            if (soundClip.range <= 0)
                m_source.spatialBlend = 0;
            else
                m_source.maxDistance = soundClip.range;

            m_originalVolume = soundClip.volume;
            
            VolumeCheck();

            // play clip
            m_source.Play();
            m_isPlaying = true;
        }

        public void Update()
        {
            // Check the what the volume of the sound should be
            VolumeCheck();

            // don't destroy the object if it should loop
            if (m_source.loop)
                return;

            // destroy SoundClipObject when clip finishes playing
            if (m_isPlaying && !m_source.isPlaying)
                Destroy(gameObject);
        }

        private void VolumeCheck()
        {
            switch(m_type)
            {
                case SoundType.Effect:
                    m_source.volume = m_originalVolume * PlayerSettings.Instance.sfxVolume;
                    break;
                case SoundType.Music:
                    m_source.volume = m_originalVolume * PlayerSettings.Instance.musicVolume;
                    break;
                case SoundType.Voice:
                    break;
            }
        }
    }

}
