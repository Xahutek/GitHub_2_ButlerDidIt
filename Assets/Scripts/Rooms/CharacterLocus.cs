using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocus : MonoBehaviour
{
    public Character identity;
    public CharacterState state;
    public bool active;
    public void Refresh(CharacterProfile.TimeBox box,Room room, Character c)
    {
        if (active) return;

        active = true;
        if (c != identity || state != box.state|| room!=box.room)
            active = false;

        if (c == identity && c == Character.Detective && box.state == CharacterState.Normal && state == CharacterState.Normal)
            Debug.Log("Log " + active);

        if (active)
            RefreshMarker();

        gameObject.SetActive(active);
    }
    public void RefreshMarker()
    {
        CharacterProfile p = identity.Profile();
        Dialogue d = p.GetDialogue();
        if (d.unique && d != p.nullDialogue)
            InteractionPointerWindow.main.SubscribeCharacterLocus(this);
        else
            InteractionPointerWindow.main.UnsubscribeCharacterLocus(this);
    }

    private void OnDisable()
    {
        InteractionPointerWindow.main.UnsubscribeCharacterLocus(this);
    }
}
