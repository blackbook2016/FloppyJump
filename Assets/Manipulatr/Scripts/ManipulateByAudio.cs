using UnityEngine;
using System.Collections;

namespace AudioAnalyzer
{
	[AddComponentMenu("Manipulatr/Transform manipulator")]
	public class ManipulateByAudio : MonoBehaviour {
	
		AudioAnalyzer audioIn;
		public GameObject audioInputObject;
		public bool affectTransform = false;
		public bool affectRotation = false;
		public bool affectScale = true;
		public bool useFreqRange = false;
		public bool scaleDecay = false;
		public Vector3 transformModifier;
		public Vector3 rotationAngleModifier;
		public Vector3 scaleModifier;
		public float scaleLimitMax = 10.0f;
		public float bandMinFreq = 40.0f;
		public float bandMaxFreq = 6000.0f;
		public float absoluteMaxFreq = 44100.0f;
		public float threshold = 1.0f;
		public float rDamp = 1.0f;
		public float sDamp = 1.0f;
		public float pDamp = 1.0f;

		private float freqsum = 0;

		// Use this for initialization
		void Start () {
			if (audioInputObject == null)
	            audioInputObject = GameObject.Find("AudioAnalyzer");
			if (audioInputObject == null)
				Debug.LogError("You need to have a FFT Source. Try adding AudioAnalyzer -prefab to your scene!");
	        audioIn = (AudioAnalyzer) audioInputObject.GetComponent("AudioAnalyzer");
		}
		
		// Update is called once per frame
		void Update () {
			absoluteMaxFreq = audioIn.GetComponent<AudioSource>().clip.frequency;
			float l = audioIn.loudness;
			Vector3 d = Vector3.zero;

			if (scaleDecay)
			{
				if (this.gameObject.transform.localScale.x > 0)
					d[0] = 1;
				if (this.gameObject.transform.localScale.y > 0)
					d[1] = 1;
				if (this.gameObject.transform.localScale.z > 0)
					d[2] = 1;
				this.gameObject.transform.localScale -= d * Time.deltaTime * sDamp;
			}

			if (useFreqRange)
			{
				int i = 0;
				freqsum = 0;
				int minbin = (int)Mathf.Floor(bandMinFreq / absoluteMaxFreq * (audioIn.cCount));
				int maxbin = (int)Mathf.Floor(bandMaxFreq / absoluteMaxFreq * (audioIn.cCount));
				for (i = minbin; i < maxbin; i++)
					freqsum += audioIn.frequencyData[i];
				freqsum /= i;
				l = freqsum*10000;
			}
			if ( l >= threshold )
			{

				if (affectTransform)
				{
					this.transform.position += (l * transformModifier *Time.deltaTime); 					
				}
				if (affectRotation)
				{
					Quaternion rr = this.gameObject.transform.rotation;
					rr.eulerAngles += rotationAngleModifier * l * Time.deltaTime;
					this.transform.rotation = rr;
				}
				if (affectScale)
				{
					this.gameObject.transform.localScale = Vector3.one + (scaleModifier * Time.deltaTime * l);
					if (scaleLimitMax != 0)
					{
						d = this.gameObject.transform.localScale;
						if (this.gameObject.transform.localScale.x > scaleLimitMax)
							d[0] = scaleLimitMax;
						if (this.gameObject.transform.localScale.y > scaleLimitMax)
							d[1] = scaleLimitMax;
						if (this.gameObject.transform.localScale.z > scaleLimitMax)
							d[2] = scaleLimitMax;
						this.gameObject.transform.localScale = d;
					}
				}
			}

			
		}
	}
}
