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

    bool leftClick;//右クリック入力
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
}
