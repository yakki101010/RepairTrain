using UnityEngine;

[CreateAssetMenu(fileName = "PartsData", menuName = "Scriptable Objects/PartsData")]
public class PartsData : ScriptableObject
{
    [SerializeField] PartProperty leadingBogie;
    public PartProperty LeadingBogie {get {return leadingBogie;}}

    [SerializeField] PartProperty trailingBogie;
    public PartProperty TrailingBogie {get {return trailingBogie;}}

   [SerializeField] PartProperty[] partProperties;
    public PartProperty[] PartProperties { get {return partProperties;}}
}

