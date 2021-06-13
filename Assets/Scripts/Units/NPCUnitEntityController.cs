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
            
            unitEntity.OnVisionUpdate += UpdateVisibility;
            unitEntity.UpdateVision();

            if (intelligence == null)
            {
                SimpleStateMachine sm = new SimpleStateMachine();
                SimpleSurveyer surveyer = new SimpleSurveyer();
                SimplePlanner planner = new SimplePlanner();
                intelligence = new NPCIntelligence(gui.Game, sm, surveyer, planner, unitEntity.Position);
                intelligence.AssignUnitEntity(unitEntity);
            }
        }

        private void UpdateVisibility()
        {
            if (world.IsVisibleTile(unitEntity.Position))
                Show();
            else
                Hide();
        }
    }
}
