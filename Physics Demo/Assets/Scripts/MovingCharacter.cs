using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovingCharacter : MonoBehaviour {

    const float MIN_GROUND_SPEED = 0.1f;
    const int FRAMES_UNTIL_FREEZE = 5;
    const float GROUND_CHECK_OFFSET = 0.3f;
    const float GROUND_CHECK_DISTANCE = 0.4f;

    Rigidbody m_rigidbody;
    new public Rigidbody rigidbody { get { return m_rigidbody; } }

    [SerializeField]
    CapsuleCollider m_collider;

    float m_capsuleRadius, m_capsuleHeight;
    Vector3 m_capsuleCenter;

    bool m_crouched;
    public bool Crouched { get { return m_crouched; } }

    [SerializeField]
    LayerMask crouchCheckMask;

    [Header("Movement")]
    public float GroundForce;
    public float AirForce;
    public float BrakingForce;

    public float JumpVelocity;
    public float RotationSpeed;


    [Header("Crouching")]
    [SerializeField]
    float m_crouchCapsuleRadius;
    [SerializeField]
    float m_crouchCapsuleHeight;
    [SerializeField]
    Vector3 m_crouchCapsuleCenter;

    Vector3 m_groundNormal;
    bool m_grounded;
    public bool IsGrounded { get { return m_grounded; } }

    int framesStationary = 0;

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
        framesStationary = 0;

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
    }

    /// <summary>
    /// Makes character move along ground at given velocity
    /// </summary>
    /// <param name="velocity"></param>
    public void SetGroundVelocity(Vector3 velocity)
    {
        // TODO if ground normal not too steep (count as (0,1,0) in air), project move onto ground and set velocity to move along it
        Vector3 groundVelocity = Vector3.ProjectOnPlane(velocity, m_groundNormal);
        bool stopping = false;

        Vector3 difference = groundVelocity - m_rigidbody.velocity;

        float acceleration;
        if (m_grounded)
        {
            float frictionRatio = 0.0f;  // How much braking force to apply
            if (groundVelocity == Vector3.zero)
            {
                // Coming to a stop
                frictionRatio = 1.0f;
                stopping = true;
                // Force stop if moving slowly
                if (m_rigidbody.velocity.sqrMagnitude < MIN_GROUND_SPEED * MIN_GROUND_SPEED)
                {
                    if (framesStationary >= FRAMES_UNTIL_FREEZE)
                    {
                        m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                        m_rigidbody.velocity = Vector3.zero;
                        difference = Vector3.zero;
                    }
                    else
                    {
                        framesStationary++;
                    }
                }
                else
                {
                    UnfreezeMovement();
                }

            }
            else
            {
                UnfreezeMovement();
                if (m_rigidbody.velocity != Vector3.zero)
                {
                    // Braking based by how far off actual velocity
                    float cosAngle = Vector3.Dot(m_rigidbody.velocity.normalized, groundVelocity.normalized);
                    frictionRatio = 0.5f - 0.5f * cosAngle;
                }
            }
            acceleration = GroundForce + frictionRatio * BrakingForce;
        } else
        {
            acceleration = AirForce;
            UnfreezeMovement();
        }

        Vector3 deltaV = Vector3.ClampMagnitude(difference, acceleration * Time.fixedDeltaTime);

        m_rigidbody.AddForce(deltaV, ForceMode.VelocityChange);

        // TODO turn rigidbody towards desired direction

        if (!stopping) {
            RotateTowards(m_rigidbody.velocity);
        }
        // Maybe if facing and desired velocity too far apart, move less while turning?



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
            UnfreezeMovement();
            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, JumpVelocity, m_rigidbody.velocity.z);
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
            UnfreezeMovement();
            m_crouched = true;
            m_collider.height = m_crouchCapsuleHeight;
            m_collider.radius = m_crouchCapsuleRadius;
            m_collider.center = m_crouchCapsuleCenter;
            return true;
        } else
        {
            if (!Physics.SphereCast(new Ray(transform.position, Vector3.up), m_capsuleRadius, m_capsuleHeight - m_capsuleRadius, crouchCheckMask))
            {
                // TODO on uncrouching, check enough headroom. If not, stay crouched and return false
                // Use a spherecast matching the standing capsule to check going to stand won't collide
                UnfreezeMovement();
                m_crouched = false;
                m_collider.height = m_capsuleHeight;
                m_collider.radius = m_capsuleRadius;
                m_collider.center = m_capsuleCenter;
                return true;
            } else
            {
                return false;
            }
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
            // TODO check if standing on moving platform, if so treat as 
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

    [ExecuteInEditMode]
    private void OnValidate()
    {
        GroundForce = Mathf.Abs(GroundForce);
        AirForce = Mathf.Abs(AirForce);
        BrakingForce = Mathf.Abs(BrakingForce);

        JumpVelocity = Mathf.Abs(JumpVelocity);
        RotationSpeed = Mathf.Abs(RotationSpeed);

        m_crouchCapsuleRadius = Mathf.Abs(m_crouchCapsuleRadius);
        m_crouchCapsuleHeight = Mathf.Abs(m_crouchCapsuleHeight);
    }

    private void UnfreezeMovement()
    {
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        framesStationary = 0;
    }

}
