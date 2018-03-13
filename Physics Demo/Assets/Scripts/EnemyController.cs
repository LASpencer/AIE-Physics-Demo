using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public RagdollManager ragdoll;

    public Collider col;

    public Animator animator;

    public MovingCharacter m_movingCharacter;

    [Header("Combat")]
    public int Health;
    public bool Alive;

	// Use this for initialization
	void Start () {
        ragdoll = GetComponent<RagdollManager>();
        ragdoll.setRagdollActive(false);

        col = GetComponent<Collider>();

        Alive = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot(RagdollJoint joint, int damage, Vector3 impactForce, Vector3 point)
    {
        // TODO particle effect where hit
        Health = Mathf.Max(Health - damage, 0);
        Debug.Log(string.Format("Shot for {0} damage", damage));
        if(Health == 0)
        {
            col.enabled = false;
            m_movingCharacter.enabled = false;
            m_movingCharacter.rigidbody.isKinematic = true;
            //TODO disable animator, rigidbody, etc
            ragdoll.setRagdollActive(true);
            Debug.Log(string.Format("Killed, force of {0} applied at {1}", impactForce, point));
            joint.Body.AddForceAtPosition(impactForce, point, ForceMode.Impulse);
        }
    }

    public void kill()
    {
        
    }
}
