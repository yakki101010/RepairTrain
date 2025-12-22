using System;
using UnityEngine;

public class ColliderCollisionDetection : MonoBehaviour
{
    [SerializeField] MakingPart makingPart;

    Action<Collision> onCollisionStay;
    Action<Collision> onCollisionExit;

    private void Start()
    {
        onCollisionExit += makingPart.ReceiveOnCollisionExit;
        onCollisionStay += makingPart.ReceiveOnCollisionStay;
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay(collision);
    }
}
