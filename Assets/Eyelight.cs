using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyelight : MonoBehaviour
{
    MeshRenderer buttonBackplate;
    Material stockMaterial;
    // Start is called before the first frame update
    void Start()
    {
        buttonBackplate = GetComponentInChildren<MeshRenderer>();
        buttonBackplate.material = new Material(buttonBackplate.material);
        stockMaterial = buttonBackplate.material;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void highlight()
    {
        Color temp = buttonBackplate.material.color;
        temp.r += .007f;
        temp.g += .007f;
        temp.b += .01f;
        buttonBackplate.material.color = temp;
    }
    public void resetMat() {
        buttonBackplate.material = stockMaterial;
     }
}
