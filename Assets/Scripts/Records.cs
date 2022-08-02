using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Records : MonoBehaviour
{
    [SerializeField] GameObject[] prefabList;       // List of GameObjects that can be spawned
    [SerializeField] string[] objectTags;
    [SerializeField] string[] allObjectTags;
    [SerializeField] GameObject playArea;
    [SerializeField] GameObject firePrefab;
    [SerializeField] Material[] cookingStatusEffectsPizza;
    [SerializeField] Material[] cookingStatusEffectsBurger;
    [SerializeField] Material[] selectablePizzaIngredientsMaterial;
    [SerializeField] Material[] selectableBurgerIngredientsMaterial;
    [SerializeField] GameObject[] notificationPrefabs;
    [SerializeField] int notificationType;
    [SerializeField] GameObject notificationSetManager;

    GameObject persistentGO;
    /* 
     * Notification Types:
     * 0. Notification on the Object(NoO)
     * 1. Notification in the Viewport(NoV)
     * 2. Notification on the Dock(NoD)
     */

    // Start is called before the first frame update
    void Start()
    {
        persistentGO = GameObject.FindGameObjectsWithTag("PersistentGO")[0];
        persistentGO.GetComponent<PersistentGOManager>().SetCurrGlobalRecordsGO(transform.gameObject);
    }

    // Update is called once per frame
   

    public GameObject[] GetPrefabList()
    {
        return prefabList;
    }

    public string[] GetObjectTags()
    {
        return objectTags;
    }

    public string[] GetAllObjectTags()
    {
        return allObjectTags;
    }

    public GameObject GetPlayArea()
    {
        return playArea;
    }

    public void SetCookingStatusEffects(int x, GameObject cookingStatusObject, string foodItem)
    {
        if (foodItem.Equals("Burger"))
        {
            cookingStatusObject.GetComponent<Renderer>().material = cookingStatusEffectsBurger[x];
        }
        else if (foodItem.Equals("Pizza"))
        {
            cookingStatusObject.GetComponent<Renderer>().material = cookingStatusEffectsPizza[x];
        }
    }

    public GameObject GetFirePrefab()
    {
        return firePrefab;
    }

    public Material GetPizzaIngredientMaterial(string x)
    {
        return selectablePizzaIngredientsMaterial[IngredientMaterialIndex(x)];
    }

    public Material GetBurgerIngredientMaterial(string x)
    {
        return selectableBurgerIngredientsMaterial[IngredientMaterialIndex(x)];
    }

    int IngredientMaterialIndex(string ingredient)
    {
        switch (ingredient)
        {
            // Pizza
            case "Dough Ketchup":
                return 0;
            case "Pepperoni Layer":
                return 1;
            case "Pepper Green Layer":
                return 2;
            case "Olive Black Slice Layer":
                return 3;
            case "Mushroom Slice Layer":
                return 4;
            case "Basil Leaf Layer":
                return 5;
            // Burger
            case "Burger Bread Down":
                return 0;
            case "Burger Bread Up":
                return 1;
            case "Cutlet B":
                return 2;
            case "Bacon Slice":
                return 3;
            case "Cheese Slice A":
                return 4;
            case "Onion Slice White":
                return 5;
            case "Tomato Slice":
                return 6;
            case "Salad Slice":
                return 7;
            default:
                return -1;
        }
    }

    public GameObject GetPersistentGO()
    {
        return persistentGO;
    }

    public GameObject GetNotificationPrefab()
    {
        return notificationSetManager.GetComponent<NotificationSetManager>().GetNotificationPrefab();
    }

    public void SetNotificationType(int x)
    {
        notificationSetManager.GetComponent<NotificationSetManager>().SetNotificationType(x);
    }

    public int GetNotificationType()
    {
        return notificationSetManager.GetComponent<NotificationSetManager>().GetNotificationType();
    }

    public void AddNotificationOnDock(string text)
    {
        notificationSetManager.GetComponent<NotificationSetManager>().AddNotificationOnDock(text);
    }

    public void AddNotificationOnViewport(string text)
    {
        notificationSetManager.GetComponent<NotificationSetManager>().AddNotificationOnViewport(text);
    }
}
