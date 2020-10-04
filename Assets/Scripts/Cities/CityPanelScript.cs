using UnityEngine;

public abstract class CityPanelScript : MonoBehaviour
{
    public virtual void Enable(City city)
    {
        gameObject.SetActive(true);
    }
}