using UnityEditor;
using UnityEngine;
using System.Collections;

namespace AudioAnalyzer
{
	[CustomEditor(typeof(SpawnByFrequencyRange))]
	class SpawnByFrequencyRangeEditor : Editor {
		SerializedProperty _timeBetweenSpawns;
		SerializedProperty _threshold;
		SerializedProperty _audioInputObject;
		SerializedProperty _objectToSpawn;
		SerializedProperty _materialColor;
		SerializedProperty _bandMinFreq;
		SerializedProperty _bandMaxFreq;
		SerializedProperty _absoluteMaxFreq;
		SerializedProperty _loudnessAffectsScale;
		SerializedProperty _randomRotationAlongY;
		private bool sceneObject = false;
		
		public void OnEnable() {
			_audioInputObject = serializedObject.FindProperty("audioInputObject");
			_threshold = serializedObject.FindProperty("threshold");
			_timeBetweenSpawns = serializedObject.FindProperty("timeBetweenSpawns");
			_objectToSpawn = serializedObject.FindProperty("objectToSpawn");
			_materialColor = serializedObject.FindProperty("materialColor");
			_bandMinFreq = serializedObject.FindProperty("bandMinFreq");
			_bandMaxFreq = serializedObject.FindProperty("bandMaxFreq");
			_absoluteMaxFreq = serializedObject.FindProperty("absoluteMaxFreq");
			_loudnessAffectsScale = serializedObject.FindProperty("loudnessAffectsScale");
			_randomRotationAlongY = serializedObject.FindProperty("randomRotationAlongY");
			sceneObject = !EditorUtility.IsPersistent(this);
		}
		
		public override void OnInspectorGUI() {
			serializedObject.Update();
			float fMin = _bandMinFreq.floatValue;
			float fMax = _bandMaxFreq.floatValue;
			_audioInputObject.objectReferenceValue = EditorGUILayout.ObjectField(" FFT Provider ",_audioInputObject.objectReferenceValue,typeof(GameObject),sceneObject);
			EditorGUILayout.PrefixLabel(" Trigger Frequency Range ");
			fMin = EditorGUILayout.FloatField(" Minimum (Hz) ",fMin);
			fMax = EditorGUILayout.FloatField(" Maximum (Hz) ",fMax);
			EditorGUILayout.MinMaxSlider(ref fMin,ref fMax,20.0f,_absoluteMaxFreq.floatValue);
			_threshold.floatValue = EditorGUILayout.Slider(" Threshold ",_threshold.floatValue,0,1);
			_timeBetweenSpawns.floatValue = EditorGUILayout.Slider(" Time between spawns (s) ",_timeBetweenSpawns.floatValue,0,10);
			_loudnessAffectsScale.boolValue = EditorGUILayout.Toggle(" Loudness affects scale ",_loudnessAffectsScale.boolValue);
			_randomRotationAlongY.boolValue = EditorGUILayout.Toggle(" Randomize rotation along Y axis ",_randomRotationAlongY.boolValue);
			_objectToSpawn.objectReferenceValue = EditorGUILayout.ObjectField(" Object to spawn ",_objectToSpawn.objectReferenceValue,typeof(GameObject), sceneObject);
			_materialColor.colorValue = EditorGUILayout.ColorField(" Object color ",_materialColor.colorValue);
			_bandMinFreq.floatValue = fMin;
			_bandMaxFreq.floatValue = fMax;
			serializedObject.ApplyModifiedProperties();
		}
	}
}