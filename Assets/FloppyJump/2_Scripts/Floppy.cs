using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
	private TrailRenderer trail;

	public GameObject canvas;
	public GameObject explosion;
	public GameObject endexplosion;

	int layerMask = 1 << 8;

	private Vector3 origin;
	private Vector3 size;

	private Vector3 initPos;
	private float timeDelay = 0;

	public Image fade;
	public float fadeTimer = 5;
	private Color c;
	private float timerStart;
	private bool changeScene = false;
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
		trail = GetComponent<TrailRenderer> ();

		size = rend.bounds.size;
		size.x -= 0.1f;
		size.y = 0.50f;

		initPos = transform.position;
	}

	void Update ()
	{
		if (changeScene) 
		{
			c.a =  ((Time.time - timerStart) / fadeTimer);
			fade.color = c;

			if (c.a >= 1)
				SceneManager.LoadScene ("Menu_0");		
		}
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
	}

	void FixedUpdate () 
	{    
		

		if (Input.GetKeyDown ("down") && !isGrounded) 
			Fall ();			

		Swing ();
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(!isGrounded)
			CheckIsGrounded (coll.collider);

		if (coll.collider.tag == "KillZone")
			Replay ();
		if (coll.collider.tag == "EndZone")
			GameOver ();
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

		Instantiate (explosion, rend.bounds.min, Quaternion.identity);
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
		rb.AddForce(Vector3.right * Mathf.Cos (Time.time - timeDelay) * swingSpeed);
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

	public void Replay()
	{
		foreach (GameObject go in GameObject.FindGameObjectsWithTag ("Explosion")) 
			Destroy (go);
		
		Time.timeScale = 1;

		if(canvas)
			canvas.SetActive(false);

		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;

		transform.position = initPos;
		transform.rotation = Quaternion.identity;

		timeDelay = Time.time;

		isGrounded = false;
		djDone = false;

		trail.Clear ();

		CameraFloppy cam = Camera.main.GetComponent<CameraFloppy> ();
		if (cam)
			cam.Reset ();
	}

	private void GameOver()
	{
//		Time.timeScale = 0;

		if (canvas && !changeScene) 
		{
			c = fade.color;
			c.a = 0;
			fade.color = c;

			changeScene = true;
			timerStart = Time.time;

			canvas.SetActive (true);
			endexplosion.SetActive (true);
		}
	}
}
