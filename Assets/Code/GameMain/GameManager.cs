using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MainInput MainInput => _mainInput;
    public StatsModel StatsAct { get; private set; } // Aktualne staty gry
    public StatsModel StatsTeam { get; private set; } //Łączne staty zespołu

    MainInput _mainInput;

    private void Awake()
    {
        Instance = this;
        _mainInput = new MainInput();
        _mainInput.Enable();
        _mainInput.Main.Enable();
        StatsAct = new StatsModel();
        StatsTeam = new StatsModel();
    }

    public void StartGame()
    {
        StatsAct.Reset();
        StatsTeam.Reset();
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
