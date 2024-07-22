using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FadeController : MonoBehaviour
{
    [SerializeField]
    GameObject _FadeContolPanel;

    [SerializeField]
    Image _PanelImage;

    [SerializeField]
    float _FadeTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOut()
    {
        StartCoroutine(CoFadeOut());
    }

    public void FadeOutWithChangeScene(string tSceneName)
    {
        StartCoroutine(CoFadeOutWithChangeScene(tSceneName));
    }

    IEnumerator CoFadeOut()
    {
        _PanelImage.color = new Color(_PanelImage.color.r, _PanelImage.color.g, _PanelImage.color.b, 0);

        _FadeContolPanel.SetActive(true);

        while(_PanelImage.color.a <= 1)
        {
            _PanelImage.color += new Color(0, 0, 0, Time.deltaTime / _FadeTime);


            yield return null;
        }        
    }

    IEnumerator CoFadeOutWithChangeScene(string tSceneName)
    {
        _PanelImage.color = new Color(_PanelImage.color.r, _PanelImage.color.g, _PanelImage.color.b, 0);

        _FadeContolPanel.SetActive(true);

        while (_PanelImage.color.a <= 1)
        {
            _PanelImage.color += new Color(0, 0, 0, Time.deltaTime / _FadeTime);


            yield return null;
        }

        SceneManager.LoadScene(tSceneName);
    }

    public void FadeIn()
    {
        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        _PanelImage.color = new Color(_PanelImage.color.r, _PanelImage.color.g, _PanelImage.color.b, 1);

        _FadeContolPanel.SetActive(true);

        while (_PanelImage.color.a >= 0)
        {
            _PanelImage.color -= new Color(0, 0, 0, Time.deltaTime / _FadeTime);

            yield return null;
        }

        _FadeContolPanel.SetActive(false);
    }
}
