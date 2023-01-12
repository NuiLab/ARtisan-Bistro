using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerTrayManager : MonoBehaviour
{
    int numObjects = 0;
    GameObject persistentGO;
    public CustomerManager cManager;
    public GameObject records; 
    IMixedRealitySceneSystem sceneSystem;
    [SerializeField]
     AudioSource correct;
    [SerializeField]
    AudioSource wrong;

    // Start is called before the first frame update
    void Start()
    {
        records = GameObject.FindGameObjectsWithTag("Global Records")[0];
        sceneSystem = MixedRealityToolkit.Instance.GetService<IMixedRealitySceneSystem>();
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        persistentGO = GameObject.FindGameObjectsWithTag("PersistentGO")[0];
        numObjects++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coffee_Cup") || other.CompareTag("Ingredient_Base"))
        {
            if (!other.GetComponent<ObjectManager>().isGrabbed)
            {
                List<string> preparedFood = CreateIngredientsList(other.gameObject);
                if (transform.parent.GetComponent<CustomerManager>().CheckIndredients(preparedFood, other.gameObject))
                {
                    persistentGO.GetComponent<PersistentGOManager>().AddData("Food Served", "Correct Food", 2, CreateIngredientsString(other.gameObject));
                    transform.parent.transform.GetComponentInParent<ServingStationManager>().RemoveCustomer(transform.parent.gameObject);
                    if (!sceneSystem.IsContentLoaded("Instructions Scene"))
                    {
                        records.GetComponent<Records>().score += cManager.timerRemaining;
                        records.GetComponent<Records>().scoreboard.GetComponent<TextMeshProUGUI>().SetText("Score: " + records.GetComponent<Records>().score);
                    }
                    Destroy(other.gameObject);
                    // Destroy(transform.parent.gameObject);
                    AudioSource audio = GetComponent<AudioSource>();
                    audio.Play();
                    StartCoroutine(CustomerLeave());
                }
                else
                {
                    if (!transform.GetChild(0).gameObject.activeSelf)
                    {
                        persistentGO.GetComponent<PersistentGOManager>().AddData("Food Served", "Wrong Food", 2, CreateIngredientsString(other.gameObject));
                        transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                persistentGO.GetComponent<PersistentGOManager>().AddData("Food Served", "Not Food", 2);
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

        if(preparedFood[0] == "Dough Ketchup")
        {
            for (int i = 3; i < foodItem.transform.childCount; i++)
            {
                preparedFood.Add(foodItem.transform.GetChild(i).GetComponentInChildren<IngredientProperties>().GetPrefabName());
            }
        }

        if(preparedFood[0] == "Burger Bread Down")
        {
            for (int i = 2; i < foodItem.transform.childCount; i++)
            {
                preparedFood.Add(foodItem.transform.GetChild(i).GetComponentInChildren<IngredientProperties>().GetPrefabName());
            }
        }
        return preparedFood;
    }

    string CreateIngredientsString(GameObject foodItem)
    {
        string preparedFood = foodItem.GetComponent<IngredientProperties>().GetPrefabName();
        if (preparedFood == "CoffeeCup")
            return "[" + preparedFood + "]";

        if(preparedFood == "Dough Ketchup")
        {
            for (int i = 3; i < foodItem.transform.childCount; i++)
            {
                preparedFood = preparedFood + ";" + foodItem.transform.GetChild(i).GetComponentInChildren<IngredientProperties>().GetPrefabName();
            }
        }

        if (preparedFood == "Burger Bread Down")
        {
            for (int i = 2; i < foodItem.transform.childCount; i++)
            {
                preparedFood = preparedFood + ";" + foodItem.transform.GetChild(i).GetComponentInChildren<IngredientProperties>().GetPrefabName();
            }
        }
        return "[" + preparedFood + "]";
    }

    IEnumerator CustomerLeave() {
       
        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);
    }
}
