using Cysharp;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterDamageIndication : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderer;
        
    [SerializeField] private Color damagedColor = Color.red;

    [SerializeField] private float totalDuration = 0.6f;

    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    public void OnHealthChanged()
    {
        // Cancel existing flash and start new one
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();

        FlashColorOnce(cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid FlashColorOnce(CancellationToken token)
    {
        float halfDuration = totalDuration / 2f;
                
        // Flash to target color (first half)
        await ChangeColorOverTime(Color.white, damagedColor, halfDuration, token);

        // Flash back to original color (second half)
        await ChangeColorOverTime(damagedColor, Color.white, halfDuration, token);
        
    }

    private async UniTask ChangeColorOverTime(Color from, Color to, float duration, CancellationToken token)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration && !token.IsCancellationRequested)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            for (int a = 0; a < spriteRenderer.Length; a++)
            {
                spriteRenderer[a].color = Color.Lerp(from, to, t);
            }
            await UniTask.Yield(token);
        }

        // Set final color if not cancelled
        if (!token.IsCancellationRequested)
        {
            for (int a = 0; a < spriteRenderer.Length; a++)
            spriteRenderer[a].color = to;
        }
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }
}

