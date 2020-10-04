using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabScript : MonoBehaviour
{
    private CityGUIScript gui;
    public Button button;
    public CityPanelScript panel;

    private void Start()
    {
        button.onClick.AddListener(() => gui.ChangeTab(this));
    }

    public void Setup(CityGUIScript gui)
    {
        this.gui = gui;
    }
}
