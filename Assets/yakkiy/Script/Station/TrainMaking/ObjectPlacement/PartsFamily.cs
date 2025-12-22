
using System.Collections.Generic;
using UnityEngine;

public class PartsFamily : MonoBehaviour
{

    [SerializeField] PartProperty partProperty;
    public PartProperty PartProperty { get { return partProperty; } }

    PartsFamily parent;
    public PartsFamily Parent { get { return parent; } }

    List<PartsFamily> child = new List<PartsFamily> ();
    public List<PartsFamily> Child {  get { return child; } }

    /// <summary>
    /// 親を指定
    /// </summary>
    /// <param name="addParent"></param>
    public void SetParent(PartsFamily addParent)
    {
        if(parent != null) parent.child.Remove(this);

        addParent.child.Add (this);
        parent = addParent;
    }

    /// <summary>
    /// 親パーツとの縁を切る
    /// </summary>
    public void RemoveParent()
    {
        if(parent == null)
        {
            Debug.LogWarning("私に親はいません");

            return;
        }

        parent.child.Remove (this);
        parent = null;
    }

    /// <summary>
    /// 子パーツを自分から切り離す
    /// </summary>
    public void RemoveChildren()
    {
        if (child.Count <= 0)
        {
            Debug.LogWarning("私に子はいません");
            return;
        }

        for (int i = 0; i < child.Count; i++)
        {
            child[i].RemoveParent();
        }
    }
}
