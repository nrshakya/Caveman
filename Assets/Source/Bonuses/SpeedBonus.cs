﻿using System.Collections;
using Caveman.Bonuses;
using Caveman.Players;
using Caveman.Setting;
using UnityEngine;

public class SpeedBonus : BonusBase 
{
    protected override void Effect(PlayerModelBase playerModel)
    {
        base.Effect(playerModel);
        preValue = playerModel.Speed;
        playerModel.Speed = playerModel.Speed*2;
    }

    protected override IEnumerator UnEffect(PlayerModelBase playerModel)
    {
        playerModel.Speed = preValue;
        return base.UnEffect(playerModel);
    }
}
