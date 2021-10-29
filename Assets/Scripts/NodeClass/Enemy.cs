using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    Status status;
    string name;
    int id;

    public abstract void ResetStat();
    public abstract void OriginSkill();
}
