using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制层
/// </summary>
public class Controller : MonoBehaviour
{
    [HideInInspector]
	public Model model;
    [HideInInspector]
	public View view;
    [HideInInspector]
    public CameraManager cameraManager;
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public AudioManager audioManager;
    [HideInInspector]
    public ADManager adManager;
    [HideInInspector]
    public DataManager dataManager;

    private FSMSystem fsm;

    private void Awake()
    {
        //获取Model和View
        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();
        cameraManager = GetComponent<CameraManager>();
        gameManager = GetComponent<GameManager>();
        audioManager = GetComponent<AudioManager>();
        adManager = GetComponent<ADManager>();
        dataManager = GetComponent<DataManager>();
    }

    private void Start()
    {
        MakeFSM();
    }

    void MakeFSM()
    {
        fsm = new FSMSystem();
        FSMState[] states = GetComponentsInChildren<FSMState>();
        foreach (FSMState state in states)
        {
            fsm.AddState(state, this);    //将所有状态添加进状态机，并设置控制层
        }

        //设置默认状态为菜单状态
        MenuState s = GetComponentInChildren<MenuState>();      //获取菜单状态
        fsm.SetCurrentState(s);
    }
}
