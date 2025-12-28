using UnityEngine;

[CreateAssetMenu(fileName = "PartProperty", menuName = "Scriptable Objects/PartProperty")]
public class PartProperty : ScriptableObject
{
    public string name;
    public GameObject stagePrefab;
    public GameObject makingPrefab;
    public GameObject CraftUIPrefab;
}
