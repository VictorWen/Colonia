using UnityEngine;
using Cities;

public abstract class CityPanelScript : MonoBehaviour
{
    protected GUIMaster gui;

    public virtual void Enable(City city, GUIMaster gui)
    {
        this.gui = gui;
        gameObject.SetActive(true);
    }
}