using UnityEngine;
using System.Collections;
using Exocortex.DSP;

namespace AudioAnalyzer
{
	[AddComponentMenu("Manipulatr/Audio Analyzer")]
	[RequireComponent(typeof(AudioSource))]
	public class AudioAnalyzer : MonoBehaviour {
		private float fFreq = 0;
		private float fLoud = 0;
		private float fFreqLoud = 0;
		private float[] fftData;
		//private int lowlimit = 1;
		public int cCount = 16;
		private int bCount = 8;
		private AudioSource audioSource;
		private AudioClip micClip;
		public int FFTBins = 4096; // must be power of two, maximum 8192!
		public int samplerate = 11025;
		public int averagingPeriod = 256;
		public float sensitivity = 100;
		public int inputDeviceNumber = 0;
		public bool usesMicrophone = true; 
		public float loudness
		{
			get {return fLoud;}
			set {Debug.Log("Parameter loudness is read-only!");}
		}
		public float fundamentalFrequency
		{
			get {return fFreq;}
			set {Debug.Log("Parameter fundamentalFrequency is read-only!");}
		}
		public float fundamentalFrequencyLoudness
		{
			get {return fFreqLoud;}
			set {Debug.Log("Parameter fundamentalFrequencyLoudness is read-only!");}
		}
		public float[] frequencyData
		{
			get {return fftData;}
			set {Debug.Log("Parameter frequencyData is read-only!");}
		}
		
	    void Start() {
			Debug.Log("Audio analyzer start on object: "+gameObject.name);


			audioSource = GetComponent<AudioSource>();
			if (usesMicrophone)
			{
				if (Microphone.devices.Length <= 0)
				{
					usesMicrophone = false;
					Debug.Log("No microphone or other audio input device present. Please add one in order to use analysis with live audio.");
					Debug.Log("Reverting to use the audio clip provided...");
					micClip = audioSource.clip;
					audioSource.Play();
				}
				else
				{
					audioSource.loop = true;
					micClip = Microphone.Start(Microphone.devices[inputDeviceNumber], true, 10, samplerate);
					audioSource.clip = micClip;
					audioSource.mute = true;
	        		while (!(Microphone.GetPosition(null) > 0)){}
				}
			}
			else
			{
				micClip = audioSource.clip;
				audioSource.Play();
			}
			cCount = FFTBins / 2;
			bCount = cCount / 2;
			fftData = new float[cCount];
			samplerate = audioSource.clip.frequency;
	    }
	
	    void Update(){
	        fLoud = GetAveragedVolume() * sensitivity;
			fFreq = GetFundamentalFrequency();
	    }
	    
		float GetAveragedVolume()
		{ 
			float[] data = new float[averagingPeriod];
			float a = 0;
			int position = 0;
			int micPosition = 0;
			if (usesMicrophone)
				micPosition = Microphone.GetPosition(Microphone.devices[inputDeviceNumber]);
			else
				micPosition = audioSource.timeSamples;
			if (micPosition > averagingPeriod)
				position = micPosition - averagingPeriod;
			micClip.GetData(data,position);
			foreach(float s in data)
			{
				a += Mathf.Abs(s);
			}
			return a/averagingPeriod;
		}
		
		float GetFundamentalFrequency()
		{
			float[] data = new float[FFTBins];
			
			int position = 0;
			int micPosition = 0;
			if (usesMicrophone)
				micPosition = Microphone.GetPosition(Microphone.devices[inputDeviceNumber]);
			else
				micPosition = audioSource.timeSamples;
			if (micPosition > FFTBins)
				position = micPosition - FFTBins;
			micClip.GetData(data,position);
			return ProcessData(data);
			
		}
		
		float ProcessData(float[] data)
		{
			float prevMax = 0f;
			ComplexF[] complexData  = new ComplexF[FFTBins];
			int j = 0;
			for (int i = 0; i < FFTBins; i++)
			{
				complexData[i] = ComplexF.FromRealImaginary(data[i],0);
				j++;
			}
			Fourier.FFT(complexData,FourierDirection.Forward);
			int max = 0;
			for (int i = 0; i < cCount; i++)
			{
				float cur = Mathf.Abs(complexData[i].Re);
				frequencyData[i] = cur / 20000;
				if ( cur > prevMax)
				{
					max = i;
					prevMax = cur;
				}
				/*Vector3 origin = new Vector3(i*0.001f,0f);
			Vector3 target = new Vector3(i*0.001f,cur);
			Debug.DrawLine(origin,target);*/
			}
			//Debug.Log(max*samplerate/FFTBins);
			return max*samplerate/FFTBins;
		}
	}
}