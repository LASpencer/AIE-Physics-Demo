using System.Collections;
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

        if (Input.GetButtonDown("Fire1"))
        {
            m_movingCharacter.SetCrouching(!m_movingCharacter.Crouched);
            m_animator.SetBool("Crouching", m_movingCharacter.Crouched);
            // TODO can't crouch in jump, on ungrounding, uncrouch
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
