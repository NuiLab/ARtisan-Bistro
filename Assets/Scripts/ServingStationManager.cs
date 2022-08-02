using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingStationManager : MonoBehaviour
{
    [SerializeField] GameObject customerPrefab;
    [SerializeField] GameObject[] customerPositionGO;
    [SerializeField] GameObject globalRecords_GO;

    Dictionary<int, GameObject> customers = new Dictionary<int, GameObject>();
    Vector3[] customerPositions = new Vector3[3];
    List<string> foodItems = new List<string>() { "Pizza", "Burger", "Coffee" };
    List<string> foodItemsLeft = new List<string>() { "Pizza", "Burger", "Coffee" };
    int numCustomers = 0;
    string[] currCustomerNames;
    GameObject notification_GO;


    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
        for (int i = 0; i < customerPositionGO.Length; i++)
        {
            customers.Add(i, null);
            customerPositions[i] = customerPositionGO[i].transform.localPosition;
        }
        /*
        customers.Add(0, null);
        customers.Add(1, null);
        customers.Add(2, null);
        customerPositions[0] = new Vector3(-0.7f, 1.2f, 0.76f);
        customerPositions[1] = new Vector3(0, 1.2f, 0.76f);
        customerPositions[2] = new Vector3(0.7f, 1.2f, 0.76f);
        */
        StartCoroutine(BringCustomer());
    }

    // Update is called once per frame
    

    public void AddCustomer(int custPos, GameObject custRef)
    {
        numCustomers++;
        customers[custPos] = custRef;
        custRef.transform.parent = transform;
        custRef.transform.localPosition = customerPositions[custPos];
        custRef.transform.localRotation = Quaternion.identity;
        custRef.GetComponent<CustomerManager>().CreateCustomer(60, GetFoodItem(), custPos, currCustomerNames);

        if (globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().GetShowNotification())
        {
            switch (globalRecords_GO.GetComponent<Records>().GetNotificationType())
            {
                case 0:
                    if (notification_GO != null)
                        Destroy(notification_GO);
                    notification_GO = Instantiate(globalRecords_GO.GetComponent<Records>().GetNotificationPrefab());
                    notification_GO.GetComponent<NotificationManager>().SetNotificationProperties("New Customer", transform.gameObject, new Vector3(-0.3f, 1.75f, 1.2f));
                    break;
                case 1:
                    globalRecords_GO.GetComponent<Records>().AddNotificationOnDock("New Customer");
                    break;
                case 2:
                    globalRecords_GO.GetComponent<Records>().AddNotificationOnViewport("New Customer");
                    break;
            }
        }
    }

    public void RemoveCustomer()
    {
        numCustomers--;
    }


    // Testing functions
    private IEnumerator BringCustomer()
    {
        while (numCustomers < 3)
        {
            GameObject tempCustomer = Instantiate(customerPrefab);
            for (int i = 0; i < 3; i++)
            {
                if (customers[i] == null)
                {
                    currCustomerNames = CustomerNames();
                    AddCustomer(i, tempCustomer);
                    break;
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    string GetFoodItem()
    {
        if (foodItemsLeft.Count == 0)
            foodItemsLeft = new List<string>(foodItems);
        int foodItemIndex = Random.Range(0, foodItemsLeft.Count);
        string foodItem = foodItemsLeft[foodItemIndex];
        foodItemsLeft.RemoveRange(foodItemIndex, 1);
        return foodItem;
    }

    string[] CustomerNames()
    {
        string[] names = new string[transform.childCount - 3];

        for (int i = 3; i < transform.childCount; i++)
        {
            names[i - 3] = GetPrefabName(transform.GetChild(i).GetChild(4).gameObject);
        }
        return names;
    }

    string GetPrefabName(GameObject temp_GO)
    {
        string objectName = temp_GO.transform.name;
        for (int i = 0; i < objectName.Length; i++)
        {
            if (((objectName[i] - '0') <= 9 && (objectName[i] - '0') >= 0) || objectName[i] == '(')     // Prefab names are only made of alphabets. Number or Brackets signify copies
            {
                return objectName.Substring(0, i);
            }
        }
        return objectName;
    }
}
