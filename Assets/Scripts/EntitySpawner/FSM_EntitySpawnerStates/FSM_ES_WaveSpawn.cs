using AxGrid;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_ES_WaveSpawn")]
public class FSM_ES_WaveSpawn : FSMState
{
    private Transform monsterPrefab;

    [Enter]
    private void OnEnter()
    {
        Debug.Log("[FSM_ES] Spawning new wave.");
        Debug.Log("[FSM_ES] Current wave: " + Model.GetInt("CurrentWave"));
        Debug.Log("[FSM_ES] Mobs to spawn: " + Model.GetInt("MobCountWaveCurrent"));        
    }

    [One(0.1f)]
    private void SpawnWave()
    {
        for (int a = 0; a < Model.GetInt("MobCountWaveCurrent"); a++)
        {
            Settings.Invoke("SpawnMob");
            Model.Inc("MobCountCurrent", 1);
        }       
        Parent.Change("FSM_ES_WaitForWaveClear");
    }

    [Exit]
    private void OnExit ()
    {
        Debug.Log("[FSM_ES] Wave spawned.");
    }
}
