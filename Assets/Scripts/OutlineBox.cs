using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBox : MonoBehaviour
{
    [SerializeField] Material outline;
    // Start is called before the first frame update
    [SerializeField] BoxCollider2D boxColliderTwoDee;
    void Start()
    {
        boxColliderTwoDee = GetComponent<BoxCollider2D>();
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.black);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Outline on");
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.white);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0.1f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Outline off");
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.black);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Outline on");
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.white);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0.1f);
    }

}
