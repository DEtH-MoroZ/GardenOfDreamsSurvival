using AxGrid.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[State("FSM_CB_Idle")]
public class FSM_CB_Idle : FSMState
{
    BehaviorScriptableObject _idleBehavior;

    public FSM_CB_Idle (BehaviorScriptableObject idleBehavior)
    {
        _idleBehavior = idleBehavior;
    }
}
