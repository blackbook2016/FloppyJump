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

	public bool isGrounded = false;
	public bool InfiniteJump = false;
	public bool doubleJump = false;
	private bool djDone = false;

	private Rigidbody2D rb;
	private MeshRenderer rend;
	//	private Collider2D groundedObj;
	//	private TrailRenderer trail;

	public string up = "up";
	public string down = "down";
	public GameObject canvas;
	public GameObject imageWin;
	public GameObject explosion;
	public GameObject endexplosion;
	public GameObject killexplosion;

	private int layerMask = 1 << 8;

	private Vector3 origin;
	private Vector3 size;

	private Vector3 initPos;
	private float timeDelay = 0;

	public Image fade;
	public float fadeTimer = 5;
	private Color c;
	private float timerStart;
	private bool changeScene = false;
	private bool replayStarted = false;
	private float timerMenu = 0;


	private AudioSource audioSource;
	public AudioClip jumpSound;
	public AudioClip deathSound;
	public AudioClip winSound;
	#endregion

	#region Unity
	void OnDrawGizmosSelected() 
	{
		rend = GetComponent<MeshRenderer> ();
		origin = transform.position;
		origin.y = rend.bounds.min.y - 0.1f;

		size = rend.bounds.size;
		size.x += 0f;
		size.y += 0.1f;

		Gizmos.color = new Color(1, 0, 0, 0.5F);

		Gizmos.DrawCube(origin, size);
	}

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		rend = GetComponent<MeshRenderer> ();
		//		trail = GetComponent<TrailRenderer> ();
		audioSource = GetComponent<AudioSource> ();

		size = rend.bounds.size;
		size.x -= 0f;
		size.y = 0.50f;

		initPos = transform.position;
	}

	void Update ()
	{
		if (changeScene) 
		{
			c.a = ((Time.time - timerStart) / fadeTimer);
			fade.color = c;

			if (c.a >= 1) 
			{
				if (timerMenu >= 10) 
					SceneManager.LoadScene ("Scene_0");
				else if (SceneManager.GetActiveScene ().name == "Scene_0")
					SceneManager.LoadScene ("Scene_1");
				else if (SceneManager.GetActiveScene ().name == "Scene_1")
					SceneManager.LoadScene ("Scene_2");
				else if (SceneManager.GetActiveScene ().name == "Scene_2")
					SceneManager.LoadScene ("Scene_3");
				else if (SceneManager.GetActiveScene ().name == "Scene_3")
					SceneManager.LoadScene ("Scene_4");
				else if (SceneManager.GetActiveScene ().name == "Scene_4")
					SceneManager.LoadScene ("Scene_5");
				else
					SceneManager.LoadScene ("Scene_0");
			}
		} 
		else if(!replayStarted)
		{
			if (Input.GetKeyDown (up)) 
			{
				if (InfiniteJump || isGrounded)
					Jump ();
				else if (doubleJump && !djDone) 
				{
					djDone = true;
					Jump ();
				}
			}

			if (Input.GetKeyDown (down) && !isGrounded) 
			{
				Fall ();
				timerMenu = 0;
			}

			if(Input.GetKey (down))
			{
				timerMenu += Time.deltaTime;

				if (timerMenu >= 10) 
					GameOver ();
			}

			if (Input.GetKeyDown ("space"))
				GameOver ();
		}
	}

	void FixedUpdate () 
	{   
		if(!replayStarted && !changeScene)
			Swing ();

		CheckIsGrounded ();
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (!changeScene) 
		{
			if (coll.collider.tag == "KillZone" && !replayStarted)
				StartCoroutine ("Replay");
			if (coll.collider.tag == "EndZone")
				GameOver ();
		}
	}
	#endregion
	public void ExternalJumpDetected()
	{
		if (!changeScene && !replayStarted) 
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

	private void Jump()
	{
		Vector3 vel = rb.velocity;
		vel.y = 0;
		rb.velocity = vel;
		rb.AddForce (Vector2.up * jumpForce);

		Instantiate (explosion, rend.bounds.min, Quaternion.identity);
		audioSource.PlayOneShot(jumpSound);
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

	private void CheckIsGrounded()
	{
		origin = transform.position;
		origin.y = rend.bounds.min.y - 0.1f;

		size = rend.bounds.size;
		size.x += 0f;
		size.y += 0.1f;

		Collider2D detectedGroundObj = Physics2D.OverlapBox (origin, size, 0, ~layerMask);

		if (!isGrounded  && isGrounded != detectedGroundObj)
			djDone = false;

		//		groundedObj = detectedGroundObj;
		isGrounded = Physics2D.OverlapBox (origin, size, 0, ~layerMask);
		//		print (detectedGroundObj);
	}

	public IEnumerator Replay()
	{
		CameraFloppy cam = Camera.main.GetComponent<CameraFloppy> ();

		replayStarted = true;
		audioSource.PlayOneShot(deathSound);
		Instantiate (killexplosion, rend.bounds.min, Quaternion.identity);
		rend.enabled = false;

		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		rb.isKinematic = true;

		Com.LuisPedroFonseca.ProCamera2D.ProCamera2DShake.Instance.Shake ();

		yield return new WaitForSeconds(1);

		//		foreach (GameObject go in GameObject.FindGameObjectsWithTag ("Explosion")) 
		//			Destroy (go);

		if(canvas)
			canvas.SetActive(false);

		transform.position = initPos;
		transform.rotation = Quaternion.identity;

		timeDelay = Time.time;

		isGrounded = false;
		djDone = false;

		//		trail.Clear ();

		rb.isKinematic = false;

		if (cam)
			cam.Reset ();

		rend.enabled = true;

		replayStarted = false;
	}

	private void GameOver()
	{
		if (canvas && !changeScene) 
		{
			c = fade.color;
			c.a = 0;
			fade.color = c;

			changeScene = true;
			timerStart = Time.time;

			audioSource.PlayOneShot(winSound);

			if (timerMenu >= 10)
				imageWin.SetActive (false);
			else 
			{
	//			Com.LuisPedroFonseca.ProCamera2D.ProCamera2DShake.Instance.Shake ();
				if(endexplosion)
					Instantiate (endexplosion, rend.bounds.min, Quaternion.identity);
			}

			canvas.SetActive (true);
		}
	}
}
