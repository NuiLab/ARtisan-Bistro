using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutletEmptyManager : MonoBehaviour
{
    [SerializeField] GameObject cookingStatusObject;

    string cookingStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
 

    public void SetCookingStatus(string cookingS)
    {
        cookingStatus = cookingS;
        
        int x = 0;
        switch (cookingS)
        {
            case "Uncooked":
                x = 0;
                break;
            case "Partially Cooked":
                x = 1;
                break;
            case "Cooked":
                x = 2;
                break;
            case "Over Cooked":
                x = 3;
                break;
            case "Burnt":
                x = 4;
                break;
        }
        transform.GetComponent<IngredientProperties>().SetCookingStatus(x, "Burger");
        // transform.GetComponent<ObjectManager>().globalRecords_GO.GetComponent<Records>().SetCookingStatusEffects(x, cookingStatusObject, "Burger");
    }

    public string GetCookingStatus()
    {
        return cookingStatus;
    }
}
