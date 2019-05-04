using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIntensity : MonoBehaviour
{
    public Transform arrowPoint;
    public Transform posArrow;
    public Transform arrowBody;
    public SwipeScript swipeScript;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        arrowPoint.transform.position = posArrow.position;
        arrowPoint.transform.rotation = posArrow.rotation;
        arrowBody.rotation = Quaternion.Euler(90,swipeScript.trajetory.transform.rotation.y *Mathf.Rad2Deg,0);
    }
    void Update()
    {
        
    }
}
