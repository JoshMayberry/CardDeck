using UnityEngine;
using UnityEngine.UI;

using jmayberry.CardDeck;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class TowerDrawPile : PileDraw<TowerAction, TowerTarget> { }
