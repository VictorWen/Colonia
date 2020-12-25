using UnityEngine;
using UnityEngine.UI;

namespace Units.Abilities
{
    public class AbilityButton : MonoBehaviour
    {
        public Text Title {get; private set;}
        private string id;

        public void SetAbility(string id, AbilityMenuScript menu)
        {
            this.id = id;
            Title = GetComponentInChildren<Text>();
            GetComponent<Button>().onClick.AddListener(() => menu.SelectAbility(this.id));
        }
    }
}