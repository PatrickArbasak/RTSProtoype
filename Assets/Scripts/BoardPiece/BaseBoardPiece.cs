using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoardPiece : HealthBoardPiece , ISelectable
{
    [SerializeField] int baseCost;
    public int BaseCost {get { return baseCost; }set { baseCost = value; }}

    public override void Selected()
    {
        base.Selected();
    }

    public override void UnSelected()
    {
        base.UnSelected();
    }
}
