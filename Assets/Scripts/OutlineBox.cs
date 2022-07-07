using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBox : MonoBehaviour
{
    Material outline;
    private SpriteRenderer sRenderer;
    // Start is called before the first frame update
    Transform player;
    [SerializeField] float ItemRadius = 1;
    private float distance;
    [SerializeField] float OutlineThickness = 0.01f;

    [ColorUsageAttribute(true, true)]
    [SerializeField] Color color = Color.white;
    void Start()
    {
        
        sRenderer = GetComponent<SpriteRenderer>();
        outline = sRenderer.material;
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.black);
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0);
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector2.Distance(this.transform.position, PlayerController.main.transform.position);
     
        if (ItemRadius>distance)
        {
            outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", color);
            outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", OutlineThickness);
        }
        if (ItemRadius < distance)
        {
            outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", Color.black);
            outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0);
        }
    }


}
