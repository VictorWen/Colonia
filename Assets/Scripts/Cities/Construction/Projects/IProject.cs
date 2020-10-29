using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cities.Construction
{
    public interface IProject
    {
        string Type { get; }

        void OnSelect(City city, GUIMaster gui);

        void OnDeselect(City city, GUIMaster gui);

        void Complete(City city, GUIMaster gui);

        IProject Copy();

        string GetDescription();

        string GetSelectionInfo(GUIMaster gui);
    }
}
