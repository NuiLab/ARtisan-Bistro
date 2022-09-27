using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientProperties : MonoBehaviour
{
    [SerializeField] Vector3 location;
    [SerializeField] Quaternion rotation;
    [SerializeField] Vector3 scale;
    [SerializeField] GameObject firePrefab;

    string prefabName = "";          // Variable to store original prefab name
    GameObject cookingStatusEffectsObject;

    public string cookingStatus = "Uncooked";

    GameObject persistentGO;


    // Start is called before the first frame update
    void Start()
    {
        persistentGO = GameObject.FindGameObjectsWithTag("PersistentGO")[0];
        SetPrefabName();
        if (prefabName.Contains(" Empty"))
            prefabName = prefabName.Substring(0, prefabName.Length - 6);
        else
            persistentGO.GetComponent<PersistentGOManager>().AddData("Ingredients", prefabName + ":" + GetInstanceID().ToString(), 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 1)
        {
            if (cookingStatus != "Uncooked" && transform.GetChild(1).gameObject.activeSelf && transform.name.Equals("Dough Ketchup") && transform.GetChild(1).gameObject.name != "Cooking Status")
                transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // SetPrefabName is called to extract prefab name from the object name
    public void SetPrefabName()
    {
        string objectName = transform.name;
        for (int i = 0; i < objectName.Length; i++)
        {
            if (((objectName[i] - '0') <= 9 && (objectName[i] - '0') >= 0) || objectName[i] == '(')     // Prefab names are only made of alphabets. Number or Brackets signify copies
            {
                if (objectName[i - 1] == ' ')
                    prefabName = objectName.Substring(0, i - 1);
                else
                    prefabName = objectName.Substring(0, i);
                
                return;
            }
        }
        prefabName = objectName;
    }

    public void SetLocation(Vector3 loc)
    {
        location = loc;
    }

    public Vector3 GetLocation()
    {
        return location;
    }

    public void SetRotation(Quaternion rot)
    {
        rotation = rot;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public void SetScale(Vector3 scle)
    {
        scale = scle;
    }

    public Vector3 GetScale()
    {
        return scale;
    }

    public string GetPrefabName()
    {
        return prefabName;
    }

    public string GetCookingStatus()
    {
        return cookingStatus;
    }

    public void SetCookingStatus(int x, string foodItem)
    {
        switch (x)
        {
            case 0:
                cookingStatus = "Uncooked";
                break;
            case 1:
                cookingStatus = "Partially Cooked";
                break;
            case 2:
                cookingStatus = "Cooked";
                break;
            case 3:
                cookingStatus = "Over Cooked";
                break;
            case 4:
                cookingStatus = "Burnt";
                break;
        }
        if (cookingStatusEffectsObject != null)
            Destroy(cookingStatusEffectsObject);
        GameObject.FindWithTag("Global Records").GetComponent<Records>().SetCookingStatusEffects(x, transform.Find("Cooking Status").gameObject, foodItem);

        if (x == 4)
        {
            // cookingStatusEffectsObject = Instantiate(transform.GetComponent<ObjectManager>().globalRecords_GO.GetComponent<Records>().GetCookingStatusEffects(x, foodItem));
            cookingStatusEffectsObject = Instantiate(GameObject.FindWithTag("Global Records").GetComponent<Records>().GetFirePrefab());
            cookingStatusEffectsObject.transform.parent = transform;
            cookingStatusEffectsObject.transform.position = transform.position;
        }
    }
}
