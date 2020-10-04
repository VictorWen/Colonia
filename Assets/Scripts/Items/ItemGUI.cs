using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text title;
    public Text count;
    public Image image;

    private GameObject tooltipPanel;
    private bool tooltipActive;
    private string tooltip;

    private void Awake()
    {
        tooltipActive = false;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        //TODO: move tooltips to a manager class
        tooltipPanel.SetActive(true);
        tooltipPanel.GetComponentInChildren<Text>().text = tooltip;
        tooltipActive = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        tooltipPanel.SetActive(false);
        tooltipActive = false;
    }

    private void Update()
    {
        if (tooltipActive)
        {
            tooltipPanel.transform.position = Input.mousePosition + new Vector3(10, 0);
        }
    }

    public void SetItem(Item item, GameObject tooltipPanel)
    {
        title.text = item.Name;
        count.text = item.Count.ToString();
        char sep = System.IO.Path.DirectorySeparatorChar;
        image.sprite = Resources.Load<Sprite>("Items" + sep + item.ID);
        this.tooltipPanel = tooltipPanel;
        tooltip = item.ToString();
    }
}
