using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI codingValueLabel;
	[SerializeField]
	TextMeshProUGUI qaValueLabel;
	[SerializeField]
	TextMeshProUGUI artValueLabel;
	[SerializeField]
	TextMeshProUGUI audioValueLabel;
	[SerializeField]
	TextMeshProUGUI designValueLabel;

	public void SetupStats(StatsModel statsModel) {
		codingValueLabel.text = statsModel.GetStat(StatsTypes.Code).ToString();
		qaValueLabel.text = statsModel.GetStat(StatsTypes.QA).ToString();
		artValueLabel.text = statsModel.GetStat(StatsTypes.Art).ToString();
		audioValueLabel.text = statsModel.GetStat(StatsTypes.Audio).ToString();
		designValueLabel.text = statsModel.GetStat(StatsTypes.Design).ToString();
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
