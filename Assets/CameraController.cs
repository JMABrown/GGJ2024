using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    public GameObject Target;

    public GameObject CameraRigRoot;

    public GameObject CameraRotate;

    public GameObject CameraTilt;

    public float horizontalSpeed = 1f;

    public float verticalSpeed = 1f;

    //private GameObject followPoint;
    
    private CinemachineVirtualCamera VirtualCam;
    
    // Start is called before the first frame update
    void Awake()
    {
        VirtualCam = GetComponent<CinemachineVirtualCamera>();

        //followPoint = new GameObject("Virtual Target");

        VirtualCam.LookAt = Target.transform;
        VirtualCam.Follow = CameraRigRoot.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRigRoot.transform.position = Target.transform.position;
        
        float xDelta = horizontalSpeed * Input.GetAxis("Mouse X");
        float yDelta = verticalSpeed * Input.GetAxis("Mouse Y");
        
        //followPoint.transform.Rotate(yDelta, xDelta, 0);
        //followPoint.transform.Rotate(yDelta, 0, 0);
        //followPoint.transform.Rotate(0, xDelta, 0);
        CameraRotate.transform.Rotate(Vector3.up, xDelta);
        CameraTilt.transform.Rotate(Vector3.right, yDelta);
        
    }
}
