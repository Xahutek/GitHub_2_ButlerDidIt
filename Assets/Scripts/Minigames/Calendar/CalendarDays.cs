using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calendar
{
    public class CalendarDays : MonoBehaviour
    {
        public Dictionary<int, DayField> fields = new Dictionary<int, DayField>();
        [SerializeField] List<DayField> allFields;

        private void Awake()
        {
            fields = new Dictionary<int, DayField>();
        }

        private void Start()
        {
            allFields = new List<DayField>();      
            foreach(Transform child in transform)
            {
                allFields.Add(child.GetComponentInChildren<DayField>());
            }
            for (int i = 0; i < allFields.Count; i++)
            {
                fields.Add(i + 1, allFields[i]);
            }
        }
    }   
}

