using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Scripts;

namespace Cities.Construction
{
    public class ConstructionSlotButton : MonoBehaviour, ITooltippable
    {
        private ConstructionSlot slot;
        private ConstructionPanelScript parent;
        public Button button;
        private Text text;
        public TooltipOnHoverScript tooltipScript;

        private void Start()
        {
            button.interactable = true;
            button.onClick.AddListener(Select);
        }

        public void Initialize(ConstructionSlot slot, ConstructionPanelScript parent, RectTransform tooltipPanel)
        {
            this.slot = slot;
            this.parent = parent;
            text = GetComponentInChildren<Text>();
            text.text = slot.GetProjectName();

            tooltipScript.Initialize(this, tooltipPanel, tooltipPanel.GetComponentInChildren<Text>(), 15, true);
        }

        public void Select()
        {
            parent.SelectConstructionSlot(button, slot);
        }

        public string GetTooltipText()
        {
            return slot.GetSlotOverview();
        }
    }
}
