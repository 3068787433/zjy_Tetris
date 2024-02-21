using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMgr : MonoBehaviour
{
    //手指第一次触摸点的位置
    Vector2 m_scenePos = new Vector2();
    //摄像机
    public Transform cameraTarget;
    void Start()
    {
        //允许多点触摸
        Input.multiTouchEnabled = true;
    }


    void Update()
    {
        //DesktopInput();
        MobileInput();
    }

    //移动端控制摄像机旋转
    private void MobileInput()
    {
        if (Input.touchCount == 0)
            return;

        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                m_scenePos = Input.touches[0].position; //第一次触屏位置
            }
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                //旋转摄像机
                cameraTarget.Rotate(new Vector3(-Input.touches[0].deltaPosition.y, Input.touches[0].deltaPosition.x, 0), Space.Self);
            }

            if (Input.touches[0].phase == TouchPhase.Ended && Input.touches[0].phase != TouchPhase.Canceled)
            {
                Vector2 pos = Input.touches[0].position;

                //判断手指移动
                //水平移动
                if (Mathf.Abs(m_scenePos.x - pos.x) > Mathf.Abs(m_scenePos.y - pos.y))
                {
                    if (m_scenePos.x > pos.x)
                    {
                        Debug.Log("手指向左滑");
                        //TODO:...
                    }
                    else
                    {
                        Debug.Log("手指右滑");
                        //TODO:...
                    }
                }
                else
                {
                    if (m_scenePos.y > pos.y)
                    {
                        Debug.Log("手指下滑");
                        //TODO:...
                    }
                    else
                    {
                        Debug.Log("手指上滑");
                        //TODO:...
                    }
                }
            }
        }//多指逻辑
        else if (Input.touchCount > 1)
        {
            //记录两个手指的位置
            Vector2 finger1 = new Vector2();
            Vector2 finger2 = new Vector2();

            //记录两个手指的移动
            Vector2 mov1 = new Vector2();
            Vector2 mov2 = new Vector2();

            for (int i = 0; i < 2; i++)
            {
                Touch touch = Input.touches[i];

                if (touch.phase == TouchPhase.Ended)
                    break;

                if (touch.phase == TouchPhase.Moved)
                {
                    float mov = 0;
                    if (i == 0)
                    {
                        finger1 = touch.position;
                        mov1 = touch.deltaPosition;
                    }
                    else
                    {
                        finger2 = touch.position;
                        mov2 = touch.deltaPosition;

                        if (finger1.x > finger2.x)
                        {
                            mov = mov1.x;
                        }
                        else
                        {
                            mov = mov2.x;
                        }

                        if (finger1.y > finger2.y)
                        {
                            mov += mov1.y;
                        }
                        else
                        {
                            mov += mov2.y;
                        }
                        cameraTarget.transform.Translate(0, 0, mov * 0.1f);
                    }
                }

            }
        }
    }

    //Window端控制摄像机旋转
    void DesktopInput()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if (mx != 0 || my != 0)
        {
            if (Input.GetMouseButton(0))
            {
                cameraTarget.Rotate(new Vector3(-my * 10, mx * 10, 0), Space.Self);
            }
        }
    }
}

