using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_ES_WaitForWaveClear")]
public class FSM_ES_WaitForWaveClear : FSMState
{
    [Enter]
    private void OnEnter()
    {
        Debug.Log("[FSM_ES] Wait For Wawe Clear.");
    }

    private void OnWaveClear()
    {
        Debug.Log("[FSM_ES Wave cleared.");
    }
}
