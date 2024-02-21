using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图形
/// </summary>
public class Shape : MonoBehaviour
{
	private bool isPause = false;

	private float timer = 0;		//计时器
	private float stepTime = 0.8f;  //下落间隔
	private int multiple = 1500;    //加速速率

	private Controller ctrl;

	private Transform pivot;    //旋转点


    private Vector2 touchStartPos;  //触屏开始坐标
    public float moveStep = 100;      //移动间距步长
    private float delMove = 0;      //移动间距
    private bool isMoveHor = false;		//水平移动
    private bool isMoveDown = false;	//向下移动

    private bool touchDown = false;	//手指按下
    private bool touchUp = false;   //手指抬起
    private bool touchMove = false;	//手指滑动

    private void Awake()
    {
		pivot = transform.Find("Pivot");
    }

    private void Update()
    {
        if (isPause) return;

		timer += Time.deltaTime;
		if(timer > stepTime)
		{
			timer = 0;
			Fall();
		}

		InputControlInMobile();

    }


    public void Init(Color color, Controller ctrl)
	{
		this.ctrl = ctrl;

		foreach (Transform t in transform)
		{
			if (t.CompareTag("Block"))
			{
				t.GetComponent<SpriteRenderer>().color = color;
			}
		}
	}

	private void Fall()
	{
		Vector3 pos = transform.position;
		pos.y -= 1;
		transform.position = pos;

		if(ctrl.model.IsValidMapPosition(this.transform) == false)
		{
			//下落位置不可用
			pos.y += 1;		//返回向上一格
			transform.position = pos;

			isPause = true;		//停止下落

			bool isLineClear = ctrl.model.PlaceShape(this.transform);	//是否消除行
			if(isLineClear)
			{
				ctrl.audioManager.PlayLineClear();
			}
			ctrl.gameManager.FallDown();
			return;
		}

        ctrl.audioManager.PlayDrop();
    }

	private void InputControl()
	{
		float h = 0;
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			h = -1;
		}
		else if( Input.GetKeyDown(KeyCode.RightArrow))
		{
			h = 1;
		}
		//左右移动
		if(h != 0)
		{
			Vector3 pos = transform.position;
			pos.x += h;
			transform.position = pos;
			if (ctrl.model.IsValidMapPosition(this.transform) == false)
			{
				pos.x -= h;
				transform.position = pos;
			}
			else
			{
				ctrl.audioManager.PlayBalloon();
			}
		}

		//旋转
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			transform.RotateAround(pivot.position, Vector3.forward, -90);
            if (ctrl.model.IsValidMapPosition(this.transform) == false)
			{
                transform.RotateAround(pivot.position, Vector3.forward, 90);
            }
			else
			{
                ctrl.audioManager.PlayBalloon();
            }
        }

		//加速
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			//按下加速
			stepTime /= multiple;
		}
	}

	//移动端输入控制
	private void InputControlInMobile()
	{
		//旋转
		if (touchDown && touchUp && !touchMove)
		{
            transform.RotateAround(pivot.position, Vector3.forward, -90);
            if (ctrl.model.IsValidMapPosition(this.transform) == false)
            {
                transform.RotateAround(pivot.position, Vector3.forward, 90);
            }
            else
            {
                ctrl.audioManager.PlayBalloon();
            }
            touchDown = false;
            touchUp = false;
            touchMove = false;
        }

		IsMove();
	}

    //暂停
    public void Pause()
	{
		isPause = true;
	}
	//继续
	public void Resume()
	{
		isPause = false;
	}

    private void IsMove()
	{
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
					touchStartPos = touch.position;
					delMove = 0;
                    touchDown = true;
                    touchUp = false;
					touchMove = false;
                    break;

                case TouchPhase.Moved:
                    if(!isMoveDown && Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
					{
						isMoveHor = true;
						touchMove = true;
						delMove += touch.position.x - touchStartPos.x;
						if(delMove > moveStep)
						{
							Move(1);
							delMove = 0;
						}
						else if(delMove < -moveStep)
						{
							Move(-1);
							delMove = 0;
                        }
					}
					if(!isMoveHor && Mathf.Abs(touch.deltaPosition.x) < Mathf.Abs(touch.deltaPosition.y))
					{
						isMoveDown = true;
						touchMove = true;
						delMove += touch.position.y - touchStartPos.y;
						if(delMove < -moveStep)
						{
                            stepTime /= multiple;
							delMove = 0;
                        }
					}
					touchStartPos = touch.position;
                    break;
				case TouchPhase.Ended:
					isMoveHor = false;
					isMoveDown = false;
					delMove = 0;
                    touchUp = true;
					break;
            }
        
		}
    }

	private void Move(int h)
	{
        Vector3 pos = transform.position;
        pos.x += h;
        transform.position = pos;
        if (ctrl.model.IsValidMapPosition(this.transform) == false)
        {
            pos.x -= h;
            transform.position = pos;
        }
        else
        {
            ctrl.audioManager.PlayBalloon();
        }
    }
}
