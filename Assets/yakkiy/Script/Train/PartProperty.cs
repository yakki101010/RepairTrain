using UnityEngine;

[CreateAssetMenu(fileName = "PartProperty", menuName = "Scriptable Objects/PartProperty")]
public class PartProperty : ScriptableObject
{
    public GameObject stagePrefab;
    public GameObject makingPrefab;

    public CraftUIParameter craftUI;
}

[System.Serializable]
public class CraftUIParameter
{
    [Header("名前")]
    public string name;
    [Header("イメージ画像")]
    public Sprite image;
    [Header("説明")]
    [TextArea]
    public string explanation;
    [Header("要求パーツ量")]
    public int price;
}
