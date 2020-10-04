using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingEffect
{
    public bool OnComplete { get; private set; }

    public BuildingEffect(bool onComplete = true)
    {
        OnComplete = onComplete;
    }

    public abstract void Apply(City e);

    public abstract void Remove(City e);

    public abstract override string ToString();
}
