using System;

namespace Units
{
    public class UnitPanel
    {
        public event Action OnSelect;
        public event Action OnDeselect;

        public TempUnitEntity SelectedUnit { get; private set; }

        public void SetSelectedUnit(TempUnitEntity newSelectedUnit)
        {
            if (newSelectedUnit != null)
            {
                if (SelectedUnit != null)
                {
                    SelectedUnit.Deselect();
                }
                SelectedUnit = newSelectedUnit;
                OnSelect?.Invoke();
            }
            else
            {
                SelectedUnit = null;
                OnDeselect?.Invoke();
            }
        }

        public void MoveAction()
        {
            SelectedUnit.MoveAction();
        }

        public void AttackAction()
        {
            SelectedUnit.AttackAction();
        }

    }
}
