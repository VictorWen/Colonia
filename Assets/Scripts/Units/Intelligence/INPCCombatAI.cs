using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    public interface INPCCombatAI
    {
        void ExecuteCombat(GameMaster game);

        List<Vector3Int> GetTelegraph();
    }
}
