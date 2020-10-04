﻿using UnityEngine;
using UnityEngine.UI;

public class ProjectButton : MonoBehaviour
{
    public Button button;

    public ConstructionPanelScript ProjectSelector { get; set; }
    public string ProjectID { get; set; }

    public void OnClick()
    {
        ProjectSelector.SelectProject(this);
    }
}
