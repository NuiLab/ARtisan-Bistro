using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] GameObject speechBubble;
    [SerializeField] string[] contentArray;
    [SerializeField] GameObject meshPos;
    [SerializeField] GameObject speechBubblePos;
    [SerializeField] GameObject[] malePrefabs;
    [SerializeField] GameObject[] femalePrefabs;

    GameObject speechBubble_GO;
    float customerLifeTime = 60;
    List<string> ingredients = new List<string>();
    int difficultyLevel = 0;
    string[] currCustomerNames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
      private IEnumerator CustomerLifeFunctions(string foodItem)
    {
        speechBubble_GO = Instantiate(speechBubble);
        speechBubble_GO.transform.parent = transform;
        speechBubble_GO.transform.localPosition = speechBubblePos.transform.localPosition;
        speechBubble_GO.GetComponent<SpeechBubbleManager>().SetContent(ingredients, foodItem);
        speechBubble_GO.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        InitializeMesh();
        yield return new WaitForSeconds(customerLifeTime);
        transform.GetComponentInParent<ServingStationManager>().RemoveCustomer(transform.gameObject);
        Destroy(gameObject);
    }

    public void CreateCustomer(float custLife, string foodItem, int custPos, string[] customerNames)
    {
        // difficultyLevel = custPos;                      // For testing.
        currCustomerNames = customerNames;
        customerLifeTime = custLife;
        InitializeFood(foodItem);
        StartCoroutine(CustomerLifeFunctions(foodItem));
    }

    void InitializeFood(string foodItem)
    {
        switch (foodItem)
        {
            case "Pizza":
                CreatePizza();
                break;
            case "Burger":
                CreateBurger();
                break;
            case "Coffee":
                CreateCoffee();
                break;
        }
    }

    void CreatePizza()
    {
        string[] tmpArray = new string[] { "Dough Ketchup", "Pepperoni Layer", "Pepper Green Layer", "Olive Black Slice Layer", "Mushroom Slice Layer", "Basil Leaf Layer" };
        ingredients = new List<string> { "Dough Ketchup" };
        for (int i = 0; i < difficultyLevel + 3; i++)
        {
            int tmpInt = Random.Range(0, tmpArray.Length);
            if (tmpInt == 0 || ingredients.Contains(tmpArray[tmpInt]))
                i--;
            else
                ingredients.Add(tmpArray[tmpInt]);
        }
    }

    void CreateBurger()
    {
        string[] tmpArray = new string[] { "Burger Bread Down", "Burger Bread Up", "Cutlet B", "Bacon Slice", "Cheese Slice A", "Onion Slice", "Tomato Slice", "Salad Slice" };
        ingredients = new List<string> { "Burger Bread Down", "Cutlet B" };
        for (int i = 0; i < difficultyLevel + 3; i++)
        {
            int tmpInt = Random.Range(0, tmpArray.Length);
            if (tmpInt == 0 || tmpInt == 1 || tmpInt == 2 || ingredients.Contains(tmpArray[tmpInt]))
                i--;
            else
                ingredients.Add(tmpArray[tmpInt]);
        }
        ingredients.Add("Burger Bread Up");
    }

    void CreateCoffee()
    {
        ingredients = new List<string>() { "CoffeeCup" };
    }

    void InitializeMesh()
    {
        int gender = Random.Range(0, 2);
        if (gender == 0)
        {
            GameObject temp;
            do
            {
                temp = malePrefabs[Random.Range(0, malePrefabs.Length)];
            } while (currCustomerNames.Contains(temp.name));
            GameObject maleCustomer = Instantiate(temp);
            maleCustomer.transform.SetParent(transform);
            maleCustomer.transform.localPosition = meshPos.transform.localPosition;
            maleCustomer.GetComponent<Animator>().SetInteger("Idle", Random.Range(0, 2));
        }
        else
        {
            GameObject temp;
            do
            {
                temp = femalePrefabs[Random.Range(0, femalePrefabs.Length)];
            } while (currCustomerNames.Contains(temp.name));
            GameObject femaleCustomer = Instantiate(temp);
            femaleCustomer.transform.SetParent(transform);
            femaleCustomer.transform.localPosition = meshPos.transform.localPosition;
            femaleCustomer.GetComponent<Animator>().SetInteger("Idle", Random.Range(0, 2));
        }
    }

    public bool CheckIndredients(List<string> preparedFood)
    {
        if (preparedFood[0] == "CoffeeCup" && ingredients.Contains(preparedFood[0]))
            return true;
        if (preparedFood.Count != difficultyLevel + 3)
            return false;
        for (int i = 0; i < preparedFood.Count; i++)
        {
            if (!ingredients.Contains(preparedFood[i]))
            {
                return false;
            }
        }
        return true;
    }

    public string CreateIngredientsString()
    {
        string ingredientString = "";
        foreach (var ingredient in ingredients)
        {
            if (ingredientString == "")
                ingredientString = ingredient;
            else
                ingredientString = ingredientString + ";" + ingredient;
        }
        return "[" + ingredientString + "]";
    }
}
