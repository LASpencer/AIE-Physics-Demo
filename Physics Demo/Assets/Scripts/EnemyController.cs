using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public RagdollManager ragdoll;

    public Collider col;

    public Animator animator;

    public MovingCharacter m_movingCharacter;

	// Use this for initialization
	void Start () {
        ragdoll = GetComponent<RagdollManager>();
        ragdoll.setRagdollActive(false);

        col = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void kill()
    {
        col.enabled = false;
        //TODO disable animator, rigidbody, etc
        ragdoll.setRagdollActive(true);
    }
}
