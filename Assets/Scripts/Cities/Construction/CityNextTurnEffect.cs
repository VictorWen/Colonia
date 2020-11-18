using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cities.Construction
{
    public interface CityNextTurnEffect
    {
        void OnNextTurn(City city, GameMaster game);
    }
}
