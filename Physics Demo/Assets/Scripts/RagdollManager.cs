using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour {

    GameObject Root;
    List<Rigidbody> RagdollBodies;

    private void Awake()
    {
        RagdollBodies = new List<Rigidbody>(Root.GetComponentsInChildren<Rigidbody>());
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setRagdollActive(bool value)
    {
        foreach(Rigidbody body in RagdollBodies)
        {
            body.isKinematic = !value;
            // HACK maybe also turn colliders on and off?
        }
    }
}
