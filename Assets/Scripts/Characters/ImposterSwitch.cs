using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterSwitch : MonoBehaviour
{   
    void OnEnable()
    {
        InteractableCharacter cha = GetComponentInParent<InteractableCharacter>();
        if (cha != null)
        {
            if(cha.character == Character.Imposter)
            {
                GetComponent<Animator>().SetTrigger("isSus");
            }
        }
    }
}
