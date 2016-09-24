using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

	public float speed = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (speed * 0, Time.deltaTime, 0);
	}
}
