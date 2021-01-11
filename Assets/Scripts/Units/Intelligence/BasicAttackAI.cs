using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.Abilities;

namespace Units.Intelligence
{
    public class BasicAttackAI : NPCAttackAI
    {
        public string UpdateAI(UnitEntity self, UnitEntity target, World world)
        {
            return null;
        }


        public void CompleteAttackAction(UnitEntity self, World world)
        {

        }

        public Ability GetAbilityTelegraph(UnitEntity self, UnitEntity target, World world)
        {
            if (target != null)
                //TODO: placeholder default UnitEntity attack ability
                return new Ability("attack", "Basic Attack", 0, 1, false, new AbilityEffect[] { new DamageAbilityEffect(0, true) }, new HexAbilityAOE(1));
            else
                return null;
        }
    }
}
