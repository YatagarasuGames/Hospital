using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerCamera : NetworkBehaviour
{
    private PlayerMovement character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            character.gameObject.transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        }
        else
        {
            GetComponent<Camera>().enabled = false;
        }
    }
}
