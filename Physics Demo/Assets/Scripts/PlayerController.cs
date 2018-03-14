﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingCharacter))]
public class PlayerController : MonoBehaviour {

    MovingCharacter m_movingCharacter;

    public Animator m_animator;

    public CameraController cam;

    public float Speed;
    //TODO crouching move speed
    Vector3 m_movementInput;
    bool m_jump;

    [Header("Combat")]
    [SerializeField]
    LayerMask ShootMask;
    [SerializeField]
    Transform ShotOrigin;
    public GameObject ShotLine;
    public GameObject SmokeEffect;
    public float LaserEffectTime;
    public int Damage;
    public int HeadshotDamage;
    public float ShotRange;
    public float ShotForce;

	// Use this for initialization
	void Start () {
        m_movingCharacter = gameObject.GetComponent<MovingCharacter>();
        cam.Player = this;
	}
	
	// Update is called once per frame
	void Update () {

        // Get forward and right direction on x-z plane relative to camera
        Vector3 cameraForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
        Vector3 cameraRight = new Vector3(cameraForward.z, 0, -cameraForward.x);
        Vector3.OrthoNormalize(ref cameraForward, ref cameraRight);

        m_movementInput = cameraForward * Input.GetAxis("Vertical") + cameraRight * Input.GetAxis("Horizontal");
        m_movementInput *= Speed;

        // HACK maybe move out to fixed update
        //m_movingCharacter.SetGroundVelocity(m_movementInput);

        // HACK get actual speed, not desired
        //m_animator.SetFloat("Speed", m_movementInput.magnitude);

        if (Input.GetButtonDown("Jump"))
        {
            m_jump = true;
            //if (m_movingCharacter.Jump())
            //{
            //    m_animator.SetTrigger("Jumped");
            //}
        }

        if (Input.GetButtonDown("Crouch"))
        {
            m_movingCharacter.SetCrouching(!m_movingCharacter.Crouched);
            m_animator.SetBool("Crouching", m_movingCharacter.Crouched);
            // TODO can't crouch in jump, on ungrounding, uncrouch
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            // TODO figure out layers, mask
            if(Physics.Raycast(clickRay, out hit, float.PositiveInfinity, ShootMask, QueryTriggerInteraction.Collide))
            {
                // second raycast from head to clicked position to check laser can hit
                Ray shotRay = new Ray(ShotOrigin.position, hit.point - ShotOrigin.position);
                if (Physics.Raycast(shotRay, out hit, ShotRange, ShootMask, QueryTriggerInteraction.Collide))
                {

                    // draw beam being shot
                    GameObject laser = Instantiate(ShotLine);
                    LineRenderer laserLine = laser.GetComponent<LineRenderer>();
                    laserLine.SetPosition(0, ShotOrigin.position);
                    laserLine.SetPosition(1, hit.point);
                    Destroy(laser, LaserEffectTime);

                    //TODO use tags, etc to make decision
                    RagdollJoint dollJoint = hit.collider.GetComponent<RagdollJoint>();
                    EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();
                    if (enemy != null && dollJoint != null)
                    {
                        int shotDamage = Damage;
                        if (dollJoint.Part == RagdollPart.Head)
                        {
                            shotDamage = HeadshotDamage;
                            Debug.Log("Headshot on enemy");
                        }
                        else
                        {
                            Debug.Log("Shot enemy");
                        }
                        // TODO check is 
                        Vector3 direction = hit.point - ShotOrigin.position;

                        enemy.Shoot(dollJoint, shotDamage, direction.normalized * ShotForce, hit.point);
                    } 
                    // If nothing hit, just put smoke effect
                    GameObject smoke = Instantiate(SmokeEffect);
                    smoke.transform.position = hit.point;
                    smoke.transform.eulerAngles = new Vector3(-90, 0, 0);
                    Destroy(smoke, 5);
                    
                }
            }
        }
    }

    private void FixedUpdate()
    {
        m_animator.SetBool("Grounded", m_movingCharacter.IsGrounded);

        m_movingCharacter.SetGroundVelocity(m_movementInput);

        // HACK get actual speed, not desired
        m_animator.SetFloat("Speed", m_movingCharacter.rigidbody.velocity.magnitude);

        if (m_jump)
        {
            if (m_movingCharacter.Jump())
            {
                m_animator.SetTrigger("Jumped");
            }
        }
        m_jump = false;
    }
}
