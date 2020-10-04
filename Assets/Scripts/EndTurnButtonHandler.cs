using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonHandler : MonoBehaviour
{
    //public CapitalCity capital;
    public Text text;

    public void endTurn()
    {
        //capital.GetInventory().AddResourceCount(EquipmentSlotID.stone, 10);
        //Debug.Log(capital.GetInventory().GetResourceCount(EquipmentSlotID.stone));
        //text.text = capital.GetInventory().GetResourceCount(EquipmentSlotID.stone).ToString();
    }
}
