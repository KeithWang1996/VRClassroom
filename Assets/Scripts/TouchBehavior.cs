using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TouchBehavior : MonoBehaviour {
    //public OVRInput.Controller l_controller = OVRInput.Controller.LTouch;
   // public OVRInput.Controller r_controller = OVRInput.Controller.RTouch;
    
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        /*
        OVRInput.Update();
        Vector3 l_position = OVRInput.GetLocalControllerPosition(l_controller);
        Vector3 r_position = OVRInput.GetLocalControllerPosition(r_controller);
        RaycastHit hit;

        Vector3 fwd = OVRInput.GetLocalControllerRotation(r_controller).eulerAngles;
        if(Physics.Raycast(r_position, fwd,out hit, 10000))
        {
            GameObject target = hit.collider.gameObject;
            print("hit something");
        }
        else
        {
            /print(" ");
        }
        //print(t_position.x);
        //l_hand.GetComponent<Transform>().position = l_position;
        //r_hand.GetComponent<Transform>().position = r_position;
       // r_hand.GetComponent<Transform>().rotation = OVRInput.GetLocalControllerRotation(r_controller);*/
    }
}
