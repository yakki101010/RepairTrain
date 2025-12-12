using UnityEngine;

[CreateAssetMenu(fileName = "Tiles", menuName = "Scriptable Objects/レールタイルのデータ")]
public class Tiles : ScriptableObject
{
    public Rails plainSeries;//平原タイル
}

[System.Serializable]
public class Rails
{
    public GameObject[] straightRails;//直進レール系

    public GameObject GetTile()
    {
        return straightRails[Random.Range(0, straightRails.Length)];
    }
}
