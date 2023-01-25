using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMakerManager : MonoBehaviour
{

    [SerializeField] Vector3 coffeePotPos;
    [SerializeField] GameObject playArea;
    [SerializeField] GameObject coffeePotGlass;
    [SerializeField] GameObject coffeeMakerBody;
    [SerializeField] GameObject coffeeLevel_GO;
    [SerializeField] GameObject coffeeLevelMeter;
    [SerializeField] float coffeeFillRate = 2;
    [SerializeField] float maxCoffeeLevel = 0.05f;
    [SerializeField] GameObject globalRecords_GO;
    [SerializeField] GameObject statusLight;



    bool coffeeMakerOn = false;
    Renderer rend;
    float coffeeLevel = -0.05f;
    int coffeeCupCnt = 0;
    GameObject notification_GO;

    // Start is called before the first frame update
    void Start()
    {
        globalRecords_GO = GameObject.FindWithTag("Global Records");
        Physics.IgnoreCollision(coffeePotGlass.GetComponent<MeshCollider>(), coffeeMakerBody.GetComponent<MeshCollider>(), true);
        rend = coffeeLevel_GO.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coffeeMakerOn)
            statusLight.GetComponent<StatusLight>().setOn();
        else
            statusLight.GetComponent<StatusLight>().setOff();

        if (coffeeMakerOn && coffeeLevel < maxCoffeeLevel && transform.GetChild(1).GetComponent<CoffeePotManager>().GetPlaced())
        {
            coffeeLevel += coffeeFillRate * 0.001f * Time.deltaTime;
            rend.material.SetFloat("FillLevel", coffeeLevel);
        }
        else if (coffeeMakerOn && coffeeLevel >= maxCoffeeLevel && transform.GetChild(1).GetComponent<CoffeePotManager>().GetPlaced())
        {
            TurnOnCoffeeMaker();
        }

        if (((coffeeLevel + maxCoffeeLevel) * 3) / (maxCoffeeLevel * 2) > coffeeCupCnt + 1)
        {
            coffeeCupCnt++;
            if (globalRecords_GO.GetComponent<Records>().GetPersistentGO().GetComponent<PersistentGOManager>().GetShowNotification())
            {

                GameObject tempNoti = globalRecords_GO.GetComponent<Records>().addIngredientNotification("Kaffeemaschine", "Becher", transform.GetInstanceID());
                tempNoti.GetComponent<Notification>().ReceiveInput("Kaffee", "Neuer Becher", "eff0");
            }

        }
    }

    public Vector3 GetCoffeePotPos()
    {
        return coffeePotPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coffee_Pot") && !other.GetComponent<CoffeePotManager>().GetPlaced())
        {
            if (!other.GetComponent<CoffeePotManager>().GetIsGrabbed())
            {
                // other.transform.parent = transform;
                other.transform.localPosition = coffeePotPos;
                other.transform.localRotation = Quaternion.identity;
                other.GetComponent<CoffeePotManager>().SetPlaced(true);
                coffeeLevel = other.GetComponent<CoffeePotManager>().GetCoffeeLevel();
            }
        }
        if (other.CompareTag("Coffee_Pot") && other.GetComponent<CoffeePotManager>().GetPlaced() && !coffeeLevelMeter.activeSelf)
        {
            coffeeLevelMeter.SetActive(true);
            coffeeLevel_GO.SetActive(true);
        }
    }

    public void TurnOnCoffeeMaker()
    {
        if (coffeeMakerOn)
        {
            coffeeMakerOn = false;
        }
        else
        {
            coffeeMakerOn = true;
            coffeeLevel = rend.material.GetFloat("FillLevel");
        }
    }

    public float GetCoffeeLevel()
    {
        return coffeeLevel;
    }
}
