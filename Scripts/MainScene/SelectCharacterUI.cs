using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacterUI : MonoBehaviour
{
    [SerializeField]
    Scrollbar _SelectCharacterScrollUI;

    [SerializeField]
    float _ScrollSpeed;

    [SerializeField]
    GameObject CharacterSlot;

    [SerializeField]
    Transform _Contents;

    [SerializeField]
    PlayerStatus[] _CharacterStatus;


    // Start is called before the first frame update
    void Start()
    {
        UpdateCharacterList();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            // ÈÙ¾÷
            if (wheelInput > 0 && _SelectCharacterScrollUI.value <= 0)
            {
                _SelectCharacterScrollUI.value += _ScrollSpeed;

            }
            // ÈÙ´Ù¿î
            else if (wheelInput < 0 && _SelectCharacterScrollUI.value >=0)
            {
                _SelectCharacterScrollUI.value -= _ScrollSpeed;

            }
        }
    }

    void UpdateCharacterList()
    {
        foreach(var tStatus in _CharacterStatus)
        {
            GameObject tCharacterSlotGO = Instantiate<GameObject>(CharacterSlot, _Contents);
            CharacterSlot tCharacterSlot = tCharacterSlotGO.GetComponent<CharacterSlot>();
            tCharacterSlot._PlayerStatus = tStatus;
            tCharacterSlot.UpdateSlot();

        }
    }

    public void OnClickCloseBtn()
    {
        gameObject.SetActive(false);
    }

}
