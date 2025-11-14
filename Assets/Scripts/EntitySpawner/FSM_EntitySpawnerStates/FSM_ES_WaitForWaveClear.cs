using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
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

        Settings.Model.EventManager.AddAction("OnMobCountCurrentChanged", OnMobCountCurrentChanged);
    }

    private void OnMobDeath()
    {
        
    }
        
    private void OnMobCountCurrentChanged()
    {
        Debug.Log("[FSM_ES] Mob count " + Model.GetInt("MobCountCurrent") + ".");        

        if (Model.GetInt("MobCountCurrent") <= 0)
        {
            Parent.Change("FSM_ES_TimeOut");
        }
    }

    [Exit]
    private void OnExit()
    {
        Model.Inc("MobCountWaveCurrent", Model.GetInt("MobCountWaveIncrease"));
        Model.Inc("CurrentWave", 1);
    }

    private void OnWaveClear()
    {
        Debug.Log("[FSM_ES Wave cleared.");
    }
}
