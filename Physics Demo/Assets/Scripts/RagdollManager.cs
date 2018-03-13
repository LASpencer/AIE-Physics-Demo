using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour {

    public GameObject Root;
    public List<RagdollJoint> DollJoints;

    private void Awake()
    {
        DollJoints = new List<RagdollJoint>(Root.GetComponentsInChildren<RagdollJoint>());
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setRagdollActive(bool value)
    {
        foreach(RagdollJoint joint in DollJoints)
        {
            joint.SetRagdollActive(value);
        }
    }
}
