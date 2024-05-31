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
		if(statsModel == null) return;
		var codeValue = statsModel.GetStat(StatsTypes.Code);
		var codeMark = codeValue > -1 ? "+" : "";
		var qaValue = statsModel.GetStat(StatsTypes.QA);
		var qaMark = qaValue > -1 ? "+" : "";
		var artValue = statsModel.GetStat(StatsTypes.Art);
		var artMark = artValue > -1 ? "+" : "";
		var audioValue = statsModel.GetStat(StatsTypes.Audio);
		var audioMark = audioValue > -1 ? "+" : "";
		var designValue = statsModel.GetStat(StatsTypes.Design);
		var designMark = designValue > -1 ? "+" : "";
		codingValueLabel.text = $"{codeMark}{codeValue}";
		qaValueLabel.text = $"{qaMark}{qaValue}";
		artValueLabel.text = $"{artMark}{artValue}";
		audioValueLabel.text = $"{audioMark}{audioValue}";
		designValueLabel.text = $"{designMark}{designValue}";
	}
}
