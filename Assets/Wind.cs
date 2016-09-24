using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	void OnTriggerStay(Collider other) {
		other.gameObject.GetComponent<Rigidbody>().AddForce (-Vector3.right * 100);
	}
}
