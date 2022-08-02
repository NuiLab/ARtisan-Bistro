using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamTrayProperties : MonoBehaviour
{
    public int boxCollisions = 0;
    float timeElapsed = 0;
    [SerializeField] GameObject iceCreamScoop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > 5)
        {
            boxCollisions = 0;
            timeElapsed = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scoop"))
            boxCollisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Scoop"))
        {
            if (boxCollisions == 3)
            {
                GameObject iceCreamScoopObject = Instantiate(iceCreamScoop);
                iceCreamScoopObject.transform.SetParent(other.transform);
                iceCreamScoopObject.transform.localPosition = other.transform.GetChild(0).localPosition;
            }
        }
    }
}
