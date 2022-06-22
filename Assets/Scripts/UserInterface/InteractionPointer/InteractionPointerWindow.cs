using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPointerWindow : MonoBehaviour
{
    public static InteractionPointerWindow main;

    public InteractionPointer pointerPrefab;
    public List<InteractionPointer> pointers= new List<InteractionPointer>();

    private void Awake()
    {
        main = this;
    }
    public InteractionPointer GetFreePointer()
    {
        foreach (InteractionPointer p in pointers)
        {
            if(!p.isOpen)
                return p;
        }

        InteractionPointer newPointer= Instantiate(pointerPrefab,transform);
        pointers.Add(newPointer);
        return newPointer;
    }

    public void SubscribeCharacterLocus(CharacterLocus locus)
    {
        UnsubscribeCharacterLocus(locus);
        InteractionPointer pointer = GetFreePointer();
        pointer.targetLocus = locus;
        pointer.ResetAnimation();
    }
    public void UnsubscribeCharacterLocus(CharacterLocus locus)
    {
        foreach (InteractionPointer p in pointers)
        {
            if (p.targetLocus == locus)
                p.targetLocus = null;
        }
    }
}
