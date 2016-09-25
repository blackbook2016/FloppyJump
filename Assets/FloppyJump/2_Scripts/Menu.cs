using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour 
{
	void Update () 
	{
		if (Input.GetKeyDown ("up")) 
			SceneManager.LoadScene ("Scene_1");
	}
}
