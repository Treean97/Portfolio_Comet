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

    // ������UI
    public GameObject _ItemUIGO;
    ItemUI _ItemUI;

    // ������ ������ ���� UI
    [SerializeField]
    GameObject _SelectItemScrollGO;

    // ������ ������ Text
    public TMP_Text _SelectItemText;

    // ������ ����UI
    public GameObject _ItemBuyUIGO;
    TMP_Text _ItemBuyUIText;

    [SerializeField]
    TMP_Text _MoneyText;

    [SerializeField]
    TMP_Text _WarningText;

    [SerializeField]
    int _WarningTime;

    // �ߺ� �Ұ����� UI
    [SerializeField]
    List<GameObject> CantDupUI = new List<GameObject>();

    // ���� ���� UI
    [SerializeField]
    public GameObject _BossSpawnUIGO;

    // ���� UI
    [SerializeField]
    GameObject _BossUIGO;

    // �Ͻ�����UI
    [SerializeField]
    GameObject _PauseUIGO;

    // ���� UI
    [SerializeField]
    GameObject _EndingUIGO;

    [SerializeField]
    FadeController _FadeController;

    Coroutine _WarningTextCoroutine;

    // UI ����(UI �ߺ�, �Ͻ����� �� ��ȣ�ۿ� ����)
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


        // �ʱ� UI Off
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
        // �� �� �ٸ� UI�� �����ִ� �������� Ȯ��
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
        // �� �� �ٸ� UI�� �����ִ� �������� Ȯ��
        if (_SelectItemScrollGO.activeSelf == false && _UIState == UIState.NotRunning)
        {
            // Pause
            Time.timeScale = 0;

            // ������ ���� ���� ���� UI Off
            _SelectItemText.gameObject.SetActive(false);

            // ������ ���� ���� UI On
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

    // �ߺ� �Ұ����� UI �� ��� ��Ȱ��ȭ
    public void HideCantDupUIWithout(GameObject tUIGO)
    {
        foreach(var t in CantDupUI)
        {
            t.SetActive(false);
        }

        // Ȱ��ȭ �� UI �� true
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
        // ����
        Time.timeScale = 0;

        _EndingUIGO.SetActive(true);
        _EndingUIGO.GetComponent<EndingUI>().UpdateResultText(tEndingType);
        _GameSceneMgr.ShowCursor();
        _MainCamera.LockCamera();
        _Player._GunState = Player.GunState.CanNotFire;
        _UIState = UIState.Running;
    }

}
