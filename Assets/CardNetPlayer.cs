using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardNetPlayer : MonoBehaviourPun
{
    public static List<CardNetPlayer> NetPlayers = new List<CardNetPlayer>(2);
    private CardPlayer cardPlayer;
    private Card[] cards;

    public void Set(CardPlayer player)
    {
        cardPlayer = player;
        cards = player.GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            //var button = card.GetComponent<button>();
            //button.OnClick.AddListener(()=>RemoveClickButton(card.AttackValue));
        }
    }

    private void OnDestroy()
    {
        foreach (var card in cards)
        {
            //var button = card.GetComponent<button>();
            //button.OnClick.RemoveListener(()=>RemoveClickButton(card.AttackValue));
        }
    }

    private void RemoveClickButton(Attack value)
    {
        if(photonView.IsMine)
        {
            photonView.RPC("RemoteClickButtonRPC",RpcTarget.Others,(int)value);
        }
    }

    [PunRPC]
    private void RemoveClickButtonRPC(int value)
    {
        foreach (var card in cards)
        {
            if(card.AttackValue == (Attack)value)
            {
                //var button = card.GetComponent<button>();
                //break;
            }
        }
    }

    private void OnEnable() 
    {
        NetPlayers.Add(this);
    }

    private void OnDisable() {
        NetPlayers.Remove(this);
    }
}
