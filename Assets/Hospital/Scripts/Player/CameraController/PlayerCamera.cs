using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerCamera : InputComponent
{
    [SerializeField] private GameObject character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    [SyncVar(hook = nameof(OnRotationChanged))]
    private Vector2 syncVelocity;

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2);
            print(sensitivity);
        }
            
        else sensitivity = 2f;

        Cursor.lockState = CursorLockMode.Locked;
        if (!isOwned) GetComponent<Camera>().enabled = false;
    }



    public void SettingsChanges()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    void Update()
    {
        if (!isOwned || !_isActive) return;

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -70, 70);
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        if (isClient)
        {
            CmdUpdateRotation(velocity);
        }
    }

    [Command]
    private void CmdUpdateRotation(Vector2 newVelocity)
    {
        syncVelocity = newVelocity;
    }

    private void OnRotationChanged(Vector2 oldValue, Vector2 newValue)
    {
        if (!isOwned)
        {
            transform.localRotation = Quaternion.AngleAxis(-newValue.y, Vector3.right);
            character.transform.localRotation = Quaternion.AngleAxis(newValue.x, Vector3.up);
        }
    }

}
