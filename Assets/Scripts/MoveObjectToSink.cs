using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Leap.Unity.Interaction
{


public class MoveObjectToSink : MonoBehaviour
{

    
    public LeapServiceProvider LeapServiceProvider;
    private Vector pos_thumb;
    private Vector pos_index;
    private GameObject testSphere_index;
    private GameObject testSphere_thumb;

    //private GameObject Middel;

    // public float fingerDistance;
    // public float erreurDistance;
        
    // private float distance;

    public float maxGrabAngle;

    private Hand _hand;

    // Start is called before the first frame update
    void Start()
    {
        testSphere_index =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSphere_index.transform.localScale = new Vector3(0.01f, 0.01f,0.01f);
        testSphere_index.AddComponent<Rigidbody>();

        // testSphere_thumb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // testSphere_thumb.transform.localScale = new Vector3(0.01f, 0.01f,0.01f);
        

        // Middel = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // Middel.transform.localScale = new Vector3(0.01f, 0.01f,0.01f);
        // Middel.name = "middel";
        
        //Middel.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameObject.Find("D52/Pull").name);
        for (int i = 0; i < LeapServiceProvider.CurrentFrame.Hands.Count; i++)
        {
            
        _hand = LeapServiceProvider.CurrentFrame.Hands[i];
        //Debug.Log(_hand.GrabAngle);
        Finger _thumb = _hand.GetThumb();
        //Debug.Log("direction of thumb: "+ _thumb.Direction);
        Finger _index = _hand.GetIndex();
        pos_thumb = _thumb.TipPosition;
        pos_index = _index.TipPosition;

        //Debug.Log(transform.position);
        UpdateHand(new Vector3(pos_index.x,pos_index.y,pos_index.z), new Vector3(pos_thumb.x, pos_thumb.y, pos_thumb.z));
        }

    }

    void UpdateHand(Vector3 pos_index, Vector3 pos_thumb)
    {
        testSphere_index.transform.position = pos_index;

        //float activate = Distance_between_two_points(Middle_of_two_points(pos_index, pos_thumb), transform.position);

        //Debug.Log(activate);
    }

    bool AcceptHandPosition(Hand _hand)
    {
        //Debug.Log(_hand.GrabAngle);
        //Debug.Log(_hand.Rotation);
        if(/*(_hand.Rotation.x>0.3) && (_hand.Rotation.x<0.7) &&*/ (_hand.GrabAngle>maxGrabAngle))
        {
            return true;
        }else
        {
            return false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(/*AcceptFingerDistance(distance, fingerDistance, erreurDistance) ||*/ AcceptHandPosition(_hand))
        {
            Debug.Log("touch object");
        }

        
        
        

    }

    void OnTriggerStay(Collider other)
    {
        if(/*AcceptFingerDistance(distance, fingerDistance, erreurDistance) ||*/ AcceptHandPosition(_hand))
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
        }
       
    }

    void OnTriggerExit(Collider other)
    {
        if(/*AcceptFingerDistance(distance, fingerDistance, erreurDistance) ||*/ AcceptHandPosition(_hand))
        {
            transform.position = GameObject.Find("FoodWashPlace").transform.position;

            Debug.Log("Trigger exit");

            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
        
    }
}
}
