using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsRenderer : MonoBehaviour
{
    [SerializeField]
    float _RotateSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            transform.Rotate(Vector3.up, _RotateSpeed);
        }
        
    }
}
