using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    private Vector3 playerRestartPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerRestartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("r")) {
           print("reseting....");
           transform.position = playerRestartPosition;
       }
    }
}
