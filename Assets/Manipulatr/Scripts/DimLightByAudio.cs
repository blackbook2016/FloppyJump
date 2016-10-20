using UnityEngine;
using System.Collections;

namespace AudioAnalyzer
{
	[AddComponentMenu("Manipulatr/Light Dimmer")]
	public class DimLightByAudio : MonoBehaviour {
	
		AudioAnalyzer audioIn;
		public GameObject audioInputObject;
		public bool useFreqRange = false;
		public float intensityModifier = 1.0f;
		public float bandMinFreq = 40.0f;
		public float bandMaxFreq = 6000.0f;
		public float absoluteMaxFreq = 44100.0f;
		public float threshold = 1.0f;
		public float rDamp = 1.0f;
		
		void Start () {
			if (audioInputObject == null)
		            audioInputObject = GameObject.Find("AudioAnalyzer");
			if (audioInputObject == null)
				Debug.LogError("You need to have a FFT Source. Try adding AudioAnalyzer -prefab to your scene!");
		    audioIn = (AudioAnalyzer) audioInputObject.GetComponent("AudioAnalyzer");
		}
		
		void Update () {
			absoluteMaxFreq = audioIn.GetComponent<AudioSource>().clip.frequency;
			float l = audioIn.loudness;
			if (useFreqRange)
			{
				int i = 0;
				float freqsum = 0;
				int minbin = (int)Mathf.Floor(bandMinFreq / absoluteMaxFreq * audioIn.FFTBins);
				int maxbin = (int)Mathf.Floor(bandMaxFreq / absoluteMaxFreq * audioIn.FFTBins);
				for (i = minbin; i < maxbin; i++)
					freqsum += audioIn.frequencyData[i];
				freqsum /= i;
				l = freqsum*10000;
			}
			if ( l >= threshold)
			{
				this.GetComponent<Light>().intensity = l * intensityModifier; 					
			}
		}
	}
}
