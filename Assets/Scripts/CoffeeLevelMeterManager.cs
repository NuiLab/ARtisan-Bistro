using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeLevelMeterManager : MonoBehaviour
{
    // [SerializeField] GameObject coffeeLevel_GO;

    Renderer coffeeLevel_rend;
    float coffeeLevel = 1;
    float prevCoffeeLevel = 0;


    // Start is called before the first frame update
    void Start()
    {
        coffeeLevel_rend = transform.GetChild(0).GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!prevCoffeeLevel.Equals(transform.parent.GetComponent<CoffeeMakerManager>().GetCoffeeLevel() * 1.6f))
        {
            coffeeLevel = transform.parent.GetComponent<CoffeeMakerManager>().GetCoffeeLevel() * 1.6f;
            coffeeLevel_rend.material.SetFloat("FillLevel", coffeeLevel);
            prevCoffeeLevel = coffeeLevel;
        }
    }
}
