using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeath : MonoBehaviour {

    [SerializeField]
    float fillAmount;
    [SerializeField]
     Image content;
    [SerializeField]
    float heath;
    [SerializeField]
    float maxHeath;

    // Use this for initialization
    void Start()
    {
        heath = maxHeath;
    }

    // Update is called once per frame
    void Update()
    {
        content.fillAmount = map(heath, 0, maxHeath, 0, 1);
        //transform.position = target.position;
    }

    private float map(float CurrentHeath, float inMin, float MaxHeath, float outMin, float outMax)
    {
        return (CurrentHeath - inMin) * (outMax - outMin) / (MaxHeath - inMin) + outMin;
    }
    // Geters
    #region
    public float GetHeath()
    {
        return heath;
    }
    public float GetMaxHeath()
    {
        return maxHeath;
    }
    #endregion
    // Setters
    #region
    public void takeDamage(float _damage)
    {
        heath -= _damage;
    }
    #endregion
}
