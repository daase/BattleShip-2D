using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    private float rotationSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -1), rotationSpeed * Time.deltaTime);
    }
}
