using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingCharacter))]
public class PlayerController : MonoBehaviour {

    MovingCharacter m_movingCharacter;

    public float Speed;

    Vector3 m_movementInput;

	// Use this for initialization
	void Start () {
        m_movingCharacter = gameObject.GetComponent<MovingCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
        m_movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        m_movementInput *= Speed;

        // HACK maybe move out to fixed update
        m_movingCharacter.SetGroundVelocity(m_movementInput);
    }
}
