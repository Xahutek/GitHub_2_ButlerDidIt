using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public float RotationSpeed = 5;
    [SerializeField]private GameObject flashlight;
    [SerializeField] private GameObject Aim;
    // Update is called once per frame
    void Update()
    {
        
        //flashlight.transform.LookAt(Aim.transform); 
        
        //flashlight.transform.Rotate((Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime), (Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime), 0, Space.World);
        //Debug.Log(Input.GetAxis("Mouse X"));
    }
}
