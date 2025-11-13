using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviourExt //current version works in waves
{
    private FSM _WaveSpawnerFSM;

    private GameObjectGrid _Grid;//saving reference to grid, to clean  saved in model. 

    private float sceneSideLength = 200f;
    private float gridSideStep = 0.25f;
    
    public int MobCountCurrent = 0;
    public int MobCountInitial = 4;
    public int MobCountIncrease = 2;

    public Transform playerPrefab;
    public Transform mobPrefab;    

    public float Timeout = 10; //time between waves
    private float currentTimeoutCounter = 0;

    [OnAwake]
    private void TheAwake()
    {
        Model.Set(nameof(sceneSideLength), sceneSideLength);
        Model.Set(nameof(gridSideStep), gridSideStep);

        MobCountCurrent = MobCountInitial;

        Model.Set(nameof(MobCountCurrent), MobCountCurrent);
        Model.Set(nameof(MobCountIncrease), MobCountIncrease);

        Model.Set(nameof(Timeout), Timeout);
        Model.Set(nameof(currentTimeoutCounter), currentTimeoutCounter);

        _WaveSpawnerFSM = new FSM();
                
        _WaveSpawnerFSM.Add(new FSM_ES_Initial());
        _WaveSpawnerFSM.Add(new FSM_ES_WaveSpawn());
        _WaveSpawnerFSM.Add(new FSM_ES_WaitForWaveClear());
        _WaveSpawnerFSM.Add(new FSM_ES_TimeOut());
    }

    [OnStart]
    private void TheStart()
    {
        Model.EventManager.AddAction(nameof(SpawnMob), SpawnMob);
        Model.EventManager.AddAction(nameof(SpawnPlayer), SpawnPlayer);
        _WaveSpawnerFSM.Start("FSM_ES_Initial");

        _Grid = Model.Get<GameObjectGrid>("GameObjectGrid");       
    }

    [OnUpdate]
    private void TheUpdate() //try fixed update?
    {
        _WaveSpawnerFSM.Update(Time.deltaTime);
    }

    private void SpawnMob()
    {
        Transform MobTransform = Instantiate(mobPrefab);

        MobTransform.GetComponent<MobInputProvider>().target = Model.Get<Transform>("PlayerTransform");
    }
    
    private void SpawnPlayer() 
    {
        Transform PlayerTransform = Instantiate(playerPrefab);
        Camera.main.transform.SetParent(PlayerTransform);
        Camera.main.transform.position = Vector3.forward * -15;
        Model.Set(nameof(PlayerTransform), PlayerTransform);
    }

    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction(nameof(SpawnMob), SpawnMob);
        Model.EventManager.RemoveAction(nameof(SpawnPlayer), SpawnPlayer);
    }
}
