using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterModel
{
    public StatsModel CharStats { get; private set; }
    public string NickName;
    public int MainStat;

    public Sprite body;
    public Sprite head;
    public Sprite hair;
    public Sprite accessory;

    public CharacterModel()
    {

    }

    public void GenerateRandom()
    {
        CharStats = new();
        MainStat = CharStats.GenerateRandom();
        NickName = DataObjectAccess.GetNick();
    }


}
