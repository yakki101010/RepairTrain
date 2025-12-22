using UnityEngine;
using UnityEngine.InputSystem;

public class TrainController : MonoBehaviour
{
    [SerializeField] Train train;

    bool forwardAccele;
    bool backAccele;

    float speed = 0;

    public void OnForwardDown()
    {
        //Debug.Log("前進入力");
        forwardAccele = true;
    }

    public void OnForwardUp()
    {
        //Debug.Log("前進入力やめ");
        forwardAccele = false;
    }

    public void OnBackDown()
    {
        backAccele = true;
    }

    public void OnBackUp()
    {
        backAccele = false;
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

        if(forwardAccele && backAccele)//ブレーキ
        {
            Brake();
        }
        else if((forwardAccele && speed < 0) || (backAccele && speed > 0))//ブレーキ
        {
            Brake();
        }
        else if (forwardAccele)//行進
        {
            if (speed < train.parameter.MaxSpeed)
            {
                speed += (train.parameter.Acceleration * Time.deltaTime);
            }
        }
        else if (backAccele)//後退
        {
            if (speed > -train.parameter.MaxSpeed)
            {
                speed -= (train.parameter.Acceleration * Time.deltaTime);
            }
        }
        else//操作なし
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
    }

    void Brake()
    {
        const float BRAKE_POWER = 0.5f;

        if (speed != 0)
        {
            speed += speed > 0 ? -(BRAKE_POWER * Time.deltaTime) : (BRAKE_POWER * Time.deltaTime);
        }
    }
}
