using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigibodyHandler : MonoBehaviour
{
    public Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Kinematics");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setKinematic(Boolean kinematic) { 
        body.isKinematic = kinematic;
    }

    IEnumerator Kinematics() { 
     yield return new WaitForSeconds(0.1f);
        unfreeze(); 
}
    public void unfreeze() {
        body.constraints = RigidbodyConstraints.None;
    } 
    

}
