using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;


    bool forwardAccele;//前進入力
    public bool ForwardAccele {  get { return forwardAccele; } }

    bool backAccele;//後進入力
    public bool BackAccele { get {return backAccele; } }

    bool rightClick;//右クリック入力
    public bool RightClick { get { return rightClick; } }
    Action RightClickDown;//入力時に呼ばれるコールバック
    /// <summary>
    /// 右クリック入力時に呼ばれるコールバックを登録
    /// </summary>
    public void CallbackRightClickDown(Action action)
    {
        RightClickDown += action;
    }
    Action RightClickUp;//左クリックを離した時に呼ばれるコールバック
    /// <summary>
    /// 右クリックを離した時に呼ばれるコールバックを登録
    /// </summary>
    public void CallbackRightClickUp(Action action)
    {
        RightClickUp += action;
    }


    bool leftClick;//左クリック入力
    public bool LeftClick {  get { return leftClick; } }
    Action LeftClickDown;//入力時に呼ばれるコールバック
    /// <summary>
    /// 左クリック入力時に呼ばれるコールバックを登録
    /// </summary>
    public void CallbackLeftClickDown(Action action)
    {
        LeftClickDown += action;
    }

    Action LeftClickUp;//左クリックを離した時に呼ばれるコールバック
    /// <summary>
    /// 左クリックを離した時に呼ばれるコールバックを登録
    /// </summary>
    public void CallbackLeftClickUp(Action action)
    {
        LeftClickUp += action;
    }

    Vector2 mousePos;//マウス位置
    public Vector2 MousePos {  get { return mousePos; } }

    Vector2 mouseMove;//マウスの移動量

    public Vector2 MouseMove { get { return mouseMove; } }

    private void Awake()
    {
        Singleton();
    }
    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnLeftClickDown()
    {
        //Debug.Log("左クリック開始");
        leftClick = true;
        if(LeftClickDown != null) LeftClickDown();
    }

    void OnLeftClickUp()
    {
        //Debug.Log("左クリックやめ");
        leftClick = false;
        if(LeftClickUp != null) LeftClickUp();
    }

    void OnRightClickDown()
    {
        //Debug.Log("右クリック開始");
        rightClick = true;
        if (RightClickDown != null) RightClickDown();
    }

    void OnRightClickUp()
    {
        //Debug.Log("右クリックやめ");
        rightClick = false;
        if (RightClickUp != null) RightClickUp();
    }


    void OnForwardDown()
    {
        //Debug.Log("前進入力");
        forwardAccele = true;
    }

    void OnForwardUp()
    {
        //Debug.Log("前進入力やめ");
        forwardAccele = false;
    }

    void OnBackDown()
    {
        backAccele = true;
    }

    void OnBackUp()
    {
        backAccele = false;
    }

    void OnMousePosition(InputValue inputValue)
    {
        mousePos = inputValue.Get<Vector2>();
    }

    void OnMouseMove(InputValue inputValue)
    {
        mouseMove = inputValue.Get<Vector2>();
    }
}
