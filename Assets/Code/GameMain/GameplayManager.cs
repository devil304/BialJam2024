using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Slider[] _statSliders;
    [SerializeField] float _passivePointsSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameManager.I.ModifyStatsBasedOnTeam(_passivePointsSpeed * Time.deltaTime);
        for (int i = 0; i < _statSliders.Length; i++)
        {
            _statSliders[i].value = GameManager.I.StatsAct.GetStat((StatsTypes)i);
        }
    }
}
