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
    }

    [Enter]
    private void SpawnWave()
    {
        for (int a = 0; a < Model.GetInt("MobCountCurrent"); a++)
        {
            Settings.Invoke("SpawnMob");
        }
        Debug.Log("[FSM_ES] Wave spawned.");
    }
}
