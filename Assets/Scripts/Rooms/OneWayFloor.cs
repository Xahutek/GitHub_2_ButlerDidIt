using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayFloor : MonoBehaviour
{
    public Collider2D col;
    [SerializeField] private LayerMask LMPlayer;
    public bool allowPrePress=true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            Debug.Log("Enter");

            if (Input.GetKey(KeyCode.S)&&allowPrePress)
                col.enabled = false;
            else
                col.enabled = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("DOWN"); 
                col.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LMPlayer.Contains(collision.gameObject))
        {
            Debug.Log("exit");
            col.enabled = true;
        }
    }

    //public static bool flipped;
    //protected PlayerController playerController;

    //protected PlatformEffector2D effector;
    //protected Collider2D collider;
    //protected float waitTime=0.5f;
    //private void Awake()
    //{
    //    collider = GetComponent<Collider2D>();
    //    effector = GetComponent<PlatformEffector2D>();
    //}
    //private void Start()
    //{
    //    playerController = PlayerController.main;
    //}

    //public virtual void Update()
    //{
    //    if (GameManager.isPaused) return;

    //    if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        waitTime = 0.5f;
    //    }
    //    if (playerController.grounded || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
    //    {
    //        effector.rotationalOffset = 0;
    //        flipped = false;
    //    }

    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        if (waitTime <= 0)
    //        {
    //            effector.rotationalOffset = 180f;
    //            flipped = true;
    //            waitTime = 0.5f;
    //        }
    //        else
    //            waitTime -= Time.deltaTime;
    //    }
    //}
}
