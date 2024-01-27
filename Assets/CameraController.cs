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

    public float horizontalLookSensitivity = 1f;

    public float verticalLookSensitivity = 1f;

    public float maxTilt = 70f;

    public float minTilt = 1f;

    //private GameObject followPoint;
    
    private CinemachineVirtualCamera VirtualCam;

    private bool hasApplicationFocus = true;
    
    // Start is called before the first frame update
    void Awake()
    {
        VirtualCam = GetComponent<CinemachineVirtualCamera>();

        VirtualCam.LookAt = Target.transform;
        VirtualCam.Follow = CameraTilt.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRigRoot.transform.position = Target.transform.position;
     
        if (!hasApplicationFocus)
        {
            return;
        }
        
        float xDelta = horizontalLookSensitivity * Input.GetAxis("Mouse X");
        float yDelta = verticalLookSensitivity * Input.GetAxis("Mouse Y");
        
        CameraRotate.transform.Rotate(Vector3.up, xDelta);
        
        CameraTilt.transform.Rotate(Vector3.right, -yDelta);
        
        // Clamping the tilt
        var angles = CameraTilt.transform.localRotation.eulerAngles;
        
        if (yDelta > 0)
        {
            if (angles.x > 330f)
            {
                angles.x = minTilt;
            }
            angles.x = Mathf.Max(angles.x, minTilt);
        }

        if (yDelta < 0)
        {
            angles.x = Mathf.Min(angles.x, maxTilt);
        }

        angles.y = 0f;
        angles.z = 0f;
        CameraTilt.transform.localEulerAngles = angles;
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            hasApplicationFocus = true;
        }
        else
        {
            hasApplicationFocus = false;
        }
    }
}
