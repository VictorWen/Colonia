using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities
{
    public class CityOverviewPanelScript : CityPanelScript
    {
        public Text cityInfoText;
        public Text currentConstructionText;

        public override void Enable(City city, GUIMaster gui)
        {
            base.Enable(city, gui);
            cityInfoText.text = city.GetDescription(gui.Game);
            currentConstructionText.text = city.construction.GetDescription(this.gui);
        }

    }
}