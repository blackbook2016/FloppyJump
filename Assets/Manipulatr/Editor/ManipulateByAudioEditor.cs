using UnityEditor;
using UnityEngine;
using System.Collections;

namespace AudioAnalyzer
{
	[CustomEditor(typeof(ManipulateByAudio))]
	public class ManipulateByAudioEditor : Editor {
		SerializedProperty _threshold;
		SerializedProperty _audioInputObject;
		SerializedProperty _materialColor;
		SerializedProperty _bandMinFreq;
		SerializedProperty _bandMaxFreq;
		SerializedProperty _absoluteMaxFreq;
		SerializedProperty _affectTransform;
		SerializedProperty _affectRotation;
		SerializedProperty _affectScale;
		SerializedProperty _useFreqRange;
		SerializedProperty _transformModifier;
		SerializedProperty _rotationAngleModifier;
		SerializedProperty _scaleModifier;
		SerializedProperty _scaleDecay;
		SerializedProperty _scaleDecayFactor;
		private bool sceneObject = false;
		
		public void OnEnable() {
			_audioInputObject = serializedObject.FindProperty("audioInputObject");
			_threshold = serializedObject.FindProperty("threshold");
			_bandMinFreq = serializedObject.FindProperty("bandMinFreq");
			_bandMaxFreq = serializedObject.FindProperty("bandMaxFreq");
			_absoluteMaxFreq = serializedObject.FindProperty("absoluteMaxFreq");
			_affectTransform = serializedObject.FindProperty("affectTransform");
			_affectRotation = serializedObject.FindProperty("affectRotation");
			_affectScale = serializedObject.FindProperty("affectScale");
			_useFreqRange = serializedObject.FindProperty("useFreqRange");
			_transformModifier = serializedObject.FindProperty("transformModifier");
			_rotationAngleModifier = serializedObject.FindProperty("rotationAngleModifier");
			_scaleModifier = serializedObject.FindProperty("scaleModifier");
			_scaleDecay = serializedObject.FindProperty("scaleDecay");
			_scaleDecayFactor = serializedObject.FindProperty("sDamp");
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
			_affectTransform.boolValue = EditorGUILayout.Toggle("Affect position", _affectTransform.boolValue);
			EditorGUI.BeginDisabledGroup(_affectTransform.boolValue == false);
				_transformModifier.vector3Value = EditorGUILayout.Vector3Field("Position modifiers",_transformModifier.vector3Value);
			EditorGUI.EndDisabledGroup();
			_affectRotation.boolValue = EditorGUILayout.Toggle("Affect rotation", _affectRotation.boolValue);
			EditorGUI.BeginDisabledGroup(_affectRotation.boolValue == false);
				_rotationAngleModifier.vector3Value = EditorGUILayout.Vector3Field("Rotation modifiers",_rotationAngleModifier.vector3Value);
			EditorGUI.EndDisabledGroup();
			_affectScale.boolValue = EditorGUILayout.Toggle("Affect scale", _affectScale.boolValue);
			EditorGUI.BeginDisabledGroup(_affectScale.boolValue == false);
				_scaleModifier.vector3Value = EditorGUILayout.Vector3Field("Scale modifiers",_scaleModifier.vector3Value);
			EditorGUI.EndDisabledGroup();
			_scaleDecay.boolValue = EditorGUILayout.Toggle("Enable scale decay", _scaleDecay.boolValue);
			EditorGUI.BeginDisabledGroup(_scaleDecay.boolValue == false);
			_scaleDecayFactor.floatValue = EditorGUILayout.FloatField(" Scale decay factor",_scaleDecayFactor.floatValue);
			EditorGUI.EndDisabledGroup();
			serializedObject.ApplyModifiedProperties();
		}
	}
}