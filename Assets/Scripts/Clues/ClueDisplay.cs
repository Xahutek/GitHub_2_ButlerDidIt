using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClueDisplay : MonoBehaviour
{
    public Clue clue;
    public Image itemImage;
    public TMP_Text exclamationMark;
    [SerializeField] private TMP_Text text;

    public void Refresh(Note n)
    {
        if (n == null || n.line.text=="")
        {
            Clear();
            return;
        }
        clue = null;
        text.text = "\""+n.line.text+"\"";
        gameObject.SetActive(true);
    }
    public void Refresh(Clue clue, bool inventoryCall)
    {
        if (clue != null && !clue.seenInInventory)
        {
            if(exclamationMark != null) { exclamationMark.enabled = true; }
            clue.seenInInventory = true;
        }
        else if (exclamationMark != null) { exclamationMark.enabled = false; }
        Refresh(clue);
    }
    public void Refresh(Clue c)
    {
        if (c == null)
        { 
            Clear();
            return;
        }
        clue = c;
        text.text = c.name;
        gameObject.SetActive(true);
    }

    public void Refresh(Item item, bool inventoryCall)
    {
        if (item != null &&!item.seenInInventory)
        {
            item.seenInInventory = true;
        }
        else if (exclamationMark != null) { exclamationMark.enabled = false; }
        Refresh(item);
    }
    public void Refresh(Item item)
    {
        if (item == null || item.givenAway)
        {
            Clear();
            return;
        }
        clue = item;
        text.text = item.name;
        itemImage.sprite = item.picture;
        gameObject.SetActive(true);

    }
    public void Clear()
    {
        clue = null;
        text.text = "";
        gameObject.SetActive(false);
    }
    public void Pick()
    {
        if(clue) EventSystem.main.PickClue(clue);
    }
}
