using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public Image fade;
	public float fadeTimer = 5;

	private bool changeSceneStarted = false;

	private Color c;
	private float timerStart;

	void Start()
	{
		c = fade.color;
		c.a = 0;
		fade.color = c;
	}

	void Update ()
	{
		if (!changeSceneStarted && Input.GetKeyDown ("up")) 
		{
			timerStart = Time.time;
			changeSceneStarted = true;
		}
		else if(changeSceneStarted)
		{
			c.a =  ((Time.time - timerStart) / fadeTimer);
			fade.color = c;

			if (c.a >= 1)
				SceneManager.LoadScene ("Scene_1");				
		}
	}
}
