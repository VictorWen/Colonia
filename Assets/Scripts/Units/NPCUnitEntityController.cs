using UnityEngine;
using Units.Intelligence;
using Units.Loot;

namespace Units
{
    public class NPCUnitEntityController : BaseUnitEntityController
    {
        [SerializeField] private NPCIntelligence intelligence;
        [SerializeField] private LootTableSO loot;

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
            if (loot != null)
                unitEntity.AssignLootTable(loot.ToLootTable());
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
