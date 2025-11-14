using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_ES_TimeOut")]
public class FSM_ES_TimeOut : FSMState
{
    float currentTimeout = 0f;

    [Enter]
    private void OnEnter()
    {
        currentTimeout = Model.GetFloat("Timeout");
        Debug.Log("[FSM_ES] Timeout is " + currentTimeout + " seconds.");
    }

    [Loop(0.2f)]
    private void TimeOut()
    {
        Model.Set("currentTimeoutCounter", (currentTimeout -= 0.2f) );

        if(currentTimeout <= 0f)
        {
            Debug.Log("[FSM_ES] Timeout finished.");
            Parent.Change("FSM_ES_WaveSpawn");
        }
    }
}
