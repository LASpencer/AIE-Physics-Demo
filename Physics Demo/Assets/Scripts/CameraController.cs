using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public PlayerController Player;

    Camera camera;

    public float distance;
    public Vector3 offset;
    public float panRate;
    [Tooltip("As proportion of screen width")]
    public float panMargin;

    public float tiltRate;
    public float minTilt;
    public float maxTilt;
    [Tooltip("As proportion of screen height")]
    public float tiltMargin;

	// Use this for initialization
	void Start () {
        camera = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        Vector3 mousePosition = camera.ScreenToViewportPoint(Input.mousePosition);
        // TODO maybe check mouse inside screen?
        Vector3 newOrientation = transform.eulerAngles;
        if(newOrientation.x > 180)
        {
            // Treating reflex angles as negative to make clamping simpler
            newOrientation.x -= 360;
        }

        if (mousePosition.x >= 0 && mousePosition.x <= 1)
        {
            if (mousePosition.x < panMargin)
            {
                // Rotate left
                newOrientation -= Vector3.up * panRate * Time.deltaTime;

            }
            else if (mousePosition.x > 1.0f - panMargin)
            {
                // Rotate right
                newOrientation += Vector3.up * panRate * Time.deltaTime;
            }
        }

        if (mousePosition.y >= 0 && mousePosition.y <= 1)
        {
            if (mousePosition.y < tiltMargin)
            {
                // Tilt up
                newOrientation += Vector3.right * tiltRate * Time.deltaTime;

            }
            else if (mousePosition.y > 1.0f - tiltMargin)
            {
                // tilt down
                newOrientation -= Vector3.right * tiltRate * Time.deltaTime;
            }
        }
        newOrientation.x = Mathf.Clamp(newOrientation.x, minTilt, maxTilt);
        transform.eulerAngles = newOrientation;

        

        transform.position = Player.transform.position + offset - transform.forward * distance;
    }
}
