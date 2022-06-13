using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FakeParallax : MonoBehaviour
{
    Camera cam;
    SpriteRenderer sprite;

    Vector2 position
    {
        get { return transform.position; }
    }
    Vector2 origin;
    public bool capright, capleft;
    public float ParallaxEccentricity = 1;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        cam = Camera.main;
        origin = transform.position;
    }

    private void Update()
    {
        float
            CamRelative = origin.x - cam.transform.position.x,
            layerSign = sprite.sortingLayerName == "Default" ? 0 : (sprite.sortingLayerName == "Foreground" ? 1 : -1)/*,*/
        //    layerOrderMultiplier = (float)sprite.sortingOrder / 10;
        //layerOrderMultiplier = (layerSign > 0 ? 1 - layerOrderMultiplier : layerOrderMultiplier)
        ;
        float
            ParallaxEccentricity = layerSign * this.ParallaxEccentricity,
            ParallaxLerp = Mathf.Clamp((Mathf.Abs(CamRelative) / (cam.orthographicSize * 2)) * Mathf.Sign(CamRelative), -1, 1);

        transform.position = Vector2.LerpUnclamped(origin, origin + Vector2.right * ParallaxEccentricity, ParallaxLerp);

        if (capright && origin.x < transform.position.x)
            transform.position = new Vector2(origin.x, transform.position.y);
        if (capleft && origin.x > transform.position.x)
            transform.position = new Vector2(origin.x, transform.position.y);
    }

    //public float mp;
    //[SerializeField]
    //private float ParallaxEccentricity
    //{
    //    get
    //    {
    //        float
    //            sign = sprite.sortingLayerName == "Default" ? 0 : (sprite.sortingLayerName == "Foreground" ? -1 : 1),
    //            multiplier = (float)sprite.sortingOrder / 10;

    //        mp = multiplier;
    //        multiplier = (sign > 0 ? 1 - multiplier : multiplier);

    //        return sign * multiplier;
    //    }
    //}
    //float ParallaxLerp
    //{
    //    get
    //    {
    //        float relative = position.x - cam.transform.position.x;
    //        return Mathf.Clamp((Mathf.Abs(relative) / (cam.orthographicSize * 2)) * Mathf.Sign(relative), -1, 1);
    //    }
    //}

    //private void Awake()
    //{
    //    sprite = GetComponent<SpriteRenderer>();
    //}
    //private void Start()
    //{
    //    cam = Camera.main;
    //    origin = transform.position;
    //}

    //private void Update()
    //{
    //        transform.position = Vector2.LerpUnclamped(origin, origin + Vector2.left * ParallaxEccentricity, ParallaxLerp);
    //}
}
