using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanManager : MonoBehaviour
{
    GameObject globalRecords_GO;                    // Reference to global records
    string[] objects;                               // List of tags which can be stacked

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
        objects = globalRecords_GO.GetComponent<Records>().GetAllObjectTags();
    }

    // Update is called once per frame
 

    private void OnTriggerStay(Collider other)
    {
        if (Array.IndexOf(objects, other.gameObject.tag) != -1)
        {
            if (!other.GetComponent<ObjectManager>().isGrabbed)
            {
                RecordData(other.gameObject);
                Destroy(other.gameObject);
            }
        }
    }
    
    private void RecordData(GameObject gObject)
    {
        globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().AddData("Ingredients", gObject.GetComponent<IngredientProperties>().GetPrefabName() + ":" + gObject.GetInstanceID().ToString(), 2);
        if (gObject.GetComponent<IngredientProperties>().GetPrefabName() == "Burger Bread Down")
        {
            for (int i = 2; i < gObject.GetComponent<ObjectManager>().numStackedIngredients + 2; i++)
            {
                string prefabName = gObject.transform.GetChild(i).GetChild(0).GetComponent<IngredientProperties>().GetPrefabName();
                globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().AddData("Ingredients", prefabName + ":" + gObject.transform.GetChild(i).GetChild(0).GetInstanceID().ToString(), 2);
            }
        }
        if (gObject.GetComponent<IngredientProperties>().GetPrefabName() == "Dough Ketchup")
        {
            for (int i = 3; i < gObject.GetComponent<ObjectManager>().numStackedIngredients + 3; i++)
            {
                string prefabName = gObject.transform.GetChild(i).GetChild(0).GetComponent<IngredientProperties>().GetPrefabName();
                globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().AddData("Ingredients", prefabName + ":" + gObject.transform.GetChild(i).GetChild(0).GetInstanceID().ToString(), 2);
            }
        }
    }
}
