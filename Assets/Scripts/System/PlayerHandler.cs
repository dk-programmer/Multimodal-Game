using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PlayerHandler : MonoBehaviour
{
    //Reference
    public PlayerMovement PlayerMovement;
    //Coins
    private int _CollectedCoins;
    public int CollectedCoins { get {return _CollectedCoins; } set { _CollectedCoins = value; OnCollectedCoinChange?.Invoke(_CollectedCoins); } }
    //Events
    public Actions.OnChangeInt OnHealthChange;
    public Actions.OnAction OnGetHit;

    public Actions.OnChangeInt OnCollectedCoinChange;
    //
    private void Start()
    {
        OnGetHit += PlayerMovement.GetHit;
    }
    public void CollectCoin(int Amount)
    {
        CollectedCoins += Amount;
    }
}
