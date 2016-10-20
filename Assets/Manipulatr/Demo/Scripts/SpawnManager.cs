using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	public static Component Instance = null;

	public List<GameObject> SpawnableObjects;
	public List<Vector3> SpawnRotations;

	private List<GameObject> m_spawnedObjects = new List<GameObject>();

	void Awake () {
		if (Instance == null)
			Instance = this;
		else
			Destroy(this);
	}

	void Start () {
		float frq = 2000/12;
		Vector3 altScale = Vector3.one;
		altScale[0] = 1f;
		altScale[1] = 100f;
		altScale[2] = 1f;
		for (int i = 0; i < 12; i++)
		{
			AudioAnalyzer.ManipulateByAudio m;
			GameObject g = (GameObject)Instantiate(SpawnableObjects[0],Vector3.zero,Quaternion.identity);
			g.transform.eulerAngles = SpawnRotations[i];
			m = g.GetComponent<AudioAnalyzer.ManipulateByAudio>();
			m.useFreqRange = true;
			m.bandMinFreq = 80 + frq * i;
			m.bandMaxFreq = 80 + frq * (i+1);
			if (i > 2)
			{
				m.scaleModifier = altScale;
			}
			m_spawnedObjects.Add(g);
		}
	}
}


