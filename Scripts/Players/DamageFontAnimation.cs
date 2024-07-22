using System.Collections;
using TMPro;
using UnityEngine;


public class DamageFontAnimation : MonoBehaviour
{
    Vector3 _Dir;
    float _Dis = 2;
    Vector3 _TargetPos;

    TMP_Text _Text;

    float _FadeTime = 1f;

    float _NormalFontSize = 1f;
    float _MaxFontSize = 2f;

    GameObject _Player;


    // Start is called before the first frame update
    private void OnEnable()
    {        
        ResetSetting();
        
        StartCoroutine(FadeOut());
        StartCoroutine(ScaleUp());        
    }

    private void Awake()
    {
        _Text = GetComponent<TMP_Text>();
        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_Dir * _Dis * Time.deltaTime);        
        this.transform.forward = _Player.transform.forward;
    }

    public void SetTargetPos(Vector3 tTargetPos)
    {
        _TargetPos = tTargetPos;
    }

    void ResetSetting()
    {
        transform.position = _TargetPos;
        float tX = Random.Range(-1f, 1f);
        float tY = Random.Range(0, 1f);
        float tZ = Random.Range(-0.5f, 0.5f);
        _Dir = new Vector3(tX, tY, tZ).normalized;
    }

    // 페이드 아웃
    IEnumerator FadeOut()
    {
        _Text.color = new Color(_Text.color.r, _Text.color.g, _Text.color.b, 1);

        while (_Text.color.a >= 0)
        {
            _Text.color -= new Color(0, 0, 0, Time.deltaTime / _FadeTime);

            yield return null;
        }

        TurnOff();
    }

    // 커지게
    IEnumerator ScaleUp()
    {
        _Text.fontSize = _NormalFontSize;

        while(_Text.fontSize <= _MaxFontSize)
        {
            _Text.fontSize += Time.deltaTime;
            
            yield return null;
        }

    }

    void TurnOff()
    {
        ObjectPool._Inst.ReturnObject(this.gameObject);

    }
}
