using UnityEditor;
using UnityEngine;
using System.Collections;

namespace AudioAnalyzer
{
	[CustomEditor(typeof(DimLightByAudio))]
	public class DimLightByAudioEditor : Editor {
		SerializedProperty _threshold;
		SerializedProperty _audioInputObject;
		SerializedProperty _materialColor;
		SerializedProperty _bandMinFreq;
		SerializedProperty _bandMaxFreq;
		SerializedProperty _absoluteMaxFreq;
		SerializedProperty _useFreqRange;
		SerializedProperty _intensityModifier;
		private bool sceneObject = false;
		
		public void OnEnable() {
			_audioInputObject = serializedObject.FindProperty("audioInputObject");
			_threshold = serializedObject.FindProperty("threshold");
			_bandMinFreq = serializedObject.FindProperty("bandMinFreq");
			_bandMaxFreq = serializedObject.FindProperty("bandMaxFreq");
			_absoluteMaxFreq = serializedObject.FindProperty("absoluteMaxFreq");
			_useFreqRange = serializedObject.FindProperty("useFreqRange");
			_intensityModifier = serializedObject.FindProperty("intensityModifier");
			sceneObject = !EditorUtility.IsPersistent(this);
		}
		
		public override void OnInspectorGUI() {
			serializedObject.Update();
			_useFreqRange.boolValue = EditorGUILayout.Toggle("Use frequency range", _useFreqRange.boolValue);
			EditorGUI.BeginDisabledGroup(_useFreqRange.boolValue == false);
				float fMin = _bandMinFreq.floatValue;
				float fMax = _bandMaxFreq.floatValue;
				_audioInputObject.objectReferenceValue = EditorGUILayout.ObjectField(" FFT Provider ",_audioInputObject.objectReferenceValue,typeof(GameObject),sceneObject);
				EditorGUILayout.PrefixLabel(" Trigger Frequency Range ");
				fMin = EditorGUILayout.FloatField(" Minimum (Hz) ",fMin);
				fMax = EditorGUILayout.FloatField(" Maximum (Hz) ",fMax);
				EditorGUILayout.MinMaxSlider(ref fMin,ref fMax,20.0f,_absoluteMaxFreq.floatValue/2);
			EditorGUI.EndDisabledGroup();
			_threshold.floatValue = EditorGUILayout.Slider(" Threshold ",_threshold.floatValue,0,1);
			_bandMinFreq.floatValue = fMin;
			_bandMaxFreq.floatValue = fMax;
			_intensityModifier.floatValue = EditorGUILayout.FloatField(" Intensity modifier ",_intensityModifier.floatValue);
			serializedObject.ApplyModifiedProperties();
		}
	}
}
