using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockedWall : MonoBehaviour
{
    public bool available;
    public Vector2 AvailableHours = new Vector2(0, 24);
    public string LockMessage;
    public BoxCollider2D col;

    private void FixedUpdate()
    {
        available = Clock.Hour > AvailableHours.x && Clock.Hour < AvailableHours.y;
        col.enabled = !available;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Comment.main.ShowComment(LockMessage, 3, 0);
    }
}
