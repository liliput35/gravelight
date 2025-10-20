using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCam == null) return;

        // Make the UI face the camera
        transform.LookAt(transform.position + _mainCam.transform.rotation * Vector3.forward,
                         _mainCam.transform.rotation * Vector3.up);
    }
}
