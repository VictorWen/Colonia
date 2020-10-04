using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages the selection of a new project for cities
 */
public class ConstructionPanelScript : CityPanelScript
{
    [Header("GameObjects")]
    public WorldTerrain world;

    public GameObject availableProjectList;
    public GameObject unavailableProjectsList;
    public GameObject projectDescriptorPanel;

    //public Text cityName;
    public Text selectionTitle;
    public Text selectionDesc;
    public Text currentDesc;

    [Header("Prefabs")]
    public GameObject projectButtonPrefab;
    public TileImprovementGhostScript tileImprovementPrefab;

    private City selectedCity;
    private ProjectButton selectedButton;

    public override void Enable(City city)
    {
        this.selectedCity = city;
        this.selectedButton = null;
        base.Enable(city);
    }

    private void OnEnable()
    {
        //Clear List Entries and Texts
        selectionTitle.text = "";
        selectionDesc.text = "";
        foreach (ProjectButton b in availableProjectList.GetComponentsInChildren<ProjectButton>())
        {
            Destroy(b.transform.gameObject);
        }
        foreach (ProjectButton b in unavailableProjectsList.GetComponentsInChildren<ProjectButton>())
        {
            Destroy(b.transform.gameObject);
        }

        //TODO: Implement automatic buttons
        //Populate project lists
        foreach (string projectID in selectedCity.construction.GetAvailableProjects())
        {
            GameObject button = Instantiate(projectButtonPrefab);
            button.transform.SetParent(availableProjectList.transform);
            button.GetComponent<ProjectButton>().ProjectSelector = this;
            //TODO: combine both properties VVVV
            button.GetComponentInChildren<Text>().text = GlobalProjectDictionary.GetProjectData(projectID).Name;
            button.GetComponent<ProjectButton>().ProjectID = projectID;
        }

        UpdateGUI();
    }

    public void SelectProject(ProjectButton b)
    {
        b.button.interactable = false;
        if (selectedButton != null)
        {
            selectedButton.button.interactable = true;
        }
        ProjectData p = GlobalProjectDictionary.GetProjectData(b.ProjectID);
        selectionTitle.text = p.Name;
        selectionDesc.text = p.ToString();
        selectedButton = b;
    }

    public void ConfirmProject()
    {
        if (selectedButton != null)
        {
            //ProjectData project = GlobalProjectDictionary.GetProjectData();
            selectedCity.construction.SetProject(selectedButton.ProjectID);
        }
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        currentDesc.text = selectedCity.construction.ToString();
    }

    public void Close()
    {
        GUIMaster.main.CloseCityGUI();
    }
}
