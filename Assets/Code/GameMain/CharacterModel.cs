using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public StatsModel CharStats { get; private set; }
    public string NickName { get; private set; }
    public List<Sprite> Sprites { get; private set; } = new List<Sprite>();

    public CharacterModel()
    {
        GenerateRandom();
    }

    public void GenerateRandom()
    {
        CharStats = new();
        CharStats.GenerateRandom();
        NickName = DataObjectAccess.GetNick();
    }

    public void AddSprite(Sprite sprite)
    {
        Sprites.Add(sprite);
    }
}
