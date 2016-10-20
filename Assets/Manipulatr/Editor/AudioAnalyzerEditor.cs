using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AudioAnalyzer
{
	[CustomEditor(typeof(AudioAnalyzer))]
	public class AudioAnalyzerEditor : Editor {
		
		SerializedProperty _FFTBins; 
		SerializedProperty _samplerate;
		SerializedProperty _sensitivity;
		SerializedProperty _averagingPeriod;
		SerializedProperty _inputDeviceNumber;
		SerializedProperty _usesMicrophone;
		private int oldInputDevice = 0;
		private int[] binAmounts = {256,512,1024,2048,4096};
		private string[] binNames = {"Lowest","Low","Medium","High","Highest"};
		private int[] sampleRates = {11025, 22050, 44100};
		private string[] srNames = {"11025","22050","44100"};
		
		public void OnEnable() {
			_usesMicrophone = serializedObject.FindProperty("usesMicrophone");
			_FFTBins = serializedObject.FindProperty("FFTBins");
			_samplerate = serializedObject.FindProperty("samplerate");
			_sensitivity = serializedObject.FindProperty("sensitivity");
			_averagingPeriod = serializedObject.FindProperty("averagingPeriod");
			_inputDeviceNumber = serializedObject.FindProperty("inputDeviceNumber");
			oldInputDevice = _inputDeviceNumber.intValue;
		}
		
		public override void OnInspectorGUI() {
			serializedObject.Update();
			_usesMicrophone.boolValue = EditorGUILayout.Toggle(" Use Microphone ",_usesMicrophone.boolValue);
			if (Microphone.devices.Length <= _inputDeviceNumber.intValue)
				_inputDeviceNumber.intValue = 0;
			_inputDeviceNumber.intValue = EditorGUILayout.Popup(" Input Device ",_inputDeviceNumber.intValue,Microphone.devices);
			_samplerate.intValue = EditorGUILayout.IntPopup(" Samplerate ",_samplerate.intValue,srNames,sampleRates);
			_sensitivity.floatValue = EditorGUILayout.Slider(" Sensitivity ",_sensitivity.floatValue,1,10000);
			_averagingPeriod.intValue = EditorGUILayout.IntSlider(" Volume Averaging Period ",_averagingPeriod.intValue,128,8196);
			_FFTBins.intValue = EditorGUILayout.IntPopup(" FFT Resolution ",_FFTBins.intValue,binNames,binAmounts);
			serializedObject.ApplyModifiedProperties ();
		}
		
		public void Update()
		{
			if (oldInputDevice != _inputDeviceNumber.intValue)
			{
				oldInputDevice = _inputDeviceNumber.intValue;
				int minFreq = -1;
				int maxFreq = -1;
				Microphone.GetDeviceCaps(Microphone.devices[oldInputDevice],out minFreq,out maxFreq);
				if (minFreq == 0 && maxFreq == 0)
				{
					sampleRates = new int[] {11025, 22050, 44100};
				}
			}
		}
	}
}