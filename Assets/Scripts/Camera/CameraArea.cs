using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    public GameObject cam;
    public bool active;

    [SerializeField] private LayerMask LMPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            active = true;
            cam.SetActive(active);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            active = false;
            cam.SetActive(active);
        }
    }
}
