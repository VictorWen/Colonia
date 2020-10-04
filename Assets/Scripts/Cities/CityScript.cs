using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: formalize CityScript and City relationship
public class CityScript : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Text title;

    public City city;

    public void OnMouseOver()
    {
        if (GUIMaster.main.GUIState == GameState.MAP)
        {
            if (Input.GetMouseButtonDown(0))
            {
                spriteRend.color = new Color(0.5f, 0.5f, 0.5f);
            }
            if (Input.GetMouseButtonUp(0))
            {
                spriteRend.color = new Color(0.8f, 0.8f, 0.8f);
                GUIMaster.main.OpenCityGUI(city);
            }
        }
    }

    //TODO: make SpriteButtonScript class
    public void OnMouseEnter()
    {
        spriteRend.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void OnMouseExit()
    {
        spriteRend.color = new Color(1, 1, 1);
    }
}
