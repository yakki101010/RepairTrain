using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] Tiles tiles;

    [SerializeField] int length;

    public GameObject[] GetStage()
    {
        GameObject[] prefabs = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            prefabs[i] = tiles.plainSeries.GetTile();
        }

        return prefabs;
    }
}

