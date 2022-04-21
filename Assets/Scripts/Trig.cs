using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Leap.Unity.Interaction
{

public class Trig : MonoBehaviour
{

    
    public LeapServiceProvider LeapServiceProvider;
    private Vector pos_thumb;
    private Vector pos_index;
    private GameObject testSphere_index;
    private GameObject testSphere_thumb;

    private GameObject Middel;

    public float fingerDistance;
    public float erreurDistance;
        
    private float distance;
    public float speed_door_open;
    
    private bool door_open = false;
    public static bool right_door_open = false;
    public static bool left_door_open = false;

    public float maxGrabAngle;

    private GameObject camera;

    private Hand _hand;
    // Start is called before the first frame update
    void Start()
    {
        testSphere_index =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSphere_index.transform.localScale = new Vector3(0.01f, 0.01f,0.01f);
        

        testSphere_thumb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        testSphere_thumb.transform.localScale = new Vector3(0.01f, 0.01f,0.01f);
        

        Middel = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Middel.transform.localScale = new Vector3(0.01f, 0.01f,0.01f);
        Middel.name = "middel";
        
        Middel.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if( door_open)
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);

            
            if(transform.parent.name == "pivot1" && _hand.IsRight)
            {
                right_door_open = true;
                transform.parent.Rotate(0,-90 * speed_door_open * Time.deltaTime,0);
            
                if(transform.parent.rotation.y <= -0.6)
                {
                    door_open=false;
                    Debug.Log("opennnn end ");
                }
            }else if(transform.parent.name == "pivot2" && _hand.IsLeft)
            {   
                left_door_open = true;
                transform.parent.Rotate(0,90 * speed_door_open * Time.deltaTime,0);
                if(transform.parent.rotation.y >= 0.6)
                {   
                    door_open=false;
                    Debug.Log("opennnn end ");
                }
            }


            MoveCamera();

        }
    }


    void UpdateHand(Vector3 pos_index, Vector3 pos_thumb)
    {
        testSphere_index.transform.position = pos_index;
        testSphere_thumb.transform.position = pos_thumb;
        Middel.transform.position = Middle_of_two_points(pos_index, pos_thumb);
        
        distance = Distance_between_two_points(pos_thumb,pos_index);
        //Debug.Log("distance : "+distance);
        float activate = Distance_between_two_points(Middle_of_two_points(pos_index, pos_thumb), transform.position);

        //Debug.Log(activate);
    }
    // function that calculate the distance between 2 point in 3D space
    public float Distance_between_two_points(Vector3 v1, Vector3 v2)
    {       
            float deltaX = v1.x - v2.x;
            float deltaY = v1.y - v2.y;
            float deltaZ = v1.z - v2.z;
            float distance = (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

            return distance ;

    }
    public float Distance_between_point_and_sphere(Vector3 v1, Vector3 v2)
    {       
            float deltaX = v1.x - v2.x;
            float deltaY = v1.y - v2.y;
            float deltaZ = v1.z - v2.z;
            float distance = (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

            return distance ;

    }
        public float Distance_between_point_and_bar(Vector3 v1, Vector3 v2)
    {       
            float deltaX = v1.x - v2.x;
            float deltaY = v1.y - v2.y;
            float deltaZ = v1.z - v2.z;
            float distance = (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);

            return distance ;

    }
    public Vector3 Middle_of_two_points(Vector3 v1, Vector3 v2)
    {
            float deltaX = (v1.x + v2.x)/2;
            float deltaY = (v1.y + v2.y)/2;
            float deltaZ = (v1.z + v2.z)/2;
            Vector3 Middle = new Vector3(deltaX, deltaY, deltaZ);
            
            return Middle;
    }
    bool AcceptFingerDistance(float distance, float fingerDistance, float erreurDistance)
    {
        if(distance <= fingerDistance - erreurDistance)return true;
        return false;
    }

    bool AcceptHandPosition(Hand _hand)
    {
        //Debug.Log(_hand.GrabAngle);
        //Debug.Log(_hand.Rotation);
        if((_hand.Rotation.x>0.3) && (_hand.Rotation.x<0.7) && (_hand.GrabAngle>maxGrabAngle))
        {
            return true;
        }else
        {
            return false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(AcceptFingerDistance(distance, fingerDistance, erreurDistance) || AcceptHandPosition(_hand))
        {
            Debug.Log("ouvrire");
            door_open = true;
        }
    }

    void MoveCamera()
    {

        if(right_door_open && left_door_open)
        {
            camera = GameObject.Find("Main Camera");
            // camera.transform.position = new Vector3(4.08400011f,0.819999993f,3.2650001f);
            // camera.transform.rotation = new Quaternion(51.1155319f,87.9286575f,358.526276f,1);
            camera.transform.Translate(0,-0.0006f,0.001f);
            //camera.transform.Rotate(0.0001f,0,0);

            Debug.Log("move camera in ");
        }
    }
     
}

}