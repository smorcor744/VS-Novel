using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    
    public void OpenDoors()
    {
        door1.transform.rotation = Quaternion.Euler(0, -90, 0); 
        door2.transform.rotation = Quaternion.Euler(0, -90, 0); 

    }
}


