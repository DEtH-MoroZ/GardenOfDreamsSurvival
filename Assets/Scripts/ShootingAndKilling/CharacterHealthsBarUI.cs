using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthsBarUI : MonoBehaviour
{    
    public Transform HealthsBar;
    public Transform HealthsBarFull;

    private float healthsPercentage;

    public void OnHealthsChanged(int currentHealths, int maxHealths) {
        healthsPercentage = (float)currentHealths / (float)maxHealths;

        HealthsBarFull.localPosition = new Vector3(healthsPercentage - 1 * 0.5f, HealthsBarFull.localPosition.y, HealthsBarFull.localPosition.z);
        HealthsBarFull.localScale = new Vector3(healthsPercentage, HealthsBarFull.localScale.y, HealthsBarFull.localScale.z);
    }
}
