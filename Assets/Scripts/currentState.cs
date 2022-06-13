using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentState : MonoBehaviour
{
    Animator anim;
    [SerializeField] bool state1 = true;
    [SerializeField] bool state2 = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (state1)
            anim.SetBool("IsIdle",true);
        if (state2)
            anim.SetBool("IsGardening",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
