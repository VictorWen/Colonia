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
        private readonly Image unitImage;

        private UnitEntity selectedUnit;
        private Sprite unitSprite;

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

            foreach (Image img in panel.GetComponentsInChildren<Image>())
            {
                if (img.name == "Image")
                    unitImage = img;
            }
        }

        public void SetSelectedUnit(BaseUnitEntityController ctrl)
        {
            UnitEntity unit = ctrl.Unit;
            unitSprite = ctrl.GetComponent<SpriteRenderer>().sprite;
            if (selectedUnit != null)
                selectedUnit.OnStatusChanged -= UpdateStatus;
            selectedUnit = unit;
            UpdateStatus();
            unit.OnStatusChanged += UpdateStatus;
        }

        public void UpdateStatus()
        {
            namePlate.text = selectedUnit.Name;
            statusText.text = selectedUnit.GetStatus();
            unitImage.sprite = unitSprite;
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
