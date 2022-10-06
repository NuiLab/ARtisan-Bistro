using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    string[] objects;                               // List of tags which can be stacked
    [SerializeField] Material redMaterial;          // Red Material
    [SerializeField] Material greenMaterial;        // Green Marerial
    string cutletCookingStatus = "n/a";

    // Start is called before the first frame update
    void Start()
    {
        objects = transform.parent.GetComponent<ObjectManager>().globalRecords_GO.GetComponent<Records>().GetObjectTags();
    }

    private void Update()
    {
        if (transform.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup") && !transform.GetComponentInParent<IngredientProperties>().GetCookingStatus().Equals("Uncooked"))
        {
            transform.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if object in collision mesh is appropriate object to stack;
        // Change parent tag; change material to green;
        // Set regionCollision of object to current collision mesh
        if (Array.IndexOf(objects, other.gameObject.tag) != -1 && CheckBaseIngredient(other.gameObject))
        {
            transform.GetComponent<Renderer>().material = greenMaterial;
            other.GetComponent<ObjectManager>().regionCollision = transform.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // Check if object in collision mesh is appropriate object to stack;
        // Change parent tag; change material to red;
        // Set regionCollision of object to global records
        if (Array.IndexOf(objects, other.gameObject.tag) != -1 && CheckBaseIngredient(other.gameObject))
        {
            transform.GetComponent<Renderer>().material = redMaterial;
            other.GetComponent<ObjectManager>().regionCollision = other.GetComponent<ObjectManager>().globalRecords_GO;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if object in collision mesh is appropriate object to stack and it is not grabbed;
        // Destroy object;
        // Find corresponding empty object in the prefabList and instantiate it;
        // Set parent and reset location, rotation and scale, and other properties;
        // Change mode of parent and object;
        if (Array.IndexOf(objects, other.gameObject.tag) != -1 && !other.gameObject.GetComponent<ObjectManager>().isGrabbed && CheckBaseIngredient(other.gameObject))
        {
            GameObject tempPrefab = new GameObject("Dummy");
            other.GetComponent<IngredientProperties>().SetPrefabName();
            
            GameObject[] prefabList = transform.GetComponentInParent<ObjectManager>().globalRecords_GO.GetComponent<Records>().GetPrefabList();
            for (int i = 0; i < prefabList.Length; i++)
            {
                if (prefabList[i].name.Contains(other.GetComponent<IngredientProperties>().GetPrefabName()))
                {
                    if (other.GetComponent<IngredientProperties>().GetPrefabName().Equals("Cutlet B"))
                        cutletCookingStatus = other.GetComponent<IngredientProperties>().GetCookingStatus();
                    Destroy(tempPrefab);
                    Destroy(other.gameObject);
                    tempPrefab = prefabList[i];
                    break;
                }
            }
            GameObject tempPrefabObject = Instantiate(tempPrefab);
            
            if (transform.parent.GetComponent<ObjectManager>().mode == 1)
                tempPrefabObject.transform.SetParent(transform.parent.parent.parent);
            else
                tempPrefabObject.transform.SetParent(transform.parent);
            tempPrefabObject.transform.parent.GetComponent<ObjectManager>().numStackedIngredients++;
            tempPrefabObject.transform.localPosition = new Vector3(tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetLocation().x,
                tempPrefabObject.transform.parent.GetComponent<ObjectManager>().topObjectLoc + tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetLocation().y,
                tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetLocation().z);
            tempPrefabObject.transform.localRotation =  tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetRotation();
            tempPrefabObject.transform.localScale = tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetScale();
            tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().SetPrefabName();
            if (tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetPrefabName().Equals("Cutlet B Empty") && cutletCookingStatus != "n/a")
                tempPrefabObject.transform.GetChild(0).GetComponent<CutletEmptyManager>().SetCookingStatus(cutletCookingStatus);
            tempPrefabObject.name = tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetPrefabName() + " " + tempPrefabObject.transform.parent.GetComponent<ObjectManager>().numStackedIngredients;
            tempPrefabObject.transform.GetChild(0).GetComponent<ObjectManager>().mode = 1;
            if (other.tag == "Ingredient_Cap")
            {
                if (transform.parent.CompareTag("Ingredient_Base"))
                    transform.parent.GetComponent<ObjectManager>().mode = 4;
                else
                    transform.parent.parent.parent.GetComponent<ObjectManager>().mode = 4;

            }
            if (transform.parent.CompareTag("Ingredient_Base"))
            {
                if (transform.parent.GetComponent<ObjectManager>().mode == 1)
                    transform.parent.GetComponent<ObjectManager>().mode = 3;
                else
                    transform.parent.GetComponent<ObjectManager>().mode = 2;
            }
            tempPrefabObject.transform.parent.GetComponent<ObjectManager>().topObjectLoc += tempPrefabObject.transform.GetChild(0).GetComponent<IngredientProperties>().GetLocation().y;
            transform.gameObject.SetActive(false);
        }
    }

    bool CheckBaseIngredient(GameObject baseObject)
    {
        List<string> pizzaIngredients = new List<string>() { "Basil Leaf Layer", "Mushroom Slice Layer", "Olive Black Slice Layer", "Pepper Green Layer", "Pepperoni Layer" };
        List<string> burgerIngredients = new List<string>() { "Bacon Slice", "Cheese Slice A", "Cutlet B", "Onion Slice", "Salad Slice", "Tomato Slice", "Burger Bread Up" };
        if (pizzaIngredients.Contains(baseObject.GetComponent<IngredientProperties>().GetPrefabName()))
        {
            if (transform.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup") || transform.parent.parent.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup"))
            {
                if (transform.parent.tag.Equals("Ingredient_Base") || transform.parent.parent.parent.tag.Equals("Ingredient_Base"))
                    return true;
                else
                    return false;
            }
        }

        if (burgerIngredients.Contains(baseObject.GetComponent<IngredientProperties>().GetPrefabName()))
        {
            if (transform.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Burger Bread Down") || transform.parent.parent.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Burger Bread Down"))
            {
                if (transform.parent.tag.Equals("Ingredient_Base") || transform.parent.parent.parent.tag.Equals("Ingredient_Base"))
                    return true;
                else
                    return false;
            }
        }
        /*
        if (transform.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup") || transform.parent.parent.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Dough Ketchup"))
        {
            if (transform.parent.tag.Equals("Ingredient_Base") || transform.parent.parent.parent.tag.Equals("Ingredient_Base"))
            {
                switch (baseObject.GetComponent<IngredientProperties>().GetPrefabName())
                {
                    case "Basil Leaf Layer":
                        return true;
                    case "Mushroom Slice Layer":
                        return true;
                    case "Olive Black Slice Layer":
                        return true;
                    case "Pepper Green Layer":
                        return true;
                    case "Pepperoni Layer":
                        return true;
                    default:
                        return false;
                }
            }
        }

        if (transform.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Burger Bread Down") || transform.parent.parent.GetComponentInParent<IngredientProperties>().GetPrefabName().Equals("Burger Bread Down"))
        {
            if (transform.parent.tag.Equals("Ingredient_Base") || transform.parent.parent.parent.tag.Equals("Ingredient_Base"))
            {
                switch (baseObject.GetComponent<IngredientProperties>().GetPrefabName())
                {
                    case "Bacon Slice":
                        return true;
                    case "Cheese Slice A":
                        return true;
                    case "Cutlet B":
                        return true;
                    case "Onion Slice":
                        return true;
                    case "Salad Slice":
                        return true;
                    case "Tomato Slice":
                        return true;
                    case "Burger Bread Up":
                        return true;
                    default:
                        return false;
                }
            }
        }*/
        return false;
    }
}
