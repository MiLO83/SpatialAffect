using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public int frameCount = 0;
    //public float speed = 0.00001f; // Controls camera movement speed
    //public float sensitivity = 0.00001f; // Controls mouse sensitivity for camera rotation
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        //if (frameCount++ < 2048)
        {
            // Move the camera forward, backward, left, and right
            //transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
            //transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            //float xRot = speed * Input.GetAxis("Mouse X") * sensitivity;
            //float yRot = speed * Input.GetAxis("Mouse Y") * sensitivity;
            //transform.Rotate(360f / (60f * 30f), 360f / (60f * 30f), 360f / (60f * 30f));
            transform.Rotate(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        }
    }
}
