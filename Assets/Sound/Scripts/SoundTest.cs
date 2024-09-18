using UnityEngine;

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

            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {
                if (Input.GetKeyDown("r"))
                {
                    controller.PlayRandomSoundClip(collectionToPlay, place);
                }
                if (Input.GetKeyDown("s"))
                {
                    controller.PlaySoundClip(collectionToPlay, clipToPlay, place);
                }
            }
        }
    }
}

