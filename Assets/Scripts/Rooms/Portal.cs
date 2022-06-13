using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Portal : MonoBehaviour
{
    public static bool isTravelling=false;

    [SerializeField] private LayerMask LMPlayer;
    [SerializeField] protected float interactRadius=2f;
    [SerializeField] protected Portal Locus;

    public static Vector3 offset;
    public float lastInput = 0;
    public AudioClip[] doorSounds;

    public void Update()
    {
        if (!isTravelling&&!GameManager.isPaused && CheckInput() && PlayerController.main.grounded &&
            interactRadius > Vector2.Distance((Vector2)transform.position, PlayerController.main.position))
            Interact();
    }
    public bool CheckInput()
    {                
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (lastInput == Input.GetAxisRaw("Vertical")) { lastInput = Input.GetAxisRaw("Vertical"); return false; }
            else
            {
                lastInput = Input.GetAxisRaw("Vertical");
                return true;
            }
        }
        else { lastInput = Input.GetAxisRaw("Vertical"); return false; }        
    }

    public virtual void Interact()
    {
        offset = PlayerController.main.position - (Vector2)transform.position;
        if(doorSounds != null)
        {
            SoundManager.main.effectSource.PlayOneShot(doorSounds[Random.Range(0, doorSounds.Length)]);
        }
        StartCoroutine(TravelRoutine(PlayerController.main));
    }
    protected virtual IEnumerator TravelRoutine(PlayerController player)
    {
        isTravelling = true;

        GlobalBlackscreen.multiplier = 1;
        GlobalBlackscreen.on = true;

        yield return new WaitForSeconds(1);

        PlayerController.main.position = Locus.transform.position+offset;
        PlayerController.main.RefreshCollider();
        isTravelling = false;

        GlobalBlackscreen.on = false;
    }
}
