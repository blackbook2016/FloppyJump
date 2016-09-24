using UnityEngine;
using System.Collections;

public class Push : MonoBehaviour {

	public GameObject MissilePrefab ;
	public float MissileSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<Rigidbody>().AddForce (Vector3.up * 0);

		if (Input.GetKeyDown (KeyCode.UpArrow)){
			GetComponent<Rigidbody>().AddForce (Vector3.up * 65f, ForceMode.Impulse);
		}
		if (Input.GetKey (KeyCode.DownArrow)){
			GetComponent<Rigidbody>().AddForce (Vector3.up * -500);
		}
		if (Input.GetKeyDown (KeyCode.Space)){
			GameObject missile = Instantiate<GameObject> (MissilePrefab);
			missile.transform.position = transform.position +0.6f * Vector3.up;
			missile.GetComponent<MissileBehaviour> ().speed = MissileSpeed;
			//GetComponent<Rigidbody>().AddForce (-Vector3.up * 20);
			//GetComponent<Rigidbody>().AddForce (Vector3.right * 10f, ForceMode.Impulse);
			//GetComponent<Rigidbody>().AddForce (-Vector3.down * 5f, ForceMode.Impulse);
			}
	}
}
