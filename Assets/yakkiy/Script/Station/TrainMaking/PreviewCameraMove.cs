using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreviewCameraMove : MonoBehaviour
{
    [Header("カメラの位置")]
    [SerializeField] Transform cameraStand;

    [Header("カメラ上下の向き範囲")]
    [SerializeField] float pitchUpMax;
    [SerializeField] float pitchDownMax;

    [Header("カメラの移動範囲")]
    [SerializeField] Vector3 posMin;
    [SerializeField] Vector3 posMax;

    [Header("カメラの移動速度")]
    [SerializeField] float moveSpeed;

    CameraManager cameraManager;

    Vector2 lookAxis;
    float mouseWheel;

    float yaw;//Y回転
    float pitch;//X回転


    bool isDuringOperation;//何かしらの操作中
    bool isRotationOperation;//カメラ回転操作中
    bool isMoveOperation;//カメラ移動操作中

    bool rightClick;//右クリック
    bool wheelClick;//マウスホイール押し込み

    private void Awake()
    {
        cameraManager = Camera.main.gameObject.GetComponent<CameraManager>();
    }

    private void Start()
    {
        pitch = transform.rotation.eulerAngles.x;
        yaw = transform.rotation.eulerAngles.y;

        CameraIsMine();
    }

    private void Update()
    {
        InputBranching();
    }

    public void OnLook(InputValue inputValue)
    {
        lookAxis = inputValue.Get<Vector2>();

        //Debug.Log($"入力始点操作{inputValue}");
    }

    public void OnMouseWheel(InputValue inputValue)
    {
        mouseWheel = inputValue.Get<Vector2>().y;

        Vector3 forwardVector = transform.forward;
        forwardVector *= mouseWheel * moveSpeed;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x + forwardVector.x, posMin.x, posMax.x), Mathf.Clamp(transform.position.y + forwardVector.y, posMin.y, posMax.y), Mathf.Clamp(transform.position.z + forwardVector.z, posMin.z, posMax.z));


    }

    public void OnRightClickDown()
    {
        rightClick = true;
    }

    public void OnRightClickUp()
    {
        rightClick = false;

        if(isRotationOperation)//カメラ回転の操作中だったら終わったことを伝える
        {
            isRotationOperation = false;
            isDuringOperation = false;
        }
    }

    public void OnWheelClickDown()
    {
        wheelClick = true;

        //Debug.Log("マウスホイール押し込み");
    }

    public void OnWheelClickUp()
    {
        //Debug.Log("マウスホイール押し込みやめ");

        wheelClick = false;

        if (isMoveOperation)//カメラ移動の操作中だったら終わったことを伝える
        {
            isMoveOperation = false;
            isDuringOperation = false;
        }
    }

    /// <summary>
    /// 操作の分岐
    /// </summary>
    void InputBranching()
    {
        //if (isDuringOperation) return;

        if (rightClick) StartingPointOperation();//カメラ回転操作

        if (wheelClick) CameraMovement();//カメラ始点の移動操作

    }
    /// <summary>
    /// 始点を回転
    /// </summary>
    void StartingPointOperation()
    {
        isDuringOperation = true;
        isRotationOperation = true;


        yaw += lookAxis.x;
        pitch = Mathf.Clamp(pitch - lookAxis.y, pitchDownMax, pitchUpMax);

        Quaternion yawRot = Quaternion.AngleAxis(yaw, Vector3.up);
        Quaternion pitchRot = Quaternion.AngleAxis(pitch, Vector3.right);

        transform.rotation = yawRot * pitchRot;

        //transform.rotation = Quaternion.Euler(pitch, yaw, 0);

    }

    /// <summary>
    /// カメラ始点の移動操作
    /// </summary>
    void CameraMovement()
    {
        isDuringOperation = true;
        isMoveOperation = true;

        Vector3 upVector = transform.up;
        Vector3 rightVector = transform.right;

        upVector *= lookAxis.y;
        rightVector *= lookAxis.x;

        Vector3 vector = (upVector + rightVector).normalized;

        //Debug.Log($"上下{upVector}:左右{rightVector}:結果{vector}");

        transform.position = new Vector3(Mathf.Clamp(transform.position.x - (vector.x * moveSpeed) , posMin.x ,posMax.x), Mathf.Clamp(transform.position.y - (vector.y * moveSpeed), posMin.y, posMax.y), Mathf.Clamp(transform.position.z - (vector.z * moveSpeed), posMin.z, posMax.z));
    }

    /// <summary>
    /// カメラを自分に従うよう要求する
    /// </summary>
    void CameraIsMine()
    {
        if (cameraManager != null) cameraManager.SetTarget(cameraStand);
    }

#if UNITY_EDITOR

    [ContextMenu("メインカメラに自分を追従対象にさせる")]
    void DebugCameraPos()//デバック用カメラ位置確認用
    {
        Camera.main.transform.position = cameraStand.position;
        Camera.main.transform.rotation = cameraStand.rotation;

        CameraIsMine();
    }

#endif
}
