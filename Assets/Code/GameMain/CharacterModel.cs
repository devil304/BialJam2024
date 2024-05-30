using UnityEngine;

public class CharacterModel
{
    public StatsModel CharStats { get; private set; }
    public string NickName { get; private set; }

    public CharacterModel()
    {
        CharStats = new();
        CharStats.GenerateRandom();
        NickName = DataObjectAccess.GetNick();
    }
}
