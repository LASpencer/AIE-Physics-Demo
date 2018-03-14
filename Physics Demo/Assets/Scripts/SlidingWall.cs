using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlidingWallTarget
{
    public Vector3 position;
    public Vector3 velocity;
    public float duration;
}

[RequireComponent(typeof(ConfigurableJoint))]
public class SlidingWall : MonoBehaviour {

    public List<SlidingWallTarget> targets;
    public int targetIndex;
    float elapsedTime;
    ConfigurableJoint joint;

	// Use this for initialization
	void Start () {
        elapsedTime = 0;
        joint = GetComponent<ConfigurableJoint>();
        joint.targetPosition = targets[targetIndex].position;
        joint.targetVelocity = targets[targetIndex].velocity;
    }
	
	// Update is called once per frame
	void Update () {
        // TODO change target position x between 0 and -8
        elapsedTime += Time.deltaTime;
        while(elapsedTime >= targets[targetIndex].duration)
        {
            elapsedTime -= targets[targetIndex].duration;
            targetIndex = (targetIndex + 1) % targets.Count;
            joint.targetPosition = targets[targetIndex].position;
            joint.targetVelocity = targets[targetIndex].velocity;
        }
	}
}
