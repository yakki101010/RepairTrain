using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] Level[] levels;
    public Level[] Levels {  get { return levels; } }
}

[System.Serializable]
public class Level
{
    [SerializeField] int level;
    public int _Level { get { return level; } }

    [SerializeField] RailTile tile;
    public RailTile Tile { get { return tile; } }

    [SerializeField] int length;//ステージの全長
    public int Length { get { return length; } }

    [SerializeField] int zombieHP;//ゾンビ体力
    public int ZombieHP { get { return zombieHP; } }

    [SerializeField] int zombieStrange;//ゾンビ攻撃力
    public int ZombieStrange { get { return zombieStrange; } }

    [SerializeField] int zombieNum;//1秒当たりのゾンビ出現数(列車移動中はスポーン間隔が短くなる)
    public int ZombieNum { get { return zombieNum; } }

    //[SerializeField] int eliteZombieNum;//このステージに出現するエリートゾンビの数

    //public int EliteZombieNum { get {return eliteZombieNum; } }
}
