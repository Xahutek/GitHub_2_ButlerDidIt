using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraLocus : MonoBehaviour
{
    private PlayerController controller;
    [SerializeField] private bool moveCamOnUI;

    public Vector2 position
    {
        get
        {
            Vector2 pos = controller.groundPosition;

            //if (moveCamOnUI)
            //{
            //    pos = Vector2.Lerp(pos, DialogueManager.main.Locus, DialogueManager.main.openProgress);
            //    pos += Vector2.left * 1.2f * InventoryUI.openProgress;
            //}

            pos += Vector2.up * 1.75f;

            return pos;
        }
    }
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

}
