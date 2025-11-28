using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorScriptableObject : ScriptableObject
{
    public virtual Vector2 MovementOutput(Vector2 position, Vector2 target)
    {
        return Vector2.zero;
    }
    public virtual Vector2 MovementOutput()
    {
        return Vector2.zero;
    }
    public virtual void ProcessEvents() { }

    public virtual void Start() { }
}
