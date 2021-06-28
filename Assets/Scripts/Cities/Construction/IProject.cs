using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public interface IProject
    {
        string ProjectType { get; }

        string ID { get; }

        Dictionary<string, int> GetResourceCost(City city, GameMaster game);

        // Returns if there is any availability to construct this Project in the given City
        bool IsConstructable(City city, GameMaster game);

        // Coroutine started when the Project is selected by the ConstructionPanelScript
        IEnumerator OnSelect(City city, GUIMaster gui);

        // Return whether the Project has completed the selection process
        bool IsSelected();

        void OnCancel(City city, World gui);

        void Complete(City city, World world);

        IProject Copy();

        string GetDescription();

        string GetSelectionInfo(World world);
    }
}
