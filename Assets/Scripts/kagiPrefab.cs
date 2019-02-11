using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kagiPrefab : MonoBehaviour {


    public bool touchKagi;
    public bool deleteKagi;
    // Use this for initialization
    void Start () {
        touchKagi = false;
        deleteKagi = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "cube")
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            touchKagi = true;
        }
        if (layerName == "cubeSky")
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            touchKagi = true;
        }
        if (layerName == "delete")
        {
            deleteKagi = true;
        }
    }




}
