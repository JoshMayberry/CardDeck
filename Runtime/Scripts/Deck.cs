using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using jmayberry.Spawner;
using jmayberry.CustomAttributes;
using UnityEngine.Events;

namespace jmayberry.CardDeck {
	public enum DrawEmptyType {
		ShuffleDiscard,
		GameOver,
	}

	public abstract class Deck<Action, Target> : ScriptableObject, IEnumerable where Action : Enum where Target : Enum {
		[SerializeField] private string title;
		[SerializeField] private DrawEmptyType whenDrawEmpty;
        [SerializeField] protected Vector3 initialCardScale = new Vector3(1, 1, 1);

        [SerializeField] private List<CardData<Action, Target>> cardList = new List<CardData<Action, Target>>();

		public UnityEvent<Card<Action, Target>> onCardUse = new UnityEvent<Card<Action, Target>>();
		public UnityEvent<Card<Action, Target>> onCardDraw = new UnityEvent<Card<Action, Target>>();
		public UnityEvent<Card<Action, Target>> onCardDiscard = new UnityEvent<Card<Action, Target>>();
		public UnityEvent<Card<Action, Target>> onCardHand = new UnityEvent<Card<Action, Target>>();
		public UnityEvent<Card<Action, Target>> onCardDestroy = new UnityEvent<Card<Action, Target>>();

		public virtual void InitializeDrawPile() {
			var cardManager = CardManager<Action, Target>.instance;

			cardManager.uiCardSpawner.DespawnAll();
			foreach (CardData<Action, Target> card in this.cardList) {
				if (card == null) {
					this.cardList.Remove(card); // Get rid of empty spots
					continue;
				}
				Card<Action, Target> uiCard = cardManager.uiCardSpawner.Spawn(Vector3.zero, cardManager.gameObject.transform);
				uiCard.gameObject.transform.localScale = this.initialCardScale;
                uiCard.currentState = CardState.Unknown;
				uiCard.SetCard(card);
				uiCard.GoToDraw();
			}

			cardManager.pileDraw.Shuffle();
		}

		public IEnumerator<Card<Action, Target>> GetEnumerator() {
			foreach (Card<Action, Target> card in CardManager<Action, Target>.instance.uiCardSpawner) {
				yield return card;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}


		public Card<Action, Target> DrawCard() {
			var cardManager = CardManager<Action, Target>.instance;

			Card<Action, Target> card = cardManager.pileDraw.GetCard();
			if (card == null) {
				switch (this.whenDrawEmpty) {
					case DrawEmptyType.ShuffleDiscard:
						cardManager.pileDiscard.MoveToPile(cardManager.pileDraw, true);
						if (cardManager.pileDraw.Count() == 0) {
							return null;
						}

						card = cardManager.pileDraw.GetCard();
						if (card == null) {
							Debug.LogError("This error should never happen");
							return null;
						}
						break;

					case DrawEmptyType.GameOver:
						return null;
				}
			}

			if (!card.GoToHand()) {
				return null;
			}
			return card;
		}
	}
}