using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    /*  The Surveyer calculates scores for positioning and provides danger, target, and efficiency scores for each nearby enemy.
     */
    public interface INPCSurveyer
    {
        // Look through as movable tiles and calculate positioning score of that tile
        Dictionary<Vector3Int, float> SurveyPositioning(UnitEntity self, World world);

        Dictionary<Vector3Int, float> SurveyWandering(UnitEntity self, World world);

/*        int CalculateDangerScore(UnitEntity self, UnitEntity other);

        int CalculateTargetScore(UnitEntity self, UnitEntity other);

        int CalculateEfficienyScore(UnitEntity self, UnitEntity other);*/
    }
}
