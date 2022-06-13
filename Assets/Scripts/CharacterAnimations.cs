using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    BoxCollider2D area;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!anim) anim = GetComponent<Animator>();

        anim.SetTrigger("CloseToButler");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!anim) anim = GetComponent<Animator>();

        anim.SetTrigger("AwayFromButler");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
