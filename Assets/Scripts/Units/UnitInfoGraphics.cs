using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public class UnitInfoGraphics
    {
        private readonly GameObject panel;
        private readonly Text namePlate;
        private readonly Text statusText;

        public UnitInfoGraphics(GameObject panel)
        {
            this.panel = panel;

            foreach (Text t in panel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    namePlate = t;
                else if (t.name == "Status Text")
                    statusText = t;
            }
        }

        public void SetSelectedUnit(BaseUnitEntity unit)
        {
            namePlate.text = unit.Name;
            statusText.text = unit.GetStatus();
        }

        public void ShowUnitInfoPanel()
        {
            panel.SetActive(true);
        }

        public void HideUnitInfoPanel()
        {
            panel.SetActive(false);
        }
    }
}
