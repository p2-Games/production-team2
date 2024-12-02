using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Millivolt.Sound.SFXController;

namespace Millivolt.Sound
{
    public class SoundTest : MonoBehaviour
    { 
        public SFXController controller;
        public SoundClipCollection soundClipCollection;
        public Transform place;
        public string collectionToPlay;
        public string clipToPlay;
        public TMP_Text SoundGroupNameText;
        public TMP_Text SoundNameText;
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SoundGroupNameText.text = collectionToPlay;
            SoundNameText.text = clipToPlay;
        }

        public void ChangeGroup(bool nextButton)
        {
            if (nextButton)
            {
                
            }
            else
            {

            }
        }

        public void ChangeSound(bool nextButton)
        {
            if (nextButton)
            {

            }
            else
            {

            }
        }

        public void ButtonSound()
        {
            controller.PlayRandomSoundClip(collectionToPlay, place);
        }
        public void ButtonSoundRandom()
        {
            controller.PlaySoundClip(collectionToPlay, clipToPlay, place);
        }
    }
}
