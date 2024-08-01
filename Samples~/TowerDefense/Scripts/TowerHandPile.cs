using UnityEngine;
using UnityEngine.UI;

using jmayberry.CardDeck;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class TowerHandPile : PileHand<TowerAction, TowerTarget> { }
