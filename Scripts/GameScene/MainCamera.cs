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

    // 캐릭터로부터 카메라가 떨어진 거리, 길이
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

        // 카메라 암 길이 설정
        _Offset = new Vector3(0f, _ArmHeight, -1f * _ArmLength);

        _CanRotateCamera = true;

        // 마우스 감도 받아오기
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
        // Y회전량 = 좌우로 움직인 마우스의 이동량 * 속도
        float tYRotateSize = Input.GetAxis("Mouse X") * _MouseYSens;

        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        _YRotate = transform.eulerAngles.y + tYRotateSize;

        // X회전량 = 위아래로 움직인 마우스의 이동량 * 속도
        float xRotateSize = Input.GetAxis("Mouse Y") * _MouseXSens * (-1);

        // 현재 x축 회전값에 더한 새로운 회전각도 계산, 각도 제한
        _XRotate = Mathf.Clamp(_XRotate + xRotateSize, -_AngleLimit, _AngleLimit);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(_XRotate, _YRotate, 0);
    }

    public void LockCamera()
    {
        // 마우스 회전 방지
        _MouseXSens = 0;
        _MouseYSens = 0;
    }

    public void UnlockCamera()
    {
        // 마우스 감도 다시 받아오기
        _MouseXSens = _GameSceneMgr.GetMouseXSens;
        _MouseYSens = _GameSceneMgr.GetMouseYSens;
    }

}
