using AxGrid.Base;
using AxGrid.Model;
using TMPro;
using UnityEngine;

public class UITextBind : MonoBehaviourExt
{
    [SerializeField]
    private string _valueToTrack = "";

    public string prefixText = "";
    public string postfixText = "";

    private TextMeshProUGUI txtContainer;
    
    [OnStart]
    private void TheStart()
    {
        Model.EventManager.AddAction($"On{_valueToTrack}Changed", OnValueToTrackChanged);
        txtContainer = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void OnValueToTrackChanged()
    {
        txtContainer.text = prefixText + " " + Model.Get(_valueToTrack) + " " + postfixText;
    }

    [OnDestroy]
    private void TheDestroy()
    {
        Model.EventManager.RemoveAction($"On{_valueToTrack}Changed", OnValueToTrackChanged);
    }

        /*
        [Bind("On{_valueToTrack}Changed")]
        private void OnValueToTrackChanged(float value)
        {
            finalText = prefixText + " " + value.ToString("0.0") + " " + postfixText;
            txtContainer.text = finalText;
        }
        */

    }
