using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupManager : MonoBehaviour
{
    [SerializeField] GameObject coffeeCupCap;

    float coffeeCupFillRate = 4.2f;
    public float coffeeLevel;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = transform.GetComponent<Renderer>();
        coffeeLevel = rend.material.GetFloat("FillLevel");
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coffee_Pour"))
        {
            if (other.transform.parent.GetComponent<CoffeePotManager>().GetIsPouring())
            {
                if (coffeeLevel < 0.03f)
                {
                    coffeeLevel += coffeeCupFillRate * 0.001f * Time.deltaTime;
                    rend.material.SetFloat("FillLevel", coffeeLevel);
                }
                else if (!coffeeCupCap.activeSelf)
                {
                    coffeeCupCap.SetActive(true);
                }
            }
        }
    }
}
