using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Cities.Construction
{
    public class ConstructionSlotButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ConstructionSlot slot;
        private ConstructionPanelScript parent;
        public Button button;
        private Text text;

        private void Start()
        {
            button.interactable = true;
            button.onClick.AddListener(Select);
        }

        public void Initialize(ConstructionSlot slot, ConstructionPanelScript parent)
        {
            this.slot = slot;
            this.parent = parent;
            text = GetComponentInChildren<Text>();
            text.text = slot.GetProjectName();
        }

        public void Select()
        {
            parent.SelectConstructionSlot(button, slot);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            parent.ShowTooltip(eventData.position, slot.GetSlotDescription());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            parent.HideTooltip();
        }
    }
}
