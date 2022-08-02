using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public int mode = 0;
    /* Mode"
     * 0: Free ingredient.
     * 1: Attached below.
     * 2: Attached above.
     * 3: Attached both.
     * 4: Attached cap ingredient.
     */

    public bool isGrabbed = false;          // Variable stating if object is grabbed
    public GameObject regionCollision;      // GameObject to keep track of which collision mesh is overlapped
    public GameObject globalRecords_GO;     // Reference to global records
    public bool justSpawned = true;         // Flag to check if object is on spawning plate
    public int numStackedIngredients = 0;   // Number of objecrts stacked on top
    public float topObjectLoc = 0;

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
        regionCollision = globalRecords_GO;
    }

 

    // SetIsGrabbed is called when the object is grabbed
    public void SetIsGrabbed()
    {
        isGrabbed = true;
        if (numStackedIngredients == 0)
        {
            if (CompareTag("Ingredient_Base"))
                transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            if (mode != 4)
            {
                if (transform.GetComponent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup"))
                    transform.GetChild(numStackedIngredients + 2).GetChild(0).GetChild(1).gameObject.SetActive(false);  // Disable drop region
                else
                    transform.GetChild(numStackedIngredients + 1).GetChild(0).GetChild(1).gameObject.SetActive(false);  // Disable drop region
            }
                
        }
    }

    // SetIsNotGrabbed is called when the object is released
    public void SetIsNotGrabbed()
    {
        isGrabbed = false;
        if (mode == 0 && !justSpawned)
        {
            if (CompareTag("Ingredient_Base"))
                transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            if (mode != 4)
            {
                if (transform.GetComponent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup"))
                    transform.GetChild(numStackedIngredients + 2).GetChild(0).GetChild(1).gameObject.SetActive(true);  // Disable drop region
                else
                    transform.GetChild(numStackedIngredients + 1).GetChild(0).GetChild(1).gameObject.SetActive(true);  // Disable drop region
            }
        }
    }
}
