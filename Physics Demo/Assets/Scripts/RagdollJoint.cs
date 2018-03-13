using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RagdollPart { Head, Chest, Pelvis, LArm, LForearm, RArm, RForearm, LThigh, LShin, RThigh, RShin};

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RagdollJoint : MonoBehaviour {

    [SerializeField]
    private RagdollPart part;
    public RagdollPart Part { get { return part; } }

    private Rigidbody m_body;
    public Rigidbody Body { get { return m_body; } }

    private Collider m_collider;
    new public Collider collider { get { return m_collider; } }

    private void Awake()
    {
        m_body = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetRagdollActive(bool value)
    {
        m_body.isKinematic = !value;
        m_collider.isTrigger = !value;
    }
}
