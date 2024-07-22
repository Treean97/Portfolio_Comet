using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField]
    GameSceneMgr _GameSceneMgr;

    [SerializeField]
    Player _Player;

    [SerializeField]
    MainCamera _MainCamera;

    // 아이템UI
    public GameObject _ItemUIGO;
    ItemUI _ItemUI;

    // 선택형 아이템 선택 UI
    [SerializeField]
    GameObject _SelectItemScrollGO;

    // 선택형 아이템 Text
    public TMP_Text _SelectItemText;

    // 아이템 구매UI
    public GameObject _ItemBuyUIGO;
    TMP_Text _ItemBuyUIText;

    [SerializeField]
    TMP_Text _MoneyText;

    [SerializeField]
    TMP_Text _WarningText;

    [SerializeField]
    int _WarningTime;

    // 중복 불가능한 UI
    [SerializeField]
    List<GameObject> CantDupUI = new List<GameObject>();

    // 보스 생성 UI
    [SerializeField]
    public GameObject _BossSpawnUIGO;

    // 보스 UI
    [SerializeField]
    GameObject _BossUIGO;

    // 일시정지UI
    [SerializeField]
    GameObject _PauseUIGO;

    // 엔딩 UI
    [SerializeField]
    GameObject _EndingUIGO;

    [SerializeField]
    FadeController _FadeController;

    Coroutine _WarningTextCoroutine;

    // UI 상태(UI 중복, 일시정지 때 상호작용 방지)
    public enum UIState
    {
        Running,
        NotRunning
    }

    public UIState _UIState;

    // Start is called before the first frame update
    void Start()
    {
        _FadeController.FadeIn();

        _UIState = UIState.NotRunning;
        _Player = FindObjectOfType<Player>();
        _ItemUI = _ItemUIGO.GetComponent<ItemUI>();
        _ItemBuyUIText = _ItemBuyUIGO.GetComponentInChildren<TMP_Text>();


        // 초기 UI Off
        HideBossSpawnUI();
        HideBossUI();
        HideBuySpawnerUI();
        HideItemUI();
        HideSelectItemText();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PauseUIOnOff();
        }
    }

    void PauseUIOnOff()
    {
        // 켤 때 다른 UI가 켜져있는 상태인지 확인
        if (_PauseUIGO.activeSelf == false && _UIState == UIState.NotRunning)
        {
            // Pause
            Time.timeScale = 0;

            _PauseUIGO.SetActive(true);

            _MainCamera.LockCamera();
            _GameSceneMgr.ShowCursor();
            _Player._GunState = Player.GunState.CanNotFire;
            _UIState = UIState.Running;
        }
        else
        {            
            // Continue
            Time.timeScale = 1;

            _PauseUIGO.SetActive(false);

            _MainCamera.UnlockCamera();
            _GameSceneMgr.HideCursor();
            _Player._GunState = Player.GunState.CanFire;
            _UIState = UIState.NotRunning;

        }
    }

    public void SelectItemUIOnOff()
    {        
        // 켤 때 다른 UI가 켜져있는 상태인지 확인
        if (_SelectItemScrollGO.activeSelf == false && _UIState == UIState.NotRunning)
        {
            // Pause
            Time.timeScale = 0;

            // 아이템 선택 상자 설명 UI Off
            _SelectItemText.gameObject.SetActive(false);

            // 아이템 선택 상자 UI On
            _SelectItemScrollGO.SetActive(true);
            _GameSceneMgr.ShowCursor();
            _MainCamera.LockCamera();
            _Player._GunState = Player.GunState.CanNotFire;
            _UIState = UIState.Running;
        }
        else
        {
            // Continue
            Time.timeScale = 1;

            _SelectItemScrollGO.SetActive(false);
            _GameSceneMgr.HideCursor();
            _MainCamera.UnlockCamera();
            _Player._GunState = Player.GunState.CanFire;
            _UIState = UIState.NotRunning;

        }
    }

    // 중복 불가능한 UI 들 모두 비활성화
    public void HideCantDupUIWithout(GameObject tUIGO)
    {
        foreach(var t in CantDupUI)
        {
            t.SetActive(false);
        }

        // 활성화 할 UI 만 true
        tUIGO.SetActive(true);
    }

        
    public void ShowBossUI()
    {
        _BossUIGO.SetActive(true);
    }

    public void HideBossUI()
    {
        _BossUIGO.SetActive(false);
    }

    public void HideItemUI()
    {
        _ItemUIGO.SetActive(false);
    }

    public void HideBossSpawnUI()
    {
        _BossSpawnUIGO.SetActive(false);
    }

    public void HideSelectItemText()
    {
        _SelectItemText.gameObject.SetActive(false);
    }

    public void UpdateItemUI(ItemStatus tItemStatus)
    {
        _ItemUI.UpdateItemUI(tItemStatus);
    }

    public void UpdateBuyItemUI(int tNeedMoney)
    {
        StringBuilder tSB = new StringBuilder();
        tSB.AppendLine("Press \"E\" For Buy.");
        tSB.Append("Need: ");
        tSB.Append(tNeedMoney);

        _ItemBuyUIText.text = tSB.ToString();
    }

    public void ShowBuyItemUI()
    {
        _ItemBuyUIGO.SetActive(true);
    }

    public void HideBuySpawnerUI()
    {
        _ItemBuyUIGO.SetActive(false);
    }

    public void UpdateMoneyUI(int tValue)
    {
        _MoneyText.text = tValue.ToString();
    }

    public void WarningTextUI(string tWarningString)
    {
        if (_WarningTextCoroutine != null)
        {
            StopCoroutine(_WarningTextCoroutine);
        }
        
        _WarningText.text = tWarningString;
        _WarningTextCoroutine = StartCoroutine(WarningText());
    }


    IEnumerator WarningText()
    {
        _WarningText.gameObject.SetActive(true);     

        int tCount = 0;

        while (tCount < _WarningTime)
        {
            yield return new WaitForSecondsRealtime(1);
            tCount++;
        }

        _WarningText.gameObject.SetActive(false);
    }

    public void EndingUI(string tEndingType)
    {
        // 정지
        Time.timeScale = 0;

        _EndingUIGO.SetActive(true);
        _EndingUIGO.GetComponent<EndingUI>().UpdateResultText(tEndingType);
        _GameSceneMgr.ShowCursor();
        _MainCamera.LockCamera();
        _Player._GunState = Player.GunState.CanNotFire;
        _UIState = UIState.Running;
    }

}
