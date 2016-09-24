using UnityEngine;
using System.Collections;

public class iA_Moving : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().AddForce (Vector3.right * 1);
	}
}
