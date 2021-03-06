using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField] public GameObject visuals=null;

    public string ID;
    [SerializeField] private float 
        interactCooldown = 0.1f,
        interactRange=3f;
    float interactTimer=0;
    public bool blocked, deactivated;

    public Vector2 AvailableTime = new Vector2(0, 24);

    public Clue Yield;
    public Item itemPrice;
    public bool deactivateOnInteraction, deactivateIfKnown;

    private Collider2D colli;

    public virtual void Awake()
    {
        if(visuals==null&&transform.childCount>0)
        visuals = transform.GetChild(0).gameObject;
    }
    private void Start() 
    {
        colli = GetComponent<Collider2D>();
    }
        
    private void Update()
    {        
        if (Input.GetMouseButtonDown(0)&&!GameManager.isPaused) { Interaction(); }
    }

    private void FixedUpdate()
    {
        Availability();
    }

    public virtual void Availability()
    {
        if (visuals != null)
            visuals.SetActive(!deactivated && Clock.Hour > AvailableTime.x && Clock.Hour < AvailableTime.y
                && (!deactivateIfKnown || !Yield || (Yield && !Yield.KnownTo(Character.Butler))));
    }

    public bool canInteract
    {
        get { return interactTimer<Time.time && (!itemPrice || (itemPrice.KnownTo(Character.Butler) && !itemPrice.givenAway)); }
    }

    public void Interaction()
    {
        if (!canInteract || blocked || deactivated) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (colli.OverlapPoint(mousePosition))
        {
            Debug.Log("interacted");

            deactivated = deactivateOnInteraction;

            ClickInteract();

            EventSystem.main.Interact(this);

            if (Yield)
                Yield.MakeKnownTo(Character.Butler);
            if (itemPrice) itemPrice.givenAway = true;

            interactTimer = Time.time + interactCooldown;
        }

    }

    public virtual void ClickInteract()
    {

    }

    public void Deactivate()
    {
        deactivated = deactivateOnInteraction;
    }
}
