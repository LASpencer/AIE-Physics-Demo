using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    RagdollManager ragdoll;

	// Use this for initialization
	void Start () {
        ragdoll = GetComponent<RagdollManager>();
        ragdoll.setRagdollActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
