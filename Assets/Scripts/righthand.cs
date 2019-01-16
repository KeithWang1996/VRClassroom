using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class righthand : MonoBehaviour {
    // Use this for initialization
    Vector3 new_position;
    Vector3 old_position;
    Vector3 ori_position;
    Vector3 ori_position2;
    LineRenderer line;
    float distance;
    float distance2;
    Vector3 error;
    Quaternion rotation;
    Quaternion rotation2;
    GameObject pivot;
    GameObject prev_target;
    public Material l_material;
    public Text g_text;
    List<GameObject> group;
    GameObject thegroup;
    MeshRenderer[] renderers;
    List<List<Material[]>> materials;
    public Material highlight;
    Material[] highlights;
    GameObject target;
    bool hasgroup;
    bool selectgroup;
    void Start () {

        new_position = transform.parent.parent.parent.position;
        old_position = new_position;
        distance = 0;
        error = Vector3.zero;
        rotation = Quaternion.identity;
        pivot = new GameObject();
        pivot.transform.position = Vector3.zero;
        pivot.AddComponent<Rigidbody>();
        pivot.GetComponent<Rigidbody>().useGravity = false;
        pivot.GetComponent<Rigidbody>().drag = 6;
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.positionCount = 2;
        line.startColor = Color.black;
        line.startColor = Color.black;
        line.material = l_material;
        ori_position = Vector3.zero;
        group = new List<GameObject>();
        thegroup = new GameObject();

        materials = new List<List<Material[]>>();
        highlights = new Material[] { highlight, highlight, highlight, highlight, highlight, highlight };
        hasgroup = false;
        selectgroup = false;
    }
	
	// Update is called once per frame
	void Update () {
       
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;



        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + fwd * 800);

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) == 1 && !OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            if (Physics.Raycast(transform.position, fwd, out hit, 100))
            {
                if (!hasgroup)
                {
                    target = hit.collider.gameObject;
                    if (target.tag != "grouped" && target.name != "Plane" && target.name != "room")
                    {
                        target.tag = "grouped";
                        group.Add(target);
                        target.transform.parent = thegroup.transform;
                        renderers = target.GetComponentsInChildren<MeshRenderer>();
                        List<Material[]> store = new List<Material[]>();
                        foreach (MeshRenderer i in renderers)
                        {
                            store.Add(i.materials);
                            i.materials = highlights;
                        }
                        materials.Add(store);
                    }
                }
            }

        }

        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) == 0 && !OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch) && materials != null && materials.Count != 0)
        {
            for (int i = 0; i < group.Count; i++)
            {
                GameObject temp = group[i];
                renderers = temp.GetComponentsInChildren<MeshRenderer>();
                List<Material[]> store = materials[i];
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].materials = store[j];
                }
            }
            materials.Clear();
        }

        else if (group.Count != 0 && hasgroup == false)
        {
            hasgroup = true;
            g_text.text = "grouped";
        }

        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) == 1 && !OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {

            if (group.Count != 0)
            {
                for (int i = 0; i < group.Count; i++)
                {
                    GameObject temp = group[i];
                    temp.tag = "Untagged";
                    temp.GetComponent<Rigidbody>().isKinematic = false;
                    temp.transform.parent = null;

                }
            }
            thegroup.transform.parent = null;
            group.Clear();
            
            hasgroup = false;
            g_text.text = "ungrouped";
        }

        if (distance2 > 0.02)
        {
            transform.parent.parent.parent.Translate(Vector3.Normalize(new_position - old_position) * 0.02f);
            distance2 -= 0.02f;
        }
        if (Physics.Raycast(transform.position, fwd, out hit, 100))
        {
            //print("There is something in front of the object!");

            line.SetPosition(1, hit.point);

            //Debug.DrawRay(transform.position, fwd);
            target = hit.collider.gameObject;

            if (target.name == "Plane")
            {

                if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
                {
                    new_position = hit.point;
                    old_position = transform.parent.parent.parent.position;
                    new_position.y = new_position.y + 0.45f;
                    distance2 = Vector3.Distance(new_position, old_position);

                }

            }
            else if(target.name == "room")
            {
                return;
            }
            else
            {
                if (target.tag == "grouped")
                {
                    
                    if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                    {
                        selectgroup = true;
                        distance = Vector3.Distance(hit.point, transform.position);
                        ori_position = hit.point;
                        if (target.transform.parent != null)
                        {
                            pivot.transform.position = hit.point;
                            target.transform.parent.SetParent(pivot.transform);
                            ori_position2 = pivot.transform.position;
                            Rigidbody[] rigids;
                            rigids = target.transform.parent.GetComponentsInChildren<Rigidbody>();
                            foreach (Rigidbody i in rigids)
                            {
                                i.isKinematic = true;
                            }

                            rotation = pivot.transform.rotation;
                            rotation2 = transform.rotation;
                        }

                    }
                    if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                    {
                        selectgroup = true;
                        if (target.transform.parent != null)
                        {
                            pivot.GetComponent<Rigidbody>().AddForce((ori_position2 + (transform.position + fwd * distance - ori_position) - pivot.transform.position) * 800);
                            pivot.transform.rotation = transform.rotation * Quaternion.Inverse(rotation2) * rotation;
                        }
                        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) == 1)
                        {
                            //print("trigger1 detected");
                            distance += 0.01f;
                        }

                        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) == 1)
                        {
                            // print("trigger2 detected");
                            if (distance > 0.5)
                            {
                                distance -= 0.01f;
                            }
                        }
                    }


                    if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch) || !selectgroup)
                    {

                        if (target.transform.parent != null)
                        {
                            target.transform.parent.parent = null;
                            Rigidbody[] rigids;
                            rigids = target.transform.parent.GetComponentsInChildren<Rigidbody>();
                            foreach (Rigidbody i in rigids)
                            {
                                i.isKinematic = false;
                            }
                            selectgroup = false;
                        }
                    }
                    
                }
                else
                {
                    
                    if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || target != prev_target)
                    {
                        
                        distance = Vector3.Distance(target.transform.position, transform.position);
                        error = target.transform.position - transform.position;
                        rotation = target.transform.rotation;
                        rotation2 = transform.rotation;

                    }
                    if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                    {
                        if(selectgroup)
                        {
                            
                            if (thegroup.transform.childCount != 0)
                            {
                                thegroup.transform.parent = null;
                                Rigidbody[] rigids;
                                rigids = thegroup.transform.GetComponentsInChildren<Rigidbody>();
                                foreach (Rigidbody i in rigids)
                                {
                                    i.isKinematic = false;
                                }
                                selectgroup = false;
                            }
                        }
                        target.GetComponent<Rigidbody>().AddForce((transform.position + distance * fwd - target.transform.position) * 800);
                        target.transform.rotation = transform.rotation * Quaternion.Inverse(rotation2) * rotation;
                        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) == 1)
                        {
                            //print("trigger1 detected");
                            distance += 0.01f;
                        }

                        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) == 1)
                        {
                            // print("trigger2 detected");
                            if (distance > 0.5)
                            {
                                distance -= 0.01f;
                            }
                        }

                    }
                    if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                    {
                        target.GetComponent<Rigidbody>().useGravity = true;
                        error = Vector3.zero;
                    }
                }
            }
            prev_target = target;
        }
        else
        {
            if (target != null && target.transform.parent != null)
            {

                target.transform.parent.parent = null;
                Rigidbody[] rigids;
                rigids = target.transform.parent.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody i in rigids)
                {
                    i.isKinematic = false;
                    i.useGravity = true;
                    i.constraints = RigidbodyConstraints.None;
                }

            }
        }

    }
    
}
