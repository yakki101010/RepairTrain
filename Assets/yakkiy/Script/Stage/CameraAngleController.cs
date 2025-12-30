using UnityEngine;

public class CameraAngleController : MonoBehaviour
{
    [SerializeField] float minAngle = 10;
    [SerializeField] float maxAngle = 90;

    [SerializeField] float angle = 30;

    InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        if(inputManager.RightClick)
        {
            angle = Mathf.Clamp(angle -= inputManager.MouseMove.y, minAngle, maxAngle);

            transform.rotation = Quaternion.AngleAxis(angle , Vector3.right);
        }
    }
}
