using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Items.ItemActions;
using Units;

namespace Items
{
    public class ItemGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Text title;
        public Text count;
        public Image image;

        private Item item;

        private GameObject tooltipPanel;
        private bool tooltipActive;
        private string tooltip;

        private bool isActiveItem;
        private GameObject inventoryPanel;
        private UnitEntityPlayerController actor;
        private VerticalLayoutGroup actionMenu;
        private Button actionButtonPrefab;

        private void Awake()
        {
            tooltipActive = false;
        }

        public void OnPointerEnter(PointerEventData data)
        {
            //TODO: move tooltips to a manager class
            tooltipPanel.GetComponentInChildren<Text>().text = tooltip;
            tooltipPanel.SetActive(true);
            tooltipActive = true;
        }

        public void OnPointerExit(PointerEventData data)
        {
            tooltipPanel.SetActive(false);
            tooltipActive = false;
        }

        public void OnPointerClick(PointerEventData data)
        {
            if (data.button.Equals(PointerEventData.InputButton.Right) && isActiveItem)
            {
                DisplayActions();
            }
        }
        
        private void Update()
        {
            if (tooltipActive)
            {
                tooltipPanel.transform.position = Input.mousePosition + new Vector3(10, 0);
            }
            if (actionMenu.gameObject.activeInHierarchy && Input.GetMouseButtonUp(0))
            {
                actionMenu.gameObject.SetActive(false);
            }
        }

        public void SetItem(Item item, GameObject tooltipPanel)
        {
            this.item = item;
            title.text = item.Name;
            count.text = item.Count.ToString();
            char sep = System.IO.Path.DirectorySeparatorChar;
            image.sprite = Resources.Load<Sprite>("Items" + sep + item.ID);
            this.tooltipPanel = tooltipPanel;
            tooltip = item.ToString();
        }

        public void SetItem(Item item, GameObject tooltipPanel, GameObject inventoryPanel, UnitEntityPlayerController actor, VerticalLayoutGroup itemActionMenu, Button actionButtonPrefab)
        {
            this.item = item;
            title.text = item.Name;
            count.text = item.Count.ToString();
            char sep = System.IO.Path.DirectorySeparatorChar;
            image.sprite = Resources.Load<Sprite>("Items" + sep + item.ID);
            this.tooltipPanel = tooltipPanel;
            tooltip = item.ToString();

            isActiveItem = true;
            this.inventoryPanel = inventoryPanel;
            this.actor = actor;
            actionMenu = itemActionMenu;
            this.actionButtonPrefab = actionButtonPrefab;
        }

        public void DisplayActions()
        {
            // Clear action menu
            foreach (Button btn in actionMenu.GetComponentsInChildren<Button>())
            {
                Destroy(btn.gameObject);
            }

            // Fill action menu
            foreach (ItemAction action in item.GetItemActions())
            {
                Button btn = Instantiate(actionButtonPrefab);
                btn.transform.SetParent(actionMenu.transform);
                btn.GetComponentInChildren<Text>().text = action.Name;
                btn.interactable = action.Enabled;
                btn.onClick.AddListener(() => action.Action(actor));
                btn.onClick.AddListener(() => actionMenu.gameObject.SetActive(false));
                btn.onClick.AddListener(() => inventoryPanel.SetActive(false));
            }
            actionMenu.transform.position = Input.mousePosition;
            actionMenu.gameObject.SetActive(true);
        }
    }
}