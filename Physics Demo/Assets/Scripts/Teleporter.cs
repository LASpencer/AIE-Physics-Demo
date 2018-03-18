using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public TeleporterExit Exit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // If moving character enters, teleport them to exit
        MovingCharacter character = other.GetComponent<MovingCharacter>();
        if(character != null)
        {
            other.transform.position = Exit.transform.position;
            Exit.TeleportEffect.Play();
        }
    }
}
