using Cities.Construction.Projects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public class ProjectOnSelectionManager : MonoBehaviour
    {
        public void StartSelection(IProject selection, ConstructionPanelScript panel, City selectedCity, GUIMaster gui)
        {
            StartCoroutine(Select(selection, panel, selectedCity, gui));
        }

        private IEnumerator Select(IProject selection, ConstructionPanelScript panel, City selectedCity, GUIMaster gui)
        {
            yield return StartCoroutine(selection.OnSelect(selectedCity, gui));
            panel.FinishSelection(selection);
            yield break;
        }
    }
}