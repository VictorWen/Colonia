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

        private PlayerUnitEntityController unit;
        private string selectedAbilityID;

        public void Enable(PlayerUnitEntityController unit)
        {
            this.unit = unit;
            selectedAbilityID = null;
            abilityDescriptionText.text = "Select an Ability";
            
            ClearButtonLayout();            
            FillButtonLayout();

            SetupListeners();
            gameObject.SetActive(true);
        }

        public void SelectAbility(string abilityID)
        {
            selectedAbilityID = abilityID;
            abilityDescriptionText.text = GlobalAbilityDictionary.GetAbility(abilityID).GetDescription();
        }

        public void ConfirmSelection()
        {
            gameObject.SetActive(false);
            unit.AbilityAction(GlobalAbilityDictionary.GetAbility(selectedAbilityID));
        }

        private void ClearButtonLayout()
        {
            foreach (AbilityButton btn in abilityListLayout.GetComponentsInChildren<AbilityButton>())
            {
                Destroy(btn.gameObject);
            }
        }

        private void FillButtonLayout()
        {
            // Get unit's abilities
            // Foreach ability instantiate ability button prefab
            foreach (string id in unit.Unit.Combat.Abilities)
            {
                // Fill in information
                AbilityButton btn = Instantiate(abilityButtonPrefab);
                btn.SetAbility(id, this);
                btn.Title.text = GlobalAbilityDictionary.GetAbility(id).Name;
                btn.transform.SetParent(abilityListLayout.transform);
            }
        }

        private void SetupListeners()
        {
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(ConfirmSelection);
        }
    }
}