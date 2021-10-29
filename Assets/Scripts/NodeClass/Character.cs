using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    public Status status;
    public string name;
    int id;

    public abstract void ResetStat();
    public abstract void OriginSkill();
}

public class Char1 : Character
{
    public override void ResetStat()
    {
        status = new Status();
        status.MaxHp = 120;
        status.Hp = status.MaxHp;
        status.Speed = 10;
        status.Attack = 150;
        status.Defense = 20;
        status.Action = 10;
    }

    public override void OriginSkill()
    {

    }
}