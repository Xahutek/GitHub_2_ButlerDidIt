using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField] Material outline;
    float distance;
    Transform PlayerPos;
    Transform OutlineObject;
    Color OutlineColor;
    [SerializeField] float outlineMaxStrength;
    // Start is called before the first frame update
    void Start()
    {
        OutlineColor = Color.black;
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
        OutlineObject = this.transform;
        outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        OutlineColor.r = Mathf.Clamp(map(distance, 1, 0, 0, 255), 0, 255);
        OutlineColor.g = Mathf.Clamp(map(distance, 1, 0, 0, 255), 0, 255);
        OutlineColor.b = Mathf.Clamp(map(distance, 1, 0, 0, 255), 0, 255);
        OutlineColor.a = Mathf.Clamp(map(distance, 1, 0, 0, 255), 0, 255);

        distance = Vector2.Distance(new Vector2(PlayerPos.transform.position.x, PlayerPos.transform.position.y), new Vector2(OutlineObject.transform.position.x, OutlineObject.transform.position.y));
        
      //  outline.SetFloat("Vector1_e2aa71b3209842c5a6eb0b87444d3361", Mathf.Clamp01(map(distance, 0.2f, 1, outlineMaxStrength, 0)));
        outline.SetColor("Color_cb38644a3f444f6cb498ab0e82528ebb", OutlineColor);
    }
    float map(float s, float a,float b, float c, float d)
    {
        return c + (s - a)*(d-c) / (b - a);
    }
}
