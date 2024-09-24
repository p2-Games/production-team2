using Millivolt.Sound;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
    namespace sound
    {
        public class SoundTest : MonoBehaviour
        {
            public SFXController controller;
            public Transform place;
            public string collectionToPlay;
            public string clipToPlay;
            public TMP_Text soundCollectionName;
            public TMP_Text soundName;

            // Start is called before the first frame update
            void Start()
            {
                soundCollectionName.text = collectionToPlay;

                soundName.text = clipToPlay;
            }

            // Update is called once per frame
            void Update()
            {

            }

            public void PlaySelectedSound()
            {
                controller.PlaySoundClipObject(collectionToPlay, clipToPlay, place);
            }
            public void PlayRandomSound()
            {
                controller.PlayRandomSoundClipObject(collectionToPlay, place);

            }
        }
    }
}

