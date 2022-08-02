using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    float speed = 0.08f;
    Vector3 movement = new Vector3(0, 1, 0);
    Vector3 rotationAngle = new Vector3(0, 240, 0);
    float maxDistance = -0.03f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movement * speed * Time.deltaTime);
        transform.Rotate(rotationAngle * Time.deltaTime);
        if (transform.localPosition.y > 0)
            movement = new Vector3(0, 1, 0);
        else if (transform.localPosition.y < maxDistance)
            movement = new Vector3(0, -1, 0);
    }
}
