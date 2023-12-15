using System;
using UnityEngine;

namespace jmayberry.CardDeck {
    [Serializable]
    public class CardAction<Action, Target> where Action : Enum where Target : Enum {
        [SerializeField] internal Action effectType;
        [SerializeField] internal Target effectTarget;
        [SerializeField] internal float effectMagnitude;
    }
}
