using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

public class UIDisableWhenLessThenZero : MonoBehaviourExt
{
    [SerializeField]
    private string _valueToTrack = "";

    [OnStart]
    private void TheStart()
    {
        Model.EventManager.AddAction<float>($"On{_valueToTrack}Changed", OnValueToTrackChanged);
        
    }

    private void OnValueToTrackChanged(float value)
    {
        if (value <= 0)
        {
            gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(true);
    }

    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction<float>($"On{_valueToTrack}Changed", OnValueToTrackChanged);
    }
}
