using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraRotate : MonoBehaviour
{
    [SerializeField]
    float _YRotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        LoadCameraRot();
    }

    // Update is called once per frame
    void Update()
    {
        CamRotate();
    }


    void CamRotate()
    {
        transform.Rotate(Vector3.up * _YRotateSpeed * Time.deltaTime);
    }

    public void SaveCameraRot()
    {
        GameManager._Inst._CameraRot = this.gameObject.transform.rotation;
    }

    public void LoadCameraRot()
    {
        this.gameObject.transform.rotation = GameManager._Inst._CameraRot;        
    }
}
