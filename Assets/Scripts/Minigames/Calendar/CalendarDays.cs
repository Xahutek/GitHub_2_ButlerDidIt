using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calendar
{
    public class CalendarDays : MonoBehaviour
    {
        public List<bool> fields = new List<bool>();
        [SerializeField] public List<DayField> allFields;

        private void Awake()
        {
            fields = new List<bool>();
        }


        private void Start()
        {
            allFields = new List<DayField>();
            foreach (Transform child in transform)
            {
                allFields.Add(child.GetComponentInChildren<DayField>());
            }
        }

        public void LoadCalendar(List<bool> calendar)
        {
            for (int i = 0; i < allFields.Count; i++)
            {
                allFields[i].SetCross(calendar[i]);
            }
        }
    }
}

