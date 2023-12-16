using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.CardDeck;

public class TowerCardManager : CardManager<TowerAction, TowerTarget> {
    public void Start() {
        // Clean up piles
        this.pileDraw.ClearPile();
        this.pileHand.ClearPile();
        this.pileDiscard.ClearPile();
        this.pileDestroy.ClearPile();
    }
}
