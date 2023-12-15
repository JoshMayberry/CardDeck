using System;

namespace jmayberry.CardDeck {
    public interface IGameContext<Action, Target> where Action : Enum where Target : Enum {
        void ApplyEffect(Action effectType, Target effectTarget, float effectMagnitude);
    }
}