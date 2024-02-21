using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 模型层
/// </summary>
public class Model : MonoBehaviour
{
	public const int NORMAL_ROWS = 20;	//正常行数
	public const int MAX_ROWS = 23;		//行数，多3行判断是否在图外
	public const int MAX_COLUMNS = 10;  //列数

	public GameObject block;
	public Transform BlockHolder;

    private Transform[,] map = new Transform[MAX_COLUMNS, MAX_ROWS];

	private int score = 0;			//当前分数
	private int highScore = 0;		//最高分数
	private int numberGame = 0;		//游戏次数
	public int Score {  get { return score; } }
	public int HighScore {  get { return highScore; } }
	public int NumberGame { get { return numberGame; } }

	[HideInInspector]
	public bool isDataUpdate = false;	//是否更新数据
	private bool isGameOver = false;    //游戏是否结束

    /// <summary>
	/// 判断下落位置是否可用
	/// </summary>
	/// <param name="t">当前图案</param>
	/// <returns></returns>
    public bool IsValidMapPosition(Transform t)
	{
		foreach(Transform child in t)
		{
			if(!child.CompareTag("Block")) continue;

			Vector2 pos = child.position.Round();

			if(IsInsideMap(pos) == false) return false;		//超出边界

			if (map[(int)pos.x, (int)pos.y] != null) { return false; }	//是否占用
		}

		return true;
	}

	public bool IsGameOver()
	{
		for(int i = NORMAL_ROWS; i < MAX_ROWS; i++)
		{
			for(int j = 0; j < MAX_COLUMNS; j++)
			{
				if (map[j, i] != null)
				{
					numberGame++;
					isGameOver = true;
                    return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// 是否在边界
	/// </summary>
	private bool IsInsideMap(Vector2 pos)
	{
		return pos.x >= 0 && pos.x < MAX_COLUMNS && pos.y >= 0;
	}

	//摆放图形，设置数组
	public bool PlaceShape(Transform t)
	{
		foreach(Transform child in t)
		{
			if (!child.CompareTag("Block")) continue;

			Vector2 pos = child.position.Round();
			map[(int)pos.x, (int)pos.y] = child;
        }
		return CheckMap();
    }
	/// <summary>
	/// 检查地图是否需要消除行
	/// </summary>
	private bool CheckMap()
	{
		int count = 0;
		for(int i = 0; i < MAX_ROWS; i++)
		{
			bool isFull = CheckIsRowFull(i);
			if (isFull)
			{
				count++;
				DeleteRow(i);
				MoveDownRowsAbove(i + 1);
				i--;
            }
        }
		if (count > 0)
		{
			score += (count * 100);
			if(score > highScore)
			{
				highScore = score;
			}
			isDataUpdate = true;
			return true;
		}
		else return false;
	}
	//检查该行
	private bool CheckIsRowFull(int row)
	{
        Debug.Log("检测该行");
        for (int i = 0; i < MAX_COLUMNS; i++)
		{
			if (map[i, row] == null) return false;
		}
		return true;
	}
	//删除该行
	private void DeleteRow(int row)
	{
		Debug.Log("删除改行");
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
			Destroy(map[i, row].gameObject);
            map[i, row] = null;
        }
    }
	//移动
	private void MoveDownRowsAbove(int row)
	{
		for(int i =  row; i < MAX_ROWS; i++)
		{
			MoveDownRow(i);
		}
	}
	private void MoveDownRow(int row)
	{
		for(int i = 0; i < MAX_COLUMNS; i++)
		{
			if(map[i, row] == null) continue;

			map[i, row - 1] = map[i, row];
			map[i, row] = null;
			map[i, row - 1].position += new Vector3(0, -1, 0);
		}
	}
	/// <summary>
	/// 重新生成
	/// </summary>
	/// <param name="isGameOver">当前游戏是否结束（false为广告重置）</param>
	public void Restart(bool isGameOver = true)
	{
		Debug.Log("重新生成Map");
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            for (int j = 0; j < MAX_ROWS; j++)
            {
                if (map[i, j] != null)
                {
                    Destroy(map[i, j].gameObject);
                    map[i, j] = null;
                }
            }
        }

        if (isGameOver)
		{
            score = 0;
        }
		else
		{
			numberGame--;	//游戏次数减一
			this.isGameOver = false;
		}
	}
    //序列化数据
    public ModelData SerializeData()
	{
		ModelData data = new ModelData();
		TransformData transData;
		if(!isGameOver)		//游戏未结束，存储地图
		{
            data.map = new List<TransformData>(MAX_COLUMNS * MAX_ROWS / 2);		//默认一半地图大小
            for (int i = 0; i < MAX_COLUMNS; i++)
            {
                for (int j = 0; j < MAX_ROWS; j++)
                {
					if (map[i, j] == null) continue;
					transData = new TransformData(map[i, j]);
					transData.i = i;
					transData.j = j;
                    data.map.Add(transData);
                }
            }
			data.isSaveMap = true;
        }
        data.score = score;
        data.highScore = highScore;
        data.numberGame = numberGame;
        return data;
	}

    //反序列化数据
    public void DeserializeData(ModelData data)
    {
		Transform trans;
		if(data.isSaveMap)
		{
			foreach(TransformData transData in data.map)
			{
                trans = Instantiate(block).transform;
                trans.SetParent(BlockHolder);
				map[transData.i, transData.j] = trans;
                trans.GetComponent<SpriteRenderer>().color = transData.color;
                trans.position = new Vector3(transData.x, transData.y, transData.z);
            }
		}

        // 其他数据映射
        score = data.score;
        highScore = data.highScore;
        numberGame = data.numberGame;
    }

    public void ClearData()
	{
		//score = 0;
		highScore = 0;
		numberGame = 0;
		//isGameOver = true;
        
		//Restart();
    }

}
