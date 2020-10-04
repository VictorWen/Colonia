using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityOverviewPanelScript : CityPanelScript
{
    public Text cityInfoText;
    public Text currentConstructionText;

    public override void Enable(City city)
    {
        base.Enable(city);
        cityInfoText.text = city.ToString();
        currentConstructionText.text = city.construction.ToString();
    }
}
