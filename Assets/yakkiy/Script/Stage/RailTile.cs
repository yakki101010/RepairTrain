using UnityEngine;

/// <summary>
/// テーマごとのタイルのデータクラス
/// </summary>
[CreateAssetMenu(fileName = "Tiles", menuName = "Scriptable Objects/レールタイルのデータ")]
public class RailTile : ScriptableObject
{
    public Rail plainSeries;//平原タイル
}
/// <summary>
/// パーツ分けされたレールタイルのデータ
/// </summary>
[System.Serializable]
public class Rail
{
    public GameObject startRail;
    public GameObject goalRail;

    public GameObject[] straightRails;//直進レール系

    public GameObject GetTile()
    {
        return straightRails[Random.Range(0, straightRails.Length)];
    }
}
