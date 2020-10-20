using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProject
{
    string Type { get; }

    void Complete(City city, GUIMaster game);

    void OnSelect(City city, GUIMaster game);

    void OnDeselect(City city, GUIMaster game);

    IProject Copy();

    string GetDescription();
}
