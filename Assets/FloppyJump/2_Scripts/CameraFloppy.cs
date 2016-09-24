using UnityEngine;
using System.Collections;

public class CameraFloppy : MonoBehaviour 
{
	public GameObject player;
	public float followSpeed = 1;

	private Vector3 offset;

	void Start () 
	{
		player = GameObject.FindWithTag("Player");
		offset = transform.position;
	}

	void LateUpdate () 
	{
		transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * followSpeed);
	}
}
