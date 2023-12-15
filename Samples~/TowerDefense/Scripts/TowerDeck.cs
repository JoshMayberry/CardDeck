using UnityEngine;

using jmayberry.CardDeck;

[CreateAssetMenu(fileName = "NewDeck", menuName = "CardGame/Deck")]
public class TowerDeck : Deck<TowerAction, TowerTarget> { }
