using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.CardDeck;

public class BasicTower : MonoBehaviour, IGameContext<TowerAction, TowerTarget> {
    int attackPower;
    float attackRate;
    float attackRange;

    public void ApplyEffect(TowerAction effectType, TowerTarget effectTarget, float effectMagnitude) {
        switch (effectType) {
            case TowerAction.ModifyAttackPower:
                this.attackPower += (int)effectMagnitude;
                break;

            case TowerAction.ModifyAttackRate:
                this.attackRate += effectMagnitude;
                break;

            case TowerAction.ModifyAttackRange:
                this.attackRange += effectMagnitude;
                break;
        }
    }
}
