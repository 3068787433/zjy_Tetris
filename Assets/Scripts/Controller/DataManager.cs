using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 数据管理
/// </summary>
public class DataManager : MonoBehaviour
{
    Controller ctrl;
    private string modelPath;
    private string shapePath;

    private void Awake()
    {
        modelPath = Application.persistentDataPath + "/ModelData.json";
        shapePath = Application.persistentDataPath + "/ShapeData.json";
        ctrl = GetComponent<Controller>();
    }

    private void Start()
    {
        ReadData();
    }

    //游戏关闭时调用
    private void OnApplicationQuit()
    {
        SaveData();
    }

    //游戏失去焦点时保存
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }
    public void SaveData()
    {
        Debug.Log("存档");
        string modeData = JsonUtility.ToJson(ctrl.model.SerializeData());
        string shapeData = JsonUtility.ToJson(ctrl.gameManager.SerializeData());
        File.WriteAllText(modelPath, modeData);
        File.WriteAllText(shapePath, shapeData);
    }

    public void ReadData()
    {
        Debug.Log("读取存档");
        if (File.Exists(modelPath))
        {
            string modeData = File.ReadAllText(modelPath);
            if (!string.IsNullOrEmpty(modeData))
            {
                ModelData data = JsonUtility.FromJson<ModelData>(modeData);
                ctrl.model.DeserializeData(data);
            }
        }

        if (File.Exists(shapePath))
        {
            string shapeData = File.ReadAllText(shapePath);
            if (!string.IsNullOrEmpty(shapeData))
            {
                ShapeData data = JsonUtility.FromJson<ShapeData>(shapeData);
                ctrl.gameManager.DeserializeData(data);
            }
        }
    }

    public void ReadModelData()
    {
        if (File.Exists(modelPath))
        {
            string modeData = File.ReadAllText(modelPath);
            if (!string.IsNullOrEmpty(modeData))
            {
                ModelData data = JsonUtility.FromJson<ModelData>(modeData);
                ctrl.model.DeserializeData(data);
            }
        }
    }

    public void ReadShapeData()
    {
        if (File.Exists(shapePath))
        {
            string shapeData = File.ReadAllText(shapePath);
            if (!string.IsNullOrEmpty(shapeData))
            {
                ShapeData data = JsonUtility.FromJson<ShapeData>(shapeData);
                ctrl.gameManager.DeserializeData(data);
            }
        }
    }
}
