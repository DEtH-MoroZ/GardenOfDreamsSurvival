
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleBehavior", menuName = "Game/Behavior/Idle Behavior")]
public class IdleScriptableObject : BehaviorScriptableObject //if in range - start move
{
    public float roamRange = 4.0f;
    public float waitTime = 1.0f;

    public float roamSpeed = 0.3f;

    private CancellationTokenSource _cancellationTokenSource;

    private Vector2 direction;

    public override Vector2 MovementOutput(s)
    {
        return direction*roamSpeed;
    }

    public override void Start()
    {
        Roam();
    }

    public void Roam()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        StartMovementLoop().Forget();
    }

    private async UniTaskVoid StartMovementLoop()
    {
        // Pass the cancellation token to the loop
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            await MoveRandomDirection();
            direction = Vector2.zero;
            await UniTask.Delay((int)(waitTime * 1000), cancellationToken: _cancellationTokenSource.Token);
        }
    }

    private async UniTask MoveRandomDirection()
    {
        // Generate a random direction vector and normalize it 
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Move the object for a fixed duration
        float moveDuration = 1f; // Move for 1 second
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: _cancellationTokenSource.Token); 
        }
    }

    void OnDestroy()
    {
        // Cancel the task when the GameObject is destroyed to prevent errors [citation:1]
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}
