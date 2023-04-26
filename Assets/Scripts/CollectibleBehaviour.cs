using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    /// <summary> ID of the Collectible </summary>
    public string id;
    /// <summary> Default upright position of the Collectible on the display table </summary>
    private Vector3 initialPos;
    /// <summary> Default upright rotation of the Collectible on the display table </summary>
    private Quaternion initialRot;

    void Awake()
    {
        //reset position and rotation to default
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    private void Update()
    {
        //reset position, rotation and velocities to default
        if (transform.position.y < -5)
        {
            ResetTransform();
        }
    }

    public void ResetTransform()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
