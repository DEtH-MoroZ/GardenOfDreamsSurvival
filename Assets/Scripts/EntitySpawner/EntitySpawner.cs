using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdvancedGridGizmoDrawer))]
public class EntitySpawner : MonoBehaviourExt //current version works in waves
{
    private FSM _WaveSpawnerFSM;

    private GameObjectGrid _Grid;//saving reference to grid, to clean saved in model. 

    private float sceneSideLength = 200f;
    private float gridSideStep = 0.25f;

    [Header("Wave Settings")]
    public int MobCountInitial = 4;
    private int CurrentWave = 1;
    private int MobCountCurrent = 0;    
    private int MobCountWaweCurrent = 0;
    public int MobCountWaweIncrease = 2;

    [Header("Character Prefabs")]
    public Transform playerPrefab;
    public Transform mobPrefab;

    [Header("Timeout between waves")]
    public float Timeout = 10;
    private float currentTimeoutCounter = 0;

    [OnAwake]
    private void TheAwake()
    {
        Model.Set(nameof(sceneSideLength), sceneSideLength);
        Model.Set(nameof(gridSideStep), gridSideStep);
                
        MobCountWaweCurrent = MobCountInitial;

        Model.Set(nameof(CurrentWave), CurrentWave);
        Model.Set(nameof(MobCountCurrent), MobCountCurrent);
        Model.Set(nameof(MobCountWaweCurrent), MobCountWaweCurrent);
        Model.Set(nameof(MobCountWaweIncrease), MobCountWaweIncrease);

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

    [Bind("OnGameObjectGridChanged")]
    private void OnGameObjectGridChanged()
    {
        _Grid = Model.Get<GameObjectGrid>("GameObjectGrid");
    }



    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction(nameof(SpawnMob), SpawnMob);
        Model.EventManager.RemoveAction(nameof(SpawnPlayer), SpawnPlayer);
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
        PlayerTransform.position = Vector2.one * 10f;
        Camera.main.transform.SetParent(PlayerTransform);
        Camera.main.transform.localPosition = Vector3.forward * -15;
        Model.Set(nameof(PlayerTransform), PlayerTransform);
    }
    /*
    [Bind("OnMobCountCurrentChanged")]
    private void OnMobCountCurrentChanged()
    {
        Debug.Log("[FSM_ES] Mob is dead, " + Model.GetInt("MobCountCurrent") + " left.");

        if (Model.GetInt("MobCountCurrent") <= 0)
        {
            _WaveSpawnerFSM.Change("FSM_ES_TimeOut");
        }

    }*/
}
