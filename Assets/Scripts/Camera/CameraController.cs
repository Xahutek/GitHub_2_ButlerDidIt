using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Camera cam;

    public Vector2 Target
    {
        get
        {
            Vector2 pos = Locus.groundPosition;
            if (DialogueManager.isOpen) pos= dialogueUI.Locus;
            if (InventoryUI.isOpen) pos += Vector2.left * 1.2f;
            pos += Vector2.up * 1.75f;
            return pos;
        }
    }

    [SerializeField] Transform CameraHandler;

    [SerializeField]private float camSpeed;
    [SerializeField] private PlayerController Locus;

    DialogueManager dialogueUI;
    InventoryUI inventoryUI;

    private void Start()
    {
        cam = Camera.main;
        dialogueUI = DialogueManager.main;
        inventoryUI = InventoryUI.main;
    }

    public void FixedUpdate()
    {
                AdjustCameraPosition();
    }
    public void AdjustCameraPosition()
    {
        Vector3 pos = Target;
        if (!Locus || (Vector2)CameraHandler.position == (Vector2)pos)
        {
            return;
        }

        Vector2 distance = (Vector2)pos - (Vector2)CameraHandler.position;
        Vector2 movement = distance.normalized * camSpeed * Time.fixedDeltaTime;

        if (movement.magnitude > distance.magnitude)
            CameraHandler.position = pos;
        else
            CameraHandler.position += (Vector3)movement;
    }
}
