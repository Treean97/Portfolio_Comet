using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    GameObject _PChar = null;

    [SerializeField]
    Vector3 _Offset = Vector3.zero;

    [SerializeField]
    float _AngleLimit = 0;

    [SerializeField]
    GameSceneMgr _GameSceneMgr;

    float _MouseXSens;
    float _MouseYSens;

    float _CameraLerp = 0.2f;

    // ĳ���ͷκ��� ī�޶� ������ �Ÿ�, ����
    [SerializeField]
    float _ArmLength = 0;

    [SerializeField]
    float _ArmHeight = 0;

    bool _CanRotateCamera = false;

    float _XRotate = 0; 
    float _YRotate = 0;

    [SerializeField]
    int _NotCullingMask;

    // Start is called before the first frame update
    void Start()
    {
        _PChar = GameObject.FindGameObjectWithTag("Player");

        // ī�޶� �� ���� ����
        _Offset = new Vector3(0f, _ArmHeight, -1f * _ArmLength);

        _CanRotateCamera = true;

        // ���콺 ���� �޾ƿ���
        _MouseXSens = _GameSceneMgr.GetMouseXSens;
        _MouseYSens = _GameSceneMgr.GetMouseYSens;

        _NotCullingMask = (-1) - (1 << LayerMask.NameToLayer("DamageFontUI"));

        GetComponent<Camera>().cullingMask = _NotCullingMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (_CanRotateCamera)
        {
            CameraMove();
        }


    }

    private void LateUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, _PChar.transform.position + this.transform.rotation * _Offset, _CameraLerp);
                
    }

    void CameraMove()
    {
        // Yȸ���� = �¿�� ������ ���콺�� �̵��� * �ӵ�
        float tYRotateSize = Input.GetAxis("Mouse X") * _MouseYSens;

        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        _YRotate = transform.eulerAngles.y + tYRotateSize;

        // Xȸ���� = ���Ʒ��� ������ ���콺�� �̵��� * �ӵ�
        float xRotateSize = Input.GetAxis("Mouse Y") * _MouseXSens * (-1);

        // ���� x�� ȸ������ ���� ���ο� ȸ������ ���, ���� ����
        _XRotate = Mathf.Clamp(_XRotate + xRotateSize, -_AngleLimit, _AngleLimit);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(_XRotate, _YRotate, 0);
    }

    public void LockCamera()
    {
        // ���콺 ȸ�� ����
        _MouseXSens = 0;
        _MouseYSens = 0;
    }

    public void UnlockCamera()
    {
        // ���콺 ���� �ٽ� �޾ƿ���
        _MouseXSens = _GameSceneMgr.GetMouseXSens;
        _MouseYSens = _GameSceneMgr.GetMouseYSens;
    }

}
