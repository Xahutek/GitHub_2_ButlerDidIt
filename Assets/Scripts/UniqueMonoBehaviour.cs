using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueMonoBehaviour : MonoBehaviour
{
    private static Dictionary<string,UniqueMonoBehaviour> instances = new Dictionary<string, UniqueMonoBehaviour>();
    [SerializeField] private string ID;
    private void Awake()
    {
        if (instances.ContainsKey(ID))
        {
            Destroy(gameObject);
            return;
        }
        else
            instances.Add(ID, this);
    }
}
