using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpManager : MonoBehaviour
{
    Character Red;
    Character Blue;

    public void HpUpdate()
    {
        if (GameManager.instance.tile.number >= 10) TurnChange();
    }

    public void HpSet()
    {
        CharSet();
        FirstAttack();
    }

    public void EndAction()
    {
        if (GameManager.instance.tile.number < 10) Invoke("EndSlide", 0.1f);
    }

    void FirstAttack()
    {
        if (Red.status.Speed < Blue.status.Speed) GameManager.instance.tile.turn = TileManager.TURN.blue;
        else if (Red.status.Speed > Blue.status.Speed) GameManager.instance.tile.turn = TileManager.TURN.red;
        else GameManager.instance.tile.turn = (TileManager.TURN)Random.Range(0, 2);
    }

    void CharSet()
    {
        Red = new Char1();
        Red.ResetStat();
        Blue = new Char1();
        Blue.ResetStat();
    }

    void TurnChange()
    {
        int val = GameManager.instance.tile.TurnChange();
        if (GameManager.instance.tile.turn == TileManager.TURN.red) Red.status.Hp -= val;
        else Blue.status.Hp -= val;
        EndAction();

        Debug.Log("blue:" + Blue.status.Hp + "red:" + Red.status.Hp);
    }

    void EndSlide()
    {
        GameManager.instance.tile.IsSwiping = 0;
    }
}
