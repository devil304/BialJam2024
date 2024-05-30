using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainer")]
public class DataContainer : ScriptableObject
{
    public string[] NickNames;
    [Range(0, 200)] public int CharMinStatsSum;
    [Range(0, 200)] public int CharMaxStatsSum;
}
