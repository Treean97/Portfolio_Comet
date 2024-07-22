using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Object/PlayerStatus", order = int.MaxValue - 1)]
public class PlayerStatus : ScriptableObject
{
    [SerializeField]
    int PlayerId = 0;
    public int GetPlayerId { get { return PlayerId; } }

    [SerializeField]
    string PlayerName;
    public string GetPlayerName { get { return PlayerName; } }

    [SerializeField]
    string PlayerCharacterInfo;
    public string GetPlayerCharacterInfo { get { return PlayerCharacterInfo; } }

    [SerializeField]
    Sprite PlayerSprite;
    public Sprite GetPlayerSprite { get { return PlayerSprite; } }


    [SerializeField]
    float PlayerHP = 0;
    public float GetPlayerHP { get { return PlayerHP; } }

    [SerializeField]
    float PlayerSpeed = 0;
    public float GetPlayerSpeed { get { return PlayerSpeed; } }

    [SerializeField]
    float PlayerMaxJumpCount = 0;
    public float GetPlayerMaxJumpCount { get { return PlayerMaxJumpCount; } }

    [SerializeField]
    float PlayerJumpPower = 0;
    public float GetPlayerJumpPower { get { return PlayerJumpPower; } }

    [SerializeField]
    float PlayerCriChange = 0;
    public float GetPlayerCriChange { get { return PlayerCriChange; } }

    [SerializeField]
    float PlayerAttackDelay = 0;
    public float GetPlayerAttackDelay { get { return PlayerAttackDelay; } }


}
