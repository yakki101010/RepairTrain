using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainStructure", menuName = "Scriptable Objects/TrainStructure")]
public class TrainStructure : ScriptableObject
{
    [Header("車両")]
    public List<PartObject> bogie = new List<PartObject>();

}

[System.Serializable]
public class PartObject
{
    public PartObject parent;
    [SerializeField] List<PartObject> childPart = new List<PartObject>();
    public List<PartObject> ChildPart {  get { return childPart; } }

    public void AddChild(PartObject child)
    {
        child.parent = this;
        childPart.Add(child);
    }

    public PartProperty partProperty;

    public Vector3 pos = Vector3.zero;
    public Quaternion rot = Quaternion.identity;
}
