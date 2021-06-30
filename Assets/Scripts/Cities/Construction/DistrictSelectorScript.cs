using Cities.Construction.Projects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities.Construction
{
    public class DistrictSelectorScript : MonoBehaviour, ProjectSelectionController
    {
        public VerticalLayoutGroup districtList;
        public Text districtTitle;
        public Text districtDescription;
        public Button confirmButton;

        public VerticalLayoutGroup districtPanelPrefab;
        public RectTransform buildingSlotPrefab;

        //private City city;
        private Building building;
        private Button selectedButton;
        private District selectedDistrict;

        private CityGUIPanelScript cityGUI;
        private bool waiting;

        public void Enable(City city, Building building, CityGUIPanelScript cityGUI)
        {
            //this.city = city;
            this.building = building;
            this.cityGUI = cityGUI;
            this.waiting = true;

            // Clear lists
            foreach (VerticalLayoutGroup districtPanel in districtList.GetComponentsInChildren<VerticalLayoutGroup>()){
                if (districtPanel.gameObject.name != "District List")
                    Destroy(districtPanel.gameObject);
            }

            // Load all districts
            // Foreach instantiate district panel prefab
            for (int i = 0; i < city.Districts.Count; i++)
            {
                District district = city.Districts[i];
                VerticalLayoutGroup districtPanel = Instantiate(districtPanelPrefab);
                districtPanel.transform.SetParent(districtList.transform);
                districtPanel.GetComponentInChildren<Text>().text = district.Name;
                Button districtButton = districtPanel.GetComponentInChildren<Button>();
                districtButton.onClick.AddListener(() => SelectDistrict(district, districtButton));
                
                // Load building slots
                // Foreach instantiate building slot prefab
                VerticalLayoutGroup slotList = districtPanel.GetComponentsInChildren<VerticalLayoutGroup>()[1];
                slotList.gameObject.SetActive(true);
                foreach (Building b in district.Buildings)
                {
                    RectTransform slot = Instantiate(buildingSlotPrefab);
                    string name = "Empty";
                    if (b != null)
                    {
                        name = b.ID; // TODO: Change to building.Name
                    }
                    slot.GetComponentInChildren<Text>().text = name;
                    slot.SetParent(slotList.transform);
                }

                if (district.Buildings.Count == 0)
                    slotList.gameObject.SetActive(false);
            }

            confirmButton.interactable = false;
            gameObject.SetActive(true);
        }

        public IEnumerator StartSelection()
        {
            while (waiting)
            {
                yield return null;
            }
            yield break;
        }

        //TODO: Handle selecting taken building slot
        public void SelectDistrict(District district, Button button)
        {
            if (selectedButton != null)
                selectedButton.interactable = true;
            
            districtTitle.text = district.Name;
            districtDescription.text = district.GetDistrictDescription();
            
            selectedDistrict = district;
            selectedButton = button;
            selectedButton.interactable = false;
            
            confirmButton.interactable = true;
        }

        public void ConfirmSelection()
        {
            building.FinishSelection(selectedDistrict);
            Close();
        }

        public void CancelSelection()
        {
            building.FinishSelection(null);
            Close();
        }

        private void Close()
        {
            cityGUI.UpdateGUI();
            gameObject.SetActive(false);
            waiting = false;
        }
    }
}