using UnityEngine;
using System.Collections;

namespace AudioAnalyzer
{
	[AddComponentMenu("Manipulatr/Spawnpoint, loudness trigger")]
	public class SpawnByLoudness : MonoBehaviour {
	
	    public GameObject audioInputObject;
	    public float threshold = 1.0f;
	    public GameObject objectToSpawn;
		public float timeBetweenSpawns = 1.0f;
		private float timeSinceLastSpawn = 0.0f;
		public bool loudnessAffectsScale = true;
		public float scaleModifier = 1.0f;
		public bool randomRotationAlongY = true;
		public bool affectColor = false;
		public Color materialColor = Color.white;
	    AudioAnalyzer audioIn;
	
	    void Start() {
	        if (objectToSpawn == null)
	            Debug.LogError("You need to set a prefab to Object To Spawn -parameter in the editor!");
	        if (audioInputObject == null)
	            audioInputObject = GameObject.Find("AudioAnalyzer");
			if (audioInputObject == null)
				Debug.LogError("You need to have a FFT Source. Try adding AudioAnalyzer -prefab to your scene!");
	        audioIn = (AudioAnalyzer) audioInputObject.GetComponent("AudioAnalyzer");
	    }
		
	    void Update () {
			timeSinceLastSpawn += Time.deltaTime;
	        float l = audioIn.loudness;
			if ( timeSinceLastSpawn >= timeBetweenSpawns )
			{
		        if (l > threshold)
		        {
					Vector3 scale = new Vector3(0.0f,0.0f,0.0f);
					if (loudnessAffectsScale)
					{
						scale.x = l;
						scale.y = l;
						scale.z = l;
					}
					Quaternion rotation = this.transform.rotation;
					if (randomRotationAlongY)
					{
						int angle = Random.Range(0,360);
						rotation.eulerAngles = new Vector3(0,angle,0);
					}					
					GameObject newObject = (GameObject)Instantiate(objectToSpawn, this.transform.position, rotation);
					newObject.transform.localScale += scale;
					newObject.transform.localScale *= scaleModifier;
					if (affectColor)
						newObject.GetComponent<Renderer>().material.color = this.materialColor;
					timeSinceLastSpawn = 0.0f;
		        }
			}
	   	}
	}
}
