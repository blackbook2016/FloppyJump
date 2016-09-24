using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Floppy : MonoBehaviour 
{
	public float jumpForce = 1;
	private Rigidbody2D rb;
	private Renderer renderer;

	public bool isGrounded = false;
	void Start () 
	{
		renderer = GetComponent<Renderer> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () 
	{    
		CheckStatus ();
		if (Input.GetKeyDown ("up")) 
		{
			if (isGrounded)
				Jump ();
			else
				FallDown ();
		}
	}

	private void CheckStatus()
	{
		if (rb.velocity.y <= 0) 
		{
			Vector3 bottom_left = transform.position + renderer.bounds.min;
			Vector3 bottom_right = bottom_left;
			bottom_right.x *= -1;

			isGrounded = Physics2D.OverlapArea (bottom_left, bottom_right);
		}
	}

	private void Jump()
	{
		rb.AddForce (Vector2.up * 100 * jumpForce);
		isGrounded = false;
	}

	private void FallDown()
	{
		rb.velocity = Vector3.zero;
		rb.AddForce (Vector2.down * 100 * jumpForce);	
	}
}
