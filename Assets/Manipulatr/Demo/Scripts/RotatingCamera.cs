using UnityEngine;
using System.Collections;

public class RotatingCamera : MonoBehaviour {

	public Transform Target;
	public float SpeedModifier;

	// Use this for initialization
	void Start () {
		transform.LookAt(Target.position);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (Target.position,new Vector3(1.0f,1.0f,0.0f),20 * Time.deltaTime * SpeedModifier);

	}
}
