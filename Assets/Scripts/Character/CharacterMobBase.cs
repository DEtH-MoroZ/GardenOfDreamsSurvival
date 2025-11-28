using AxGrid.Base;
using AxGrid.FSM;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMobBase : CharacterBase
{
    private FSM _fsm;

    [Header("Behavior")]
    public BehaviorScriptableObject idleBehavior;
    public BehaviorScriptableObject chargeBehavior;
    public BehaviorScriptableObject atackBehavior;


    [OnStart(2)]
    public void StartFSM()
    {
        _fsm = new FSM();
        _fsm.Add(new FSM_CB_Initial());
        _fsm.Add(new FSM_CB_Idle(idleBehavior));
        _fsm.Add(new FSM_CB_Move());
        _fsm.Add(new FSM_CB_Atack());

        _fsm.Start("FSM_CB_Initial");
    }

    public override void Die()
    {
        Debug.Log("[Character Mob Base] Mob died.");
        Model.Dec("MobCountCurrent", 1);
        gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        gameObject.SetActive(false);
    }
}
