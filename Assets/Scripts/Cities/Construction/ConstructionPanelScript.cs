using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

        public ConstructionSlotButton constructionSlotButtonPrefab;
        public VerticalLayoutGroup constructionSlotButtons;
        public RectTransform tooltip;
        private ConstructionSlot selectedConstructionSlot;
        private Button selectedConstructionSlotButton;

        public Button confirmButton;

        //public Text cityName;
        public Text cityDescription;
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

            //FillConstructionSlotButtons();

            UpdateGUI();
        }

        private void FillAvailableProjectButtons()
        {
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
                pb.Initialize(selectedCity, gui.Game, this, projectID, tooltip);
            }
        }
        
        private void FillConstructionSlotButtons()
        {
            foreach (Button btn in constructionSlotButtons.GetComponentsInChildren<Button>())
            {
                Destroy(btn.gameObject);
            }

            foreach (ConstructionSlot slot in selectedCity.construction.Slots)
            {
                ConstructionSlotButton slotButton = Instantiate(constructionSlotButtonPrefab);
                slotButton.Initialize(slot, this, tooltip);
                slotButton.transform.SetParent(constructionSlotButtons.transform);
            }
        }

        public void SelectConstructionSlot(Button button, ConstructionSlot slot)
        {
            if (selectedConstructionSlotButton != null)
                selectedConstructionSlotButton.interactable = true;
            button.interactable = false;
            selectedConstructionSlotButton = button;
            selectedConstructionSlot = slot;
            UpdateSelectedSlotInfo();
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
                selectedConstructionSlot.SetProject(selection, gui.Game);
            }
            UpdateGUI();
            Debug.Log("Project Selection Finished");
        }

        public override void UpdateGUI()
        {
            cityDescription.text = selectedCity.GetDescription(gui.Game);
            FillConstructionSlotButtons();
            if (selectedConstructionSlot == null)
                return;
            UpdateSelectedSlotInfo();
        }

        private void UpdateSelectedSlotInfo()
        {
            FillAvailableProjectButtons();
            selectionTitle.text = selectedConstructionSlot.GetProjectName();
            currentDesc.text = selectedConstructionSlot.GetDescription(gui.Game);
        }

    }
}