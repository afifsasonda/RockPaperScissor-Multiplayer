using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class CardGameManager : MonoBehaviour
{
    public GameObject netPlayerPrefab;
    public CardPlayer P1;
    public CardPlayer P2;
    public GameState State= GameState.ChooseAttack;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;

    private CardPlayer damagedPlayer;
    private CardPlayer winner;
    public enum GameState
    {
        ChooseAttack,
        Attacks,
        Damages,
        Draw,
        GameOver,
    }

    private void Start() {
        gameOverPanel.SetActive(false);
        PhotonNetwork.Instantiate(netPlayerPrefab.name,Vector3.zero,Quaternion.identity);
        
    }

    private void Update() {
        switch (State)
        {
            case GameState.ChooseAttack:
                if(P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.IsClickable(false);
                    P2.IsClickable(false);
                    State = GameState.Attacks;
                }
                break;

            case GameState.Attacks:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    damagedPlayer = GetDamagedPlayer();
                    if(damagedPlayer != null)
                    {
                        damagedPlayer.AnimateDamage();
                        State = GameState.Damages;
                    }
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        State = GameState.Draw;
                    }
                }
                break;

            case GameState.Damages:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    //kalkulasi health
                    if(damagedPlayer == P1)
                    {
                        P1.ChangeHealth(-10);
                        P2.ChangeHealth(5);
                    }
                    else
                    {
                        P1.ChangeHealth(5);
                        P2.ChangeHealth(-10);
                    }

                    var winner = GetWinner();

                    if(winner == null)
                    {
                        P1.IsClickable(true);
                        P2.IsClickable(true);
                        ResetPlayers();
                        State = GameState.ChooseAttack;
                    }
                    else
                    {
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner ==P1 ? "CardPlayer 1 wins" : "CardPlayer 2 wins";  
                        ResetPlayers();
                        State = GameState.GameOver;
                    }
                }
                break;
                
            case GameState.Draw:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    ResetPlayers();
                    P1.IsClickable(true);
                    P2.IsClickable(true);
                    State = GameState.ChooseAttack;
                }
                break;
        }

    }

    private void ResetPlayers()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }

    private CardPlayer GetDamagedPlayer()
    {
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper)
        {
            return P1;
        }
        else if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissor)
        {
            return P2;
        }
        else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock)
        {
            return P2;
        }
        else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissor)
        {
            return P1;
        }
        else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Rock)
        {
            return P1;
        }
        else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Paper)
        {
            return P2;
        }

        return null;
    }

    private CardPlayer GetWinner()
    {
        if(P1.Health == 0)
        {
            return P2;
        }
        else if(P2.Health == 0)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }
    
}
