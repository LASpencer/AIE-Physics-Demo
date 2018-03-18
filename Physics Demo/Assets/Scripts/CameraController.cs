using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public PlayerController Player;

    Camera camera;

    [Tooltip("Distance from camera to target")]
    public float distance;
    [Tooltip("Point relative to player position camera looks at")]
    public Vector3 offset;
    [Tooltip("Degrees per second camera pans left and right")]
    public float panRate;
    [Tooltip("Proportion of screen width where mouse makes camera pan")]
    public float panMargin;

    [Tooltip("Degrees per second camera tilts up and down")]
    public float tiltRate;
    public float minTilt;
    public float maxTilt;
    [Tooltip("Proportion of screen height where mouse makes camera tilt")]
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
        Vector3 newOrientation = transform.eulerAngles;
        if(newOrientation.x > 180)
        {
            // Treating reflex angles as negative to make clamping simpler
            newOrientation.x -= 360;
        }

        if (mousePosition.x >= 0 && mousePosition.x <= 1 && mousePosition.y >= 0 && mousePosition.y <= 1)
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
