///<summary>
/// Author: Emily
///
/// Handles swapping the material on the face to display Robert's emotions
///
///</summary>

using System;
using System.Collections;
using UnityEngine;

namespace Millivolt
{
	public class PlayerEmotion : MonoBehaviour
	{
        [Tooltip("Input the element number for the material that is the face")]
        [SerializeField] private int m_eyeMatIndex;

        [Tooltip("Drag all the emotion materials in")]
		[SerializeField] private Material[] m_emotions;

        /// <summary>
        /// Displays Roberts Default face
        /// </summary>
		public void DisplayDefault()
		{
			foreach (Material emotion in m_emotions)
			{
				if (emotion.name.Contains("Default"))
                    ChangeMaterial(emotion);
            }            
		}

        /// <summary>
        /// Displays Roberts Happy face
        /// </summary>
		public void DisplayHappy(float duration)
		{
            print("DISPLAY HAPPY");
            foreach (Material emotion in m_emotions)
            {
                if (emotion.name.Contains("Happy"))
                    ChangeMaterial(emotion);
            }

            StartCoroutine(DefaultDelay(duration));
        }

        /// <summary>
        /// Displays Roberts Shocked face
        /// </summary>
		public void DisplayShocked(float duration)
		{

            foreach (Material emotion in m_emotions)
            {
                if (emotion.name.Contains("Shocked"))
                    ChangeMaterial(emotion);
            }

            StartCoroutine(DefaultDelay(duration));
        }

        /// <summary>
        /// Displays Roberts Sleepy face
        /// </summary>
		public void DisplaySleepy(float duration)
		{
            foreach (Material emotion in m_emotions)
            {
                if (emotion.name.Contains("Sleepy"))
                    ChangeMaterial(emotion);
            }

            StartCoroutine(DefaultDelay(duration));
        }

        /// <summary>
        /// ReInitalises all the materials on Robert. It HAS to be done like this (from my internet research) to properly get all the materials done correctly
        /// </summary>
        /// <param name="material"></param>
        private void ChangeMaterial(Material material)
        {
            //Temp array to hold all the materials on robert
            Material[] holdMats = GetComponent<MeshRenderer>().materials;
            //Create instance of the material you want to change to
            Material emotionInstance = new Material(material);
            //Grabs the material in the array spot to change to the face you want
            holdMats[m_eyeMatIndex] = emotionInstance;
            //Wipes the material list and replaces it with the new one
            GetComponent<MeshRenderer>().materials = holdMats;
        }

        /// <summary>
        /// Will wait for a set amount of time and then call Default face display
        /// </summary>
        private IEnumerator DefaultDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            DisplayDefault();
        }
    }
}
