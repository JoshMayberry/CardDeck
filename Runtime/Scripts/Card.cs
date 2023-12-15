using System;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.CustomAttributes;
using UnityEngine.Events;

namespace jmayberry.CardDeck {
    public enum CardRarityType {
        Common,
        Uncommon,
        Rare,
        UltraRare,
        Unique,
    }

    public enum CardHoloType {
        Normal,
        Holo,
        Reverse,
    }

    public enum CardState {
        Unknown,
        InDraw,
        InHand,
        InDiscard,
        Destroyed,
    }
    public enum CardAfterUseType {
        GoToDiscard,
        GoToDraw,
        GoToHand,
        GoToDestroy,
    }

    public abstract class Card<Action, Target> : ScriptableObject where Action : Enum where Target : Enum {
        [SerializeField] public string title { get; private set; }
        [SerializeField] public string description { get; private set; }
        [SerializeField] public CardRarityType rarity { get; private set; }
        [SerializeField] public CardHoloType holo { get; private set; }
        [SerializeField] public CardAfterUseType afterUse { get; private set; }
        [SerializeField] public int cost { get; private set; }
        [SerializeField] public Sprite artwork { get; private set; }
        [SerializeField] public List<CardAction<Action, Target>> actionList { get; private set; }

        [Readonly] internal CardState currentState = CardState.Unknown;
        public UnityEvent<Card<Action, Target>> onCardDrawn = new UnityEvent<Card<Action, Target>>();
        internal UnityEvent<Card<Action, Target>> onCardDiscard = new UnityEvent<Card<Action, Target>>();
        internal UnityEvent<Card<Action, Target>> onCardHand = new UnityEvent<Card<Action, Target>>();
        internal UnityEvent<Card<Action, Target>> onCardDestroy = new UnityEvent<Card<Action, Target>>();
        internal UnityEvent<Card<Action, Target>> onCardUse = new UnityEvent<Card<Action, Target>>();

        public virtual void PlayCard(IGameContext<Action, Target> context) {
            foreach (CardAction<Action, Target> action in this.actionList) {
                context.ApplyEffect(action.effectType, action.effectTarget, action.effectMagnitude);
            }

            switch (this.afterUse) {
                case CardAfterUseType.GoToDiscard:
                    GoToDiscard();
                    break;

                case CardAfterUseType.GoToDraw:
                    GoToDraw();
                    break;

                case CardAfterUseType.GoToHand:
                    GoToHand();
                    break;

                case CardAfterUseType.GoToDestroy:
                    GoToDestroy();
                    break;
            }
        }

        public virtual bool GoToDiscard() {
            this.currentState = CardState.InDiscard;
            return true;
        }

        public virtual bool GoToDestroy() {
            this.currentState = CardState.Destroyed;
            return true;
        }

        public virtual bool GoToHand() {
            this.currentState = CardState.InHand;
            return true;
        }

        public virtual bool GoToDraw() {
            this.currentState = CardState.InDraw;
            return true;
        }
    }
}
