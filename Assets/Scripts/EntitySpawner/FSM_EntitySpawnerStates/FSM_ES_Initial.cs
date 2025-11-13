using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_ES_Initial")]
public class FSM_ES_Initial : FSMState
{
    GameObjectGrid theGrid;

    [Enter]
    private void OnEnter() {        
        InitializeES();
        SpawnPlayer();
        Parent.Change("FSM_ES_TimeOut");
    }
        
    private void InitializeES ()
    {
        Debug.Log("[FSM_ES] Initialization...");

        theGrid = new GameObjectGrid(Model.GetFloat("sceneSideLength"), Model.GetFloat("gridSideStep"));

        Model.Set("GameObjectGrid", theGrid);
    }

    private void SpawnPlayer()
    {
        Settings.Invoke("SpawnPlayer");
    }
}
