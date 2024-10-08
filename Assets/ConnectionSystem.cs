using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading;

public class ConnectionSystem : MonoBehaviour
{
    [SerializeField] DateTime networkTime;

    bool isConnect = false;
    const float TimeToConnect = 6.0f;
    float Timer = TimeToConnect;
    Action action = Action.Connection;

    [SerializeField] private GameObject panelFail;
    [SerializeField] private GameObject panelConnection;
    [SerializeField] private GameObject panelSuccess;

    TimeSystem _timeSystem;

    enum Action
    {
        Connection,
        Fail,
        Success
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeSystem = GameObject.FindFirstObjectByType<TimeSystem>();
    }

   

    // Update is called once per frame
    void Update()
    {
        switch (action)
        {
            case Action.Connection:
                Timer -= Time.deltaTime;
                if(Timer <= 0.0f || !_timeSystem.InConnection)
                {
                    
                    if(isConnect)
                    {
                        action = Action.Success;
                        panelSuccess.SetActive(true);
                        _timeSystem.InConnection = false;
                    }
                    else
                    {
                        action = Action.Fail;
                        panelFail.SetActive(true);
                        _timeSystem.InConnection = false;
                    }
                    panelConnection.SetActive(false);
                }
            break;

            case Action.Success:
                SceneManager.LoadScene(1);
                break;
        }
        
    }

    public void Retry()
    {
        panelFail.SetActive(false);
        panelConnection.SetActive(true);
        action = Action.Connection;
        Timer = TimeToConnect;
        _timeSystem.InConnection = true;
    }

    public void OnConnected(DateTime networkTime)
    {
        isConnect = true;
        this.networkTime = networkTime;
    }
}
