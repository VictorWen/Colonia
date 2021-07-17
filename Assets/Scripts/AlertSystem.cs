using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Scripts
{
    public class AlertSystem : MonoBehaviour
    {
        public Button alertButtonPrefab;
        public GUIMaster gui;

        private void Start()
        {
            gui.Game.addAlert += AddAlert;
        }

        public void AddAlert(string description)
        {
            Button button = Instantiate(alertButtonPrefab);
            button.GetComponentInChildren<Text>().text = description;
            button.transform.SetParent(transform);
            button.onClick.AddListener(() => ClearButton(button));
        }

        public void ClearButton(Button b)
        {
            Destroy(b.gameObject);
        }
    }
}