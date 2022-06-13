using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    public Collider2D col;
    [SerializeField] private LayerMask LMPlayer;
    Vector2 direction;
    private float walkAngle;
    public void Update()
    {
        direction= new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        walkAngle = Vector2.SignedAngle(new Vector2(1, 0), direction);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            //if (Input.GetKey(KeyCode.W))
            //{
            //    col.enabled = true;
            //}
            //else
            //{
            //    col.enabled = !col.enabled;
            //}
            if (walkAngle <= 0||walkAngle==180)
            {
                col.enabled = false;
            }
            if (walkAngle > 20 && walkAngle < 170)
            {
                col.enabled = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //if (LMPlayer.Contains(other.gameObject))
        //{
        //    if (Input.GetKey(KeyCode.W))
        //        col.enabled = true;
        //    if (Input.GetKey(KeyCode.S))
        //        col.enabled = false;
        //}

        if (walkAngle <=0||walkAngle == 180)
        {
            col.enabled = false;
        }
        if (walkAngle > 20 &&walkAngle < 170)
        {
            col.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (LMPlayer.Contains(collision.gameObject))
        //{
        //    col.enabled = !col.enabled;
        //}
    }

}
