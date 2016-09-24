using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody2D))]
public class Floppy : MonoBehaviour 
{
	public float jumpForce = 100;
	public float falldownSpeed = 1.5f;
	public float swingSpeed = 1.5f;
	public float gravity = 1.0f;

	public bool isGrounded = false;
	public bool InfiniteJump = false;

	private Rigidbody2D rb;
	private MeshRenderer renderer;

	public Vector3 moveDirection = Vector3.zero;

	int layerMask = 1 << 8;

	private List<Collider2D> colliders;

	void OnDrawGizmosSelected() 
	{
		renderer = GetComponent<MeshRenderer> ();

		Vector3 origin = transform.position;
		origin.y -= renderer.bounds.size.y / 2;

		Vector3 size = renderer.bounds.size;
		size.x -= 0.1f;

		Gizmos.color = new Color(1, 0, 0, 0.5F);

		Gizmos.DrawCube(origin, size);
	}

	void Start () 
	{
		renderer = GetComponent<MeshRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		colliders = new List<Collider2D> ();
	}

	void FixedUpdate () 
	{    
		CheckIsGrounded ();

		if (Input.GetKeyDown ("up")) 
		{
			if(InfiniteJump)
				Jump ();
			else if (isGrounded)
				Jump ();
		}

		if (Input.GetKeyDown ("down") && !isGrounded) 
		{
			FallDown ();			
		}

		Swing ();
	}

	private void Jump()
	{
		rb.AddForce (Vector2.up * jumpForce);
	}

	private void FallDown()
	{
		Vector3 vel = rb.velocity;
		vel.y = 0;
		rb.velocity = vel;
		rb.AddForce(Vector3.down * falldownSpeed);
	}

	private void Swing()
	{
		rb.AddForce(Vector3.right * Mathf.Cos (Time.time) * swingSpeed);
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (!colliders.Contains (coll.collider))
			colliders.Add (coll.collider);
	}
	void OnCollisionExit2D(Collision2D coll) 
	{
		colliders.Remove (coll.collider);
	}

	private void CheckIsGrounded()
	{
		Vector3 origin = transform.position;
		origin.y -= renderer.bounds.size.y / 2;

		Vector3 size = renderer.bounds.size;
		size.x -= 0.1f;

		Collider2D detectedGroundObj = Physics2D.OverlapBox (origin, size, 0, ~layerMask);

		isGrounded = colliders.Contains(detectedGroundObj);

//		print (isGrounded + "  " + Physics2D.OverlapBox (origin, size, 0, ~layerMask));
	}
}
