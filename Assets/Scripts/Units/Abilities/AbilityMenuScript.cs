using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

namespace Units.Abilities
{
    public class AbilityMenuScript : MonoBehaviour
    {
        public VerticalLayoutGroup abilityListLayout;
        public Text abilityDescriptionText;
        public Button selectButton;
        public AbilityButton abilityButtonPrefab;

        // TODO: temporary AbilityMenuScript field variables
        public World world;
        public Tile red;

        private UnitEntityController unit;
        private string selectedAbilityID;

        public void Enable(UnitEntityController unit)
        {
            this.unit = unit;
            selectedAbilityID = null;
            abilityDescriptionText.text = "Select an Ability";
            // Clear layout
            foreach (AbilityButton btn in abilityListLayout.GetComponentsInChildren<AbilityButton>())
            {
                Destroy(btn.gameObject);
            }

            // Get unit's abilities
            // Foreach ability instantiate ability button prefab
            foreach (string id in unit.Unit.Abilities)
            {
                // Fill in information
                AbilityButton btn = Instantiate(abilityButtonPrefab);
                btn.SetAbility(id, this);
                btn.Title.text = GlobalAbilityDictionary.GetAbility(id).Name;
                btn.transform.SetParent(abilityListLayout.transform);
            }
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(ConfirmSelection);
            gameObject.SetActive(true);
        }

        public void SelectAbility(string abilityID)
        {
            this.selectedAbilityID = abilityID;
            // Fill in description informaiton
            abilityDescriptionText.text = GlobalAbilityDictionary.GetAbility(abilityID).GetDescription();
        }

        public void ConfirmSelection()
        {
            unit.CastAbility(GlobalAbilityDictionary.GetAbility(selectedAbilityID));
        }
    }
}