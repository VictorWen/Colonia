using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cities;
using Cities.Construction.Projects;

namespace Cities.Construction
{
    /*
     * Manages the selection of a new project for cities
     */
    public class ConstructionPanelScript : CityPanelScript
    {
        [Header("GameObjects")]
        public World world;

        public GameObject availableProjectList;
        //public GameObject unavailableProjectsList;
        public GameObject projectDescriptorPanel;

        public Button constructionSlotButtonPrefab;
        public VerticalLayoutGroup constructionSlotButtons;

        public Button confirmButton;

        //public Text cityName;
        public Text selectionTitle;
        public Text selectionDesc;
        public Text currentDesc;

        public ProjectSelectionManager selectionManager;

        [Header("Prefabs")]
        public ProjectButton projectButtonPrefab;

        private City selectedCity;
        private ProjectButton selectedButton;

        public override void Enable(City city, GUIMaster gui)
        {
            this.selectedCity = city;
            this.selectedButton = null;
            base.Enable(city, gui);

            //Clear List Entries and Texts
            selectionTitle.text = "";
            selectionDesc.text = "";
            foreach (ProjectButton b in availableProjectList.GetComponentsInChildren<ProjectButton>())
            {
                Destroy(b.transform.gameObject);
            }

            //TODO: Implement automatic buttons
            //Populate project lists
            foreach (string projectID in selectedCity.construction.GetAvailableProjects())
            {
                ProjectButton pb = Instantiate(projectButtonPrefab);
                pb.transform.SetParent(availableProjectList.transform);
                pb.ProjectSelector = this;
                pb.text.text = GlobalProjectDictionary.GetProjectData(projectID).Name;
                pb.ProjectID = projectID;
            }

            FillConstructionSlotButtons();

            UpdateGUI();
        }
        
        private void FillConstructionSlotButtons()
        {
            foreach (Button btn in constructionSlotButtons.GetComponentsInChildren<Button>())
            {
                Destroy(btn.gameObject);
            }

            foreach (ConstructionSlot slot in selectedCity.construction.Slots)
            {
                Button slotButton = Instantiate(constructionSlotButtonPrefab);
                slotButton.GetComponentInChildren<Text>().text = slot.GetProjectName();
                ConstructionSlot copy = slot;
                slotButton.onClick.AddListener(() => SelectConstructionSlot(copy));
                slotButton.transform.SetParent(constructionSlotButtons.transform);
            }
        }

        private void SelectConstructionSlot(ConstructionSlot slot)
        {
            Debug.Log(slot.GetProjectName());
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
            selectionDesc.text = p.GetDescription(selectedCity, gui.Game);
            selectedButton = b;
            confirmButton.interactable = p.IsConstructable(selectedCity, gui.Game);
        }

        public void ConfirmProject()
        {
            Debug.Log("Project Selection Confirmed");
            if (selectedButton != null)
            {
                //ProjectData project = GlobalProjectDictionary.GetProjectData();
                Debug.Log("Project Selection Started");
                ProjectData data = GlobalProjectDictionary.GetProjectData(selectedButton.ProjectID);
                IProject selection = data.Project;
                selection.AcceptProjectVisitor(selectedCity, selectionManager);
            }
        }

        public void FinishSelection(IProject selection)
        {
            if (selection.IsSelected())
            {
                Debug.Log("Project Selected");
                selectedCity.construction.SetProject(selection, gui.Game);
            }
            UpdateGUI();
            Debug.Log("Project Selection Finished");
        }

        public override void UpdateGUI()
        {
            currentDesc.text = selectedCity.construction.GetDescription(gui.Game);
        }

    }
}