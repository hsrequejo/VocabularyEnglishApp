using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    public Transform cameraTransform;
    private Transform cameraDesiredTransform;

    private void Update()
    {
        if (cameraDesiredTransform != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredTransform.rotation, 6 * Time.deltaTime);
        }
    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredTransform = menuTransform;
    }

}
