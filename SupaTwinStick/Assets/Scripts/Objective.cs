using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {

	public LayerMask collisionMask;
	public Material activeSkin;
	public Light objectiveLight ;
	Material objectiveSkin;
	public event System.Action OnActivated;
	bool activated = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			if (OnActivated != null && !activated) 
			{
				Activation ();
				OnActivated ();
				activated = true;
			}
		}
	}

	public void Activation (){
		GetComponent<Renderer>().material = activeSkin ;
		objectiveLight.color = activeSkin.color ;
		objectiveLight.range += 10;
	}
}
