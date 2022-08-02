using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrillManager : MonoBehaviour
{
    [SerializeField] float cookingSpeed;

    // Dictionary<int, float> cookingProgress = new Dictionary<int, float>();
    // Dictionary<int, GameObject> cutlets = new Dictionary<int, GameObject>();
    // Dictionary<int, Cutlets> cookingCutlets = new Dictionary<int, Cutlets>();
    List<string> cutletNames = new List<string>();
    int prevQuotient;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ingredient")
        {
            if (other.GetComponent<IngredientProperties>().GetPrefabName() == "Cutlet B" && !other.gameObject.GetComponent<ObjectManager>().isGrabbed)
            {
                if (cutletNames.Count == 0)
                {
                    cutletNames.Add(other.name);
                    other.GetComponent<CutletManager>().SetCookingState(true);
                }
                else
                {
                    if (!cutletNames.Contains(other.GetComponent<IngredientProperties>().GetPrefabName()))
                    {
                        cutletNames.Add(other.name);
                        other.GetComponent<CutletManager>().SetCookingState(true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ingredient")
        {
            if (other.GetComponent<IngredientProperties>().GetPrefabName() == "Cutlet B" && other.gameObject.GetComponent<ObjectManager>().isGrabbed)
            {
                if (cutletNames.Contains(other.name))
                {
                    cutletNames.Remove(other.name);
                    other.GetComponent<CutletManager>().SetCookingState(false);
                }
            }
        }
    }
}
