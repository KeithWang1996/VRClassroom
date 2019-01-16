using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class lefthand : MonoBehaviour {
    public GameObject desk;
    public GameObject chair;
    public GameObject locker;
    public GameObject cabinet;
    public GameObject TV;
    public GameObject board;
    GameObject[] objects;
    string[] objects_name;
    GameObject togenerate;
    GameObject stuff;
    GameObject stuff2;
    bool flag;
    bool isgroup;
    bool triggered;
    GameObject pivot;
    GameObject handruler;
    LineRenderer line;
    LineRenderer ruler;
    public Material l_material;
    public Material r_material;
    GameObject tocopy;
    GameObject scene;
    Vector3 average;
    Vector3 diff;
    public Text d_text;
    public Text g_text;
    int switch_counter;
    int switch_index;
    float scale;
    float quo;
    //public GameObject chair;
    // Use this for initialization
    void Start () {
        objects = new GameObject[]{desk, chair, locker, cabinet, TV, board};
        objects_name = new string[] {"desk", "chair", "locker", "cabinet", "TV", "board"};
        switch_index = 0;
        togenerate = objects[0];
        switch_counter = 30;
        handruler = new GameObject();
        handruler.transform.position = transform.position;
        handruler.transform.parent = transform;
        average = Vector3.zero;
        tocopy = null;
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.positionCount = 2;
        line.material = l_material;
        ruler = handruler.AddComponent<LineRenderer>();
        ruler.startWidth = 0.01f;
        ruler.endWidth = 0.01f;
        ruler.positionCount = 2;
        ruler.material = r_material;
        ruler.GetComponent<Renderer>().enabled = false;
        flag = false;
        isgroup = false;
        triggered = false;
        scale = 1;
        scene = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + fwd * 800);
        fwd = Vector3.Normalize(fwd)*2;

        Vector2 stick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        if(stick.x > 0.8)
        {
            switch_counter -= 1;
            if (switch_counter <= 0)
            {
                if(switch_index < 5)
                {
                    switch_index += 1;

                }
                else
                {
                    switch_index = 0;
                }
                switch_counter = 30;
            }
        }

        else if(stick.x < -0.8)
        {
            switch_counter -= 1;
            if (switch_counter <= 0)
            {
                if (switch_index > 0)
                {
                    switch_index -= 1;

                }
                else
                {
                    switch_index = 5;
                }
                switch_counter = 30;
            }
        }

        else
        {
            switch_counter = 30;
        }

        togenerate = objects[switch_index];
        g_text.text = "Generating: " + objects_name[switch_index];

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) == 1 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) == 1)
        {
            GameObject[] controllers;
            controllers = GameObject.FindGameObjectsWithTag("GameController");
            if (!triggered)
            {
                scale = Vector3.Distance(controllers[0].transform.position, controllers[1].transform.position);
                triggered = true;
            }
            float scale2 = Vector3.Distance(controllers[0].transform.position, controllers[1].transform.position);
            quo = scale2 / scale;
            Transform[] all = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];
            //print(quo);
            foreach(Transform i in all)
            {
                if (i.tag != "EditorOnly" || i.tag != "GameController")
                {
                    i.parent = scene.transform;
                }
            }

            scene.transform.localScale = scene.transform.localScale * quo;

        }
        else
        {
            triggered = false;
        }
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) == 1 && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) == 1)
        {

        }

      

        if (Physics.Raycast(transform.position, fwd, out hit, 100))
        {
            line.SetPosition(1, hit.point);

            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
            {

                ruler.SetPosition(0, hit.point);

            }

            if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))
            {
                ruler.SetPosition(1, hit.point);
                ruler.GetComponent<Renderer>().enabled = true;
            }
            if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch))
            {
                ruler.GetComponent<Renderer>().enabled = false;
                d_text.text = "Distance: " + (Vector3.Distance(ruler.GetPosition(0), ruler.GetPosition(1))*2.5f).ToString("0.00") + "m";
            }

            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) == 1)
            {


                if (hit.collider.gameObject.name != "Plane" && hit.collider.gameObject.name != "room")
                {
                    tocopy = hit.collider.gameObject;
                    if (tocopy.tag == "grouped")
                    {
                        isgroup = true;
                        tocopy = tocopy.transform.parent.gameObject;
                        for (int i = 0; i < tocopy.transform.childCount; i++)
                        {
                            average += tocopy.transform.GetChild(i).position;
                        }
                        average = average / tocopy.transform.childCount;
                        diff = tocopy.transform.position - hit.point;
                    }
                    else
                    {
                        isgroup = false;
                    }

                }
            }
        }

        else {
            ruler.GetComponent<Renderer>().enabled = false;
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) == 1)
        {   if (tocopy != null && flag == false)
            {
                if (isgroup)
                {
                    stuff2 = Instantiate(tocopy, transform.position + fwd + diff, tocopy.transform.rotation);
                    int num = stuff2.transform.childCount;
                    for (int i = 0; i < num; i++)
                    {
                        stuff2.transform.GetChild(0).tag = "Untagged";
                        stuff2.transform.GetChild(0).parent = null;
                    }
                    
                }
                else
                {
                    Instantiate(tocopy, transform.position + fwd, tocopy.transform.rotation);
                }
                flag = true;
            }
        
        }
        

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch) == 0)
        {
            
            flag = false;
            
        }

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            //print("x pressed");
            stuff = Instantiate(togenerate, transform.position + fwd, togenerate.transform.rotation);
            stuff.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            //stuff.transform.position = transform.position + fwd;
            stuff.GetComponent<Rigidbody>().AddForce((transform.position + fwd - stuff.transform.position) * 800);
            //print(stuff.transform.position.x);
        }
        if(OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            stuff.GetComponent<Rigidbody>().useGravity = true;
            stuff.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
