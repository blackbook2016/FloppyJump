using UnityEngine;
using System.Collections;

public class CameraFloppy : MonoBehaviour 
{
	public GameObject player;
	public float followSpeed = 1;

	public Vector3 offset;
	private float swingSpeed;

	void Start () 
	{
		player = GameObject.FindWithTag("Player");
		offset = transform.position - player.transform.position;
	}

	void FixedUpdate()
	{
		Vector3 followPos = transform.position;
		followPos.x = player.transform.position.x;
		transform.position = followPos;

		transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * followSpeed);
	}
}
