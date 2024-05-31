using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }
    public MainInput MainInput => _mainInput;
    public StatsModel StatsAct { get; private set; } // Aktualne staty gry
    public StatsModel StatsTeam { get; private set; } //Łączne staty zespołu
    public List<CharacterModel> Team { get; private set; } = new();
    [SerializeField]
    List<GameObject> _minigamesPrefabs;
    [SerializeField]
    int _gameplaySceneIndex = 2;
    public List<IMinigame> _minigames;

    MainInput _mainInput;

    [SerializeField] bool _debug;

    private void Awake()
    {
        if (I != null) return;
        I = this;
        _mainInput = new();
        _mainInput.Enable();
        _mainInput.Main.Enable();
        StatsAct = new();
        StatsTeam = new();
        if (_debug && Team.Count <= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                var teamMember = new CharacterModel();
                teamMember.GenerateRandom();
                AddTeamMember(teamMember);
            }
            FinishSelectingTeam();
        }
        _minigames = _minigamesPrefabs.Select(mp => mp.GetComponent<IMinigame>()).ToList();
        DontDestroyOnLoad(gameObject);
        DataObjectAccess.ClearNicks();
    }

    private void OnEnable()
    {
        if (I == null)
            Awake();
    }

    private void OnDestroy()
    {
        I = null;
        _mainInput.Main.Disable();
        _mainInput.Disable();
        _mainInput.Dispose();
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
        SceneManager.LoadScene(_gameplaySceneIndex);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenu();
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void ModifyStats((float, float, float, float, float) statsMod)
    {
        StatsAct.StatsModify(statsMod);
    }
    public void ModifyStats(float allVal)
    {
        StatsAct.StatsModify(allVal);
    }

    public void ModifyStatsBasedOnTeam(float allVal)
    {
        for (int i = 0; i < StatsAct.Stats.Length; i++)
        {
            StatsAct.StatModify((StatsTypes)i, allVal * (StatsTeam.Stats[i] / 100f));
        }
    }

    public void ModifyStats(StatsModel statsMod)
    {
        StatsAct.StatsModify(statsMod);
    }
}
