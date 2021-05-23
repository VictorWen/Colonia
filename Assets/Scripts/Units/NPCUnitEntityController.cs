using UnityEngine;
using Units.Intelligence;

namespace Units
{
    public class NPCUnitEntityController : BaseUnitEntityController
    {
        [SerializeField] private NPCIntelligence intelligence;

        override protected void Start()
        {
            base.Start();
            if (intelligence == null)
            {
                SimpleSurveyer surveyer = new SimpleSurveyer();
                SimplePlanner planner = new SimplePlanner();
                intelligence = new NPCIntelligence(gui.Game, surveyer, planner);
                intelligence.AssignUnitEntity(unitEntity);
            }
        }
    }
}
