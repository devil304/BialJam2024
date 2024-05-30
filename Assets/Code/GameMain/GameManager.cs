using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MainInput MainInput => _mainInput;
    public StatsModel StatsAct { get; private set; } // Aktualne staty gry
    public StatsModel StatsTeam { get; private set; } //Łączne staty zespołu
    public List<CharacterModel> Team { get; private set; }

    MainInput _mainInput;

    private void Awake()
    {
        Instance = this;
        _mainInput = new();
        _mainInput.Enable();
        _mainInput.Main.Enable();
        StatsAct = new();
        StatsTeam = new();
    }

    public void StartGame()
    {
        StatsAct.Reset();
        StatsTeam.Reset();
    }

    public void AddTeamMember(CharacterModel characterModel)
    {
        Team.Add(characterModel);
    }

    public void FinishSelectingTeam()
    {
        foreach (var character in Team)
        {
            StatsTeam.StatsModify(character.CharStats);
        }
        StatsTeam.Normalize();
    }

    public void ModifyStats((float, float, float, float, float) statsMod)
    {
        StatsAct.StatsModify(statsMod);
    }

    public void ModifyStats(StatsModel statsMod)
    {
        StatsAct.StatsModify(statsMod);
    }
}
