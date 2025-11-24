using AxGrid.Base;
using AxGrid.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerBase : CharacterBase
{
    public GameObject Crosshair;

    private SmoothFollowCamera2D followCamera;

    [OnDelay(0.2f)]
    public void SetUpCamera ()
    {
        followCamera = Camera.main.GetComponent<SmoothFollowCamera2D>();
        followCamera.target = this.transform;
    }

    
    public override void FindTargetAndAtack()
    {
        base.FindTargetAndAtack();
        
        if (TargetForAtack != null )
        {
            Crosshair.transform.SetParent( TargetForAtack.transform );
            Crosshair.transform.localPosition = Vector3.up;
            Crosshair.SetActive(true);
            followCamera.targetToAim = TargetForAtack.transform;
        }
        else
        {
            followCamera.targetToAim = null;
            Crosshair.SetActive(false);
        }
    }

    public override void Atack()
    {
        base.Atack();
    }
}
