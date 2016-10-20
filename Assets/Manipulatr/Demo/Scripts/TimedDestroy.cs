using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,3.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
