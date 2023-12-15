using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using jmayberry.CustomAttributes;
using AYellowpaper.SerializedCollections;

namespace jmayberry.CardDeck {
    public class CardManager<Action, Target> : MonoBehaviour where Action : Enum where Target : Enum {
        [Required] public Deck<Action, Target> currentDeck;
        public int cardsPerRound = 3;
        public UnityEvent<Deck<Action, Target>> onOutOfCards = new UnityEvent<Deck<Action, Target>>();

        [Header("Sprites")]
        [Required] public Sprite spriteBack;
        [Required] public UiCard<Action, Target> uiCardPrefab;
        [SerializedDictionary("Rarity", "Sprite")] public SerializedDictionary<CardRarityType, Sprite> spriteRarity;
        [SerializedDictionary("Holo", "Sprite")] public SerializedDictionary<CardHoloType, Sprite> spriteHolo;
        [SerializedDictionary("After Use", "Sprite")]  public SerializedDictionary<CardAfterUseType, Sprite> spriteAfterUse;
        [SerializedDictionary("Action", "Sprite")]  public SerializedDictionary<Action, Sprite> spriteAction;
        [SerializedDictionary("Target", "Sprite")]  public SerializedDictionary<Target, Sprite> spriteTarget;

        void UpdateCardDisplay(Card<Action, Target>[] cards) {
            // TODO: Update the GUI with the given cards
            // TODO: Implement the drag and drop logic in the GUI
        }

        public void OnTriggerCardsOver() {
            Card<Action, Target>[] cardsToDisplay = new Card<Action, Target>[cardsPerRound];
            for (int i = 0; i < cardsPerRound; i++) {
                var card = currentDeck.DrawCard();

                if (card == null) {
                    this.onOutOfCards.Invoke(this.currentDeck);
                    return;
                }

                cardsToDisplay[i] = card;
            }

            UpdateCardDisplay(cardsToDisplay);
        }

        // TODO: Call this from the GUI when a card is applied
        public void OnApplyCard(Card<Action, Target> card, IGameContext<Action, Target> context) {
            card.PlayCard(context);
        }
    }
}