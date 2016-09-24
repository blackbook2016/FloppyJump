using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody2D))]
public class Floppy : MonoBehaviour 
{
	#region Properties
	public float jumpForce = 100;
	public float falldownSpeed = 1.5f;
	public float swingSpeed = 1.5f;

	private bool isGrounded = false;
	public bool InfiniteJump = false;
	public bool doubleJump = false;
	private bool djDone = false;

	private Rigidbody2D rb;
	private MeshRenderer rend;
	private Collider2D groundedObj;

	int layerMask = 1 << 8;

	private Vector3 origin;
	private Vector3 size;


	#endregion

	#region Unity
	void OnDrawGizmosSelected() 
	{
		rend = GetComponent<MeshRenderer> ();

		origin = transform.position;
		origin.y -= rend.bounds.size.y / 2;

		Gizmos.color = new Color(1, 0, 0, 0.5F);

		Gizmos.DrawCube(origin, size);
	}

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		rend = GetComponent<MeshRenderer> ();

		size = rend.bounds.size;
		size.x -= 0.1f;
		size.y = 0.50f;
	}

	void FixedUpdate () 
	{    
		if (Input.GetKeyDown ("up")) 
		{
			if (InfiniteJump || isGrounded)
				Jump ();
			else if (doubleJump && !djDone) 
			{
				djDone = true;
				Jump ();
			}
		}

		if (Input.GetKeyDown ("down") && !isGrounded) 
			Fall ();			

		Swing ();
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(!isGrounded)
			CheckIsGrounded (coll.collider);
	}

	void OnCollisionStay2D(Collision2D coll) 
	{
		if(!isGrounded)
			CheckIsGrounded (coll.collider);
	}

	void OnCollisionExit2D(Collision2D coll) 
	{
		if (coll.collider == groundedObj) 
		{
			isGrounded = false;
			djDone = false;
		}
	}
	#endregion


	private void Jump()
	{
		Vector3 vel = rb.velocity;
		vel.y = 0;
		rb.velocity = vel;
		rb.AddForce (Vector2.up * jumpForce);
	}

	private void Fall()
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

	private void CheckIsGrounded(Collider2D coll)
	{
		origin = transform.position;
		origin.y -= rend.bounds.size.y / 4;

		Collider2D detectedGroundObj = Physics2D.OverlapBox (origin, size, 0, ~layerMask);

		if (detectedGroundObj && coll == detectedGroundObj) 
		{
			groundedObj = coll;
			isGrounded = true;
//			print (rend.bounds.Intersects (detectedGroundObj.bounds) + "  " + detectedGroundObj);
		}
	}
}
