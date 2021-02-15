using System;
using UnityEngine;

namespace Colonia
{
    public class EndTurnEventHandler : ScriptableObject
    {
        public event Action OnEndTurn;
    }
}
