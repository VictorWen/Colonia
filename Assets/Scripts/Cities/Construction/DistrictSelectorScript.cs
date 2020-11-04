using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities.Construction
{
    public class DistrictSelectorScript : MonoBehaviour
    {
        public VerticalLayoutGroup districtList;
        public Text districtTitle;
        public Text districtDescription;
        public Button confirmButton;

        public VerticalLayoutGroup districtPanelPrefab;
        public RectTransform buildingSlotPrefab;

        private City city;
        private Building building;
        private Button selectedButton;
        private District selectedDistrict;

        private GUIMaster gui;

        public void Enable(City city, Building building, GUIMaster gui)
        {
            this.city = city;
            this.building = building;
            this.gui = gui;

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
            building.SetDistrict(selectedDistrict);
            gameObject.SetActive(false);
        }

        public void CancelSelection()
        {
            city.construction.SetProject(null, gui);
            gameObject.SetActive(false);
        }
    }
}