using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTrayManager : MonoBehaviour
{
    int numObjects = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  
    private void OnTriggerEnter(Collider other)
    {
        numObjects++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coffee_Cup") || other.CompareTag("Ingredient_Base"))
        {
            List<string> preparedFood = CreateIngredientsList(other.gameObject);
            if (!other.GetComponent<ObjectManager>().isGrabbed && transform.parent.GetComponent<CustomerManager>().CheckIndredients(preparedFood))
            {
                transform.parent.transform.GetComponentInParent<ServingStationManager>().RemoveCustomer(transform.parent.gameObject);
                Destroy(transform.parent.gameObject);
                Destroy(other.gameObject);
            }
        }
        else
        {
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        numObjects--;
        if (numObjects == 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    List<string> CreateIngredientsList(GameObject foodItem)
    {
        List<string> preparedFood = new List<string>() { foodItem.GetComponent<IngredientProperties>().GetPrefabName() };
        if (preparedFood[0] == "CoffeeCup")
            return preparedFood;

        for (int i = 2; i < foodItem.transform.childCount; i++)
        {
            preparedFood.Add(foodItem.transform.GetChild(i).GetComponent<IngredientProperties>().GetPrefabName());
        }
        return preparedFood;
    }
}
