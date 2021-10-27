using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TileManager tile;
    public HpManager hp;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (tile == null) tile = GetComponent<TileManager>();
        tile.TileSet();
        if (hp == null) hp = GetComponent<HpManager>();
        hp.HpSet();
    }

    void Update()
    {
        tile.TileUpdate();
        hp.HpUpdate();
    }
}
