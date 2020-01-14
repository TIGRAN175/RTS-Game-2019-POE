using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDamageHandler : MonoBehaviour {

    [Header("Unity Generics")]
    public Image HealthBar;

    public void UpdateHealth(float newHealth, float maxHealth)
    {
        HealthBar.fillAmount = newHealth / maxHealth;
        //HealthBar.fillAmount = 0.5f;
    }
	// Use this for initialization
	void Start () {
        HealthBar.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
