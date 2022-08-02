using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePotManager : MonoBehaviour
{
    [SerializeField] GameObject coffeeLevelMeter;
    [SerializeField] GameObject coffeeLevel_GO;
    [SerializeField] Transform origin = null;
    [SerializeField] GameObject streamPrefab = null;

    [SerializeField] float coffeePourRate = 2;


    bool isGrabbed = false;
    bool placed = true;
    Renderer rend;
    float coffeeLevel = 0;
    float pourThreshold = 50;
    bool isPouring = false;
    Stream currentStream = null;
    float minCoffeeLevel = -0.05f;


    // Start is called before the first frame update
    void Start()
    {
        rend = coffeeLevel_GO.GetComponent<Renderer>();

        // coffeeLevel = rend.material.GetFloat("FillLevel");          /////////// Remove this
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.GetComponent<Rigidbody>().isKinematic && placed)
            transform.GetComponent<Rigidbody>().isKinematic = true;
        else if (transform.GetComponent<Rigidbody>().isKinematic && !placed)
            transform.GetComponent<Rigidbody>().isKinematic = false;

        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }

        if (isPouring && coffeeLevel > minCoffeeLevel)
        {
            pourThreshold = ((coffeeLevel + 0.05f) * 460) + 10;
            if (isPouring)
            {
                coffeeLevel -= coffeePourRate * 0.001f * Time.deltaTime;
                rend.material.SetFloat("FillLevel", coffeeLevel);
            }
        }
        else if (isPouring && coffeeLevel <= minCoffeeLevel)
        {
            coffeeLevel_GO.SetActive(false);
            EndPour();
        }
    }

    public float GetCoffeeLevel()
    {
        return coffeeLevel;
    }

    public void SetIsGrabbed()
    {
        isGrabbed = true;
        placed = false;
        coffeeLevelMeter.SetActive(false);
        coffeeLevel = rend.material.GetFloat("FillLevel");
    }

    public void SetIsNotGrabbed()
    {
        isGrabbed = false;
        transform.rotation = Quaternion.identity;
    }

    public bool GetIsGrabbed()
    {
        return isGrabbed;
    }

    public void SetPlaced(bool x)
    {
        placed = x;
    }

    public bool GetPlaced()
    {
        return placed;
    }

    public GameObject GetCoffeeLevelMeterObject()
    {
        return coffeeLevelMeter;
    }

    public bool GetIsPouring()
    {
        return isPouring;
    }

    void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
    }

    void EndPour()
    {
        if (currentStream != null)
        {
            currentStream.End();
            currentStream = null;
        }
    }

    float CalculatePourAngle()
    {
        return transform.up.y * Mathf.Rad2Deg;
    }

    Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}
