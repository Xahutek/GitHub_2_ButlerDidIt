using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBox : MonoBehaviour
{
    [SerializeField] Material outline;
    // Start is called before the first frame update
    void Start()
    {
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.black);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.white);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0.5f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.black);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0);
    }

}
