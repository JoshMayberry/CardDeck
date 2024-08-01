using UnityEngine;
using UnityEngine.UI;

using jmayberry.CardDeck;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class TowerDiscardPile : PileDiscard<TowerAction, TowerTarget> { }
