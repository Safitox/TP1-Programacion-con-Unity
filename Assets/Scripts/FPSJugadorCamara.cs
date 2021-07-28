using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSJugadorCamara : MonoBehaviour
{
    [SerializeField]
    Transform character;
    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    public float sensitivity = 1;
    public float smoothing = 2;
    public bool Activar = true;


    void Reset()
    {
        character = transform.parent;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Activar)
        {
            // Get smooth mouse look.
            Vector2 smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * sensitivity * smoothing);
            appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
            currentMouseLook += appliedMouseDelta;
            currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

            // Rotate camera and controller.
            transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
        }
    }
}
