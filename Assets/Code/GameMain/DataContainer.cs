using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainer")]
public class DataContainer : ScriptableObject
{
    public string[] NickNames;
    [Range(0, 500)] public int CharMinStatsSum;
    [Range(0, 500)] public int CharMaxStatsSum;
}
