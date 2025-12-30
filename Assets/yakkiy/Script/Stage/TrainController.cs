using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrainController : MonoBehaviour
{
    [SerializeField] Train train;

    float speed = 0;

    InputManager inputManager;

    int clungNum;//まとわりついたゾンビの数

    int maxClungNum;//引っ張れる上限
    int speedD​downClungNum;//引っ張っていると減速がかかる数

    private void Start()
    {
        inputManager = InputManager.Instance;

        maxClungNum = Train.Instance.parameter.PullingForce;//ココ以外でも使うなら一度変数にいれる

        speedDdownClungNum = (int)(maxClungNum * 0.5f);

        StartCoroutine(TimeBreak());
    }

    private void Update()
    {
        Speed​Control();

        RouteGenerator.Instance.SetSpeed(speed);
    }

    void Speed​Control()
    {
        const float DECELERATION = 0.1f;
        const float STOP_ERROR = 0.05f;


        if(clungNum > maxClungNum)//大量のゾンビにまとわりつかれていたら強制ブレーキ
        {
            Brake();
            //Neutral();
            return;
        }

        if(inputManager.ForwardAccele && inputManager.BackAccele)//ブレーキ
        {
            Brake();
        }
        else if((inputManager.ForwardAccele && speed < 0) || (inputManager.BackAccele && speed > 0))//ブレーキ
        {
            Brake();
        }
        else if (inputManager.ForwardAccele)//行進
        {
            if (speed < train.parameter.MaxSpeed)
            {
                Accelerator(train.parameter.Acceleration * Time.deltaTime);
            }
        }
        else if (inputManager.BackAccele)//後退
        {
            if (speed > -train.parameter.MaxSpeed)
            {
                Accelerator(-train.parameter.Acceleration * Time.deltaTime);
            }
        }
        else//操作なし
        {
            Neutral();
        }

        void Neutral()//入力無し徐々に減速する
        {
            if (speed > STOP_ERROR || speed < -STOP_ERROR)
            {
                speed += speed > 0 ? -(DECELERATION * Time.deltaTime) : (DECELERATION * Time.deltaTime);
            }
            else
            {
                speed = 0;
            }
        }

        void Brake()//急激に減速する
        {
            const float BRAKE_POWER = 0.5f;

            if (speed != 0)
            {
                speed += speed > 0 ? -(BRAKE_POWER * Time.deltaTime) : (BRAKE_POWER * Time.deltaTime);
            }
        }

        void Accelerator(float value)
        {
            const float DOWN_SPEED = 0.1f;//減速する場合の強さ

            if (clungNum > speedDdownClungNum) value *= DOWN_SPEED;

            speed += value;
        }
    }
    

    /// <summary>
    /// 列車に張り付く(張り付きCountを1増やす)
    /// </summary>
    public void Stick()
    {
        clungNum++;
    }

    /// <summary>
    /// 列車に張り付く(張り付きCountをN増やす)
    /// </summary>
    public void Stick(int n)
    {
        clungNum += n;
    }

    /// <summary>
    /// 列車から離れる(張り付きCountを1減らす)
    /// </summary>
    public void Leave()
    {
        clungNum--;
    }

    /// <summary>
    /// 列車から離れる(張り付きCountをN減らす)
    /// </summary>
    public void Leave(int n)
    {
        clungNum -= n;
    }

    /// <summary>
    /// ゾンビによる妨害誤作動防止用
    /// 仮に誤作動でゾンビがついていないのにカウントが0にならなかった時のために時間経過でも減るようにしておく
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeBreak()
    {
        const float TIME = 30;

        while (true)
        {
            if(clungNum > 0)
            {
                clungNum--;
            }

            yield return new WaitForSeconds(TIME);
        }
    }
}
