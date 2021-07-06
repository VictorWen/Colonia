using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public interface ITooltippable
    {
        string GetTooltipText();
    }

    public class TooltipOnHoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ITooltippable tooltippable;
        private RectTransform tooltipObject;
        private Text tooltipText;
        private int offset;
        private bool movingTooltip;
        private bool rightSide;

        private bool showingToolTip = false;

        public void Initialize(ITooltippable tooltippable, RectTransform obj, Text text, int offset, bool moving = false, bool rightSide = true)
        {
            this.tooltippable = tooltippable;
            this.tooltipObject = obj;
            this.tooltipText = text;
            this.offset = offset;
            if (this.offset < 5)
                this.offset = 5;
            this.movingTooltip = moving;
            this.rightSide = rightSide;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltip(eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
        }

        protected virtual void ShowTooltip(Vector3 mousePosition)
        {
            showingToolTip = true;
            string tooltip = tooltippable.GetTooltipText();
            tooltipText.text = tooltip;

            tooltipObject.position = GetTooltipPosition(mousePosition);
            tooltipObject.gameObject.SetActive(true);
        }

        protected virtual void HideTooltip()
        {
            showingToolTip = false;
            tooltipObject.gameObject.SetActive(false);
        }

        protected virtual Vector3 GetTooltipPosition(Vector3 mousePosition)
        {
            if (rightSide)
                return mousePosition + new Vector3(offset, 0);
            else
                return mousePosition - new Vector3(tooltipObject.rect.width + offset, 0);
        }

        protected void Update()
        {
            if (movingTooltip && showingToolTip)
                tooltipObject.position = GetTooltipPosition(Input.mousePosition);
        }
    }
}
