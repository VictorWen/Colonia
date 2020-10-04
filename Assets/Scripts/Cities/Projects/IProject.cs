using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProject
{
    string Type { get; }

    void Complete(City city);

    void OnSelect(City city);

    void OnDeselect(City city);

    IProject Copy();

    string GetDescription();
}
