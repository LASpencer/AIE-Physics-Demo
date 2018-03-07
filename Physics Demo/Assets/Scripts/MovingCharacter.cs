using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovingCharacter : MonoBehaviour {

    const float GROUND_CHECK_OFFSET = 0.3f;
    const float GROUND_CHECK_DISTANCE = 0.4f;

    Rigidbody m_rigidbody;

    [SerializeField]
    CapsuleCollider m_collider;

    float m_capsuleRadius, m_capsuleHeight;
    Vector3 m_capsuleCenter;

    bool m_crouched;
    public bool Crouched { get { return m_crouched; } }

    [SerializeField]
    float m_crouchCapsuleRadius;
    [SerializeField]
    float m_crouchCapsuleHeight;
    [SerializeField]
    Vector3 m_crouchCapsuleCenter;

    Vector3 m_groundNormal;
    bool m_grounded;
    public bool IsGrounded { get { return m_grounded; } }

    public float RotationSpeed;

    public float JumpForce;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
        // HACK figure out if this is correct/matters
        // Doing this so user could add multiple capsule colliders and set one, but if not just pick one
		if(m_collider == null)
        {
            m_collider = gameObject.GetComponent<CapsuleCollider>();
        }
        m_capsuleCenter = m_collider.center;
        m_capsuleHeight = m_collider.height;
        m_capsuleRadius = m_collider.radius;

        m_rigidbody = gameObject.GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        m_crouched = false;

        m_grounded = true;
        checkGround();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        checkGround();
        Debug.Log("Velocity: " + m_rigidbody.velocity.ToString());
    }

    /// <summary>
    /// Makes character move along ground at given velocity
    /// </summary>
    /// <param name="velocity"></param>
    public void SetGroundVelocity(Vector3 velocity)
    {
        // TODO if ground normal not too steep (count as (0,1,0) in air), project move onto ground and set velocity to move along it
        Vector3 groundVelocity = Vector3.ProjectOnPlane(velocity, m_groundNormal);
        // TODO turn rigidbody towards desired direction

        RotateTowards(groundVelocity);

        // Maybe if facing and desired velocity too far apart, move less while turning?

        //TODO maybe air control can be set at different levels from 0 (momentum only) to 1(treat like ground movement)

        // Move along desired velocity
        if (m_grounded)
        {
            m_rigidbody.velocity = groundVelocity;
        } else
        {
            // If not grounded, keep vertical component of velocity
            groundVelocity.y = m_rigidbody.velocity.y;
            m_rigidbody.velocity = groundVelocity;
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        // TODO set velocity to move given displacement (including y movement)
        // TODO turn rigidbody to face movement direction
        RotateTowards(velocity);


        m_rigidbody.velocity = velocity;
    }

    // TODO rotate without moving
    public void RotateTowards(Vector3 forward)
    {
        // TODO rotate front towards given direction
        forward.y = 0;
        if (forward != Vector3.zero)
        {
            Quaternion desiredDirection = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredDirection, RotationSpeed * Time.fixedDeltaTime);
        }
    }

    // TODO jump command
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns true if character could jump</returns>
    public bool Jump()
    {
        if (m_grounded)
        {
            // HACK try both adding and setting Y
            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, JumpForce, m_rigidbody.velocity.z);
            m_grounded = false;
            m_groundNormal = Vector3.up;
            return true;
        } else
        {
            return false;
        }
    }

    // TODO crouch/uncrouch function
    // Uncrouch needs to check nothing too high above
    public bool SetCrouching(bool crouch)
    {
        if(crouch == m_crouched)
        {
            return true;
        } else if (crouch)
        {
            // Crouching
            m_crouched = true;
            m_collider.height = m_crouchCapsuleHeight;
            m_collider.radius = m_crouchCapsuleRadius;
            m_collider.center = m_crouchCapsuleCenter;
            return true;
        } else
        {
            // TODO on uncrouching, check enough headroom. If not, stay crouched and return false
            m_crouched = false;
            m_collider.height = m_capsuleHeight;
            m_collider.radius = m_capsuleRadius;
            m_collider.center = m_capsuleCenter;
            return true;
        }
    }
    
    void checkGround()
    {
        if (m_grounded || m_rigidbody.velocity.y <= 0)      // If not grounded and moving up, won't ground
        {
            // checkGround to get grounded and ground normal
            RaycastHit groundTest;
            // TODO figure out layermask, whether querytriggerinteraction needs setting
            m_grounded = Physics.Raycast(transform.position + Vector3.up * GROUND_CHECK_OFFSET, Vector3.down, out groundTest, GROUND_CHECK_DISTANCE);
            Debug.DrawRay(transform.position + Vector3.up * GROUND_CHECK_OFFSET, Vector3.down, Color.red);
            Vector3 hitNormal = groundTest.normal;
            if (m_grounded)
            {
                m_groundNormal = groundTest.normal;
            }
            else
            {
                m_groundNormal = Vector3.up;
            }
        }
        else
        {
            m_groundNormal = Vector3.up;
        }

        //TODO more robust check (multiple rays, boxcast)
    }
}
