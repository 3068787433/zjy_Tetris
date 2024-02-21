using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏控制
/// </summary>
public class GameManager : MonoBehaviour
{
	private bool isPause = true;    //游戏是否暂停
    private Shape currentShape = null;  //当前图案

    public Shape[] shapes;      //图案预制体
    public Color[] colors;
    private int index;      //图案数组索引
    private int indexColor; //颜色数组索引

    private Controller ctrl;

    private Transform blockHolder;      //所有格子的收纳格
    private bool isDelete;

    private void Awake()
    {
        ctrl = GetComponent<Controller>();
        blockHolder = transform.Find("BlockHolder");
    }

    private void Update()
    {
        if (isPause) { return; }    //暂停状态

        if(currentShape == null)
        {
            SpawnShape();
        }
    }

    //开始
    public void StartGame()
    {
        isPause = false;
        if (currentShape != null)
        {
            currentShape.Resume();
        }
    }
    //暂停
    public void PauseGame()
    {
        isPause = true;
        if(currentShape != null)
        {
            currentShape.Pause();
        }
    }
    //清空
    public void ClearShape()
    {
        if(currentShape != null)
        {
            Debug.Log("清空shape");
            Destroy(currentShape.gameObject);
            currentShape = null;
            isDelete = true;
        }
    }

    //广告重置
    public void ResetAll()
    {
        //销毁当前图案
        ClearShape();
        //重置地图数组（Model的Restart，分数不变）
        ctrl.model.Restart(false);
        //继续游戏（隐藏游戏结束界面）
        StartGame();
        ctrl.view.HideGameOverUI();
    }

    /// <summary>
    /// 生成图形
    /// </summary>
    private void SpawnShape()
    {
        index = Random.Range(0, shapes.Length);
        indexColor = Random.Range(0, colors.Length);
        currentShape = GameObject.Instantiate(shapes[index]);
        currentShape.transform.SetParent(blockHolder, false);
        currentShape.Init(colors[indexColor], ctrl);
    }

    //方块落下
    public void FallDown()
    {
        currentShape = null;
        if(ctrl.model.isDataUpdate)
        {
            ctrl.view.UpdateGameUI(ctrl.model.Score, ctrl.model.HighScore);
        }
        foreach(Transform t in blockHolder)
        {
            if(t.childCount == 1)   //删除只有一个旋转点Pivot的，不能小于等于1（会删除读档的方块）
            {
                Destroy(t.gameObject);
            }
        }

        if(ctrl.model.IsGameOver())
        {
            PauseGame();
            isDelete = true;
            ctrl.view.ShowGameOverUI(ctrl.model.Score);
        }
    }

    public ShapeData SerializeData()
    {
        ShapeData data = new();
        data.isDelete = isDelete;
        data.index = index;
        data.indexColor = indexColor;
        if (currentShape != null)
        {
            data.x = currentShape.transform.position.x;
            data.y = currentShape.transform.position.y;
            data.z = currentShape.transform.position.z;
        }
        return data;
    }

    public void DeserializeData(ShapeData data)
    {
        if (data.isDelete)
        {
            currentShape = null;
            return;
        }
        index = data.index;
        indexColor = data.indexColor;
        currentShape = Instantiate(shapes[index]);  //生成
        currentShape.transform.position = new Vector3(data.x, data.y, data.z);  //坐标
        currentShape.transform.SetParent(blockHolder, false);   //父级
        currentShape.Init(colors[indexColor], ctrl);    //初始化
        PauseGame();    //暂停
    }
}
