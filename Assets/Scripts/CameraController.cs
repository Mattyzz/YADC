using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float lerpSpeed; // lerp allows for smooth movement

    //Limits how much the camera can follow the player
    Vector3 tempPosition;
    [SerializeField]
    float minX, minY, maxX, maxY;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null) return;
        tempPosition = target.position;
        tempPosition.z = -10;

        //Camera hits min position
        if (target.position.x < minX)
            tempPosition.x = minX;
        if (target.position.y < minY)
            tempPosition.y = minY;

        //Camera hits max position
        if (target.position.x > maxX)
            tempPosition.x = maxX;
        if (target.position.y > maxY)
            tempPosition.y = maxY;

        //Smooth movement of the camera
        transform.position = Vector3.Lerp(transform.position, tempPosition, lerpSpeed * Time.deltaTime);
    }
}
