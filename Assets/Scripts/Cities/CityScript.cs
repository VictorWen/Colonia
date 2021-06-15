using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities
{
    public class CityScript : MonoBehaviour
    {
        public SpriteRenderer spriteRend;
        public Text title;

        private GUIStateManager state;
        public City city; //TODO: change back to private after testing
        private CityGUIScript panel;

        public static CityScript Create(string name, Vector3 position, GUIMaster gui)
        {
            CityScript script = Instantiate(gui.cityPrefab, position, new Quaternion());
            script.state = gui.GUIState;
            script.city = new City(name, gui.Game.World.WorldToCell(position), gui.Game.World);
            gui.Game.World.UpdatePlayerVisionGrpahics();
            script.panel = gui.cityGUI;

            gui.Game.AddNewCity(script.city);
            script.title.text = name + "(" + script.city.population + ")";
            return script;
        }

        public void OnMouseOver()
        {
            if (state.TileInteraction)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    spriteRend.color = new Color(0.5f, 0.5f, 0.5f);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    spriteRend.color = new Color(0.8f, 0.8f, 0.8f);
                    panel.OpenCityGUI(city);
                }
            }
        }

        //TODO: make SpriteButtonScript class
        public void OnMouseEnter()
        {
            if (state.TileInteraction)
                spriteRend.color = new Color(0.8f, 0.8f, 0.8f);
        }

        public void OnMouseExit()
        {
            if (state.TileInteraction)
                spriteRend.color = new Color(1, 1, 1);
        }
    }
}