using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Portal : MonoBehaviour
{
    public static bool isTravelling=false;

    private BoxCollider2D col;
    public BoxCollider2D clickCol;

    [SerializeField] private LayerMask LMPlayer;
    [SerializeField] protected Portal Locus;

    public static Vector3 offset;
    public float lastInput = 0;
    bool playerInRange;
    public AudioClip[] doorSounds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange=false;
    }
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }
    public void Update()
    {

        if (playerInRange && !isTravelling && !GameManager.isPaused && Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.S) && PlayerController.main.grounded)
            Interact();

        if (Input.GetMouseButtonDown(0) && !GameManager.isPaused && playerInRange)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickCol.OverlapPoint(mousePos))
                Interact();
        }
    }
    public bool CheckInput()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");
        if (verticalAxis != 0)
        {
            if (lastInput == verticalAxis) { lastInput = verticalAxis; return false; }
            else
            {
                lastInput = verticalAxis;
                return true;
            }
        }
        else { lastInput = verticalAxis; return false; }        
    }

    public virtual void Interact()
    {
        offset = PlayerController.main.position - (Vector2)transform.position;
        if(doorSounds != null)
        {
            SoundManager.main.PlayOneShot(doorSounds[Random.Range(0, doorSounds.Length)]);
        }
        StartCoroutine(TravelRoutine(PlayerController.main));
    }
    protected virtual IEnumerator TravelRoutine(PlayerController player)
    {
        isTravelling = true;
        GameManager.manualPaused = true;

        GlobalBlackscreen.multiplier = 1;
        GlobalBlackscreen.on = true;

        yield return new WaitForSeconds(1);

        PlayerController.main.position = Locus.transform.position+offset;
        PlayerController.main.RefreshCollider();
        isTravelling = false;

        GlobalBlackscreen.on = false;
        GameManager.manualPaused = false;
    }
}
