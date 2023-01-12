using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusLight : MonoBehaviour
   
   
{
    [SerializeField]
    Material onMat;
    [SerializeField]
    Material offMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setOn() {
        this.gameObject.GetComponent<MeshRenderer>().material = onMat;
    }
    public void setOff()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = offMat;
    }

}
