using System;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.CustomAttributes;

namespace jmayberry.CardDeck {
	public abstract class Deck<Action, Target> : ScriptableObject where Action : Enum where Target : Enum {
		[SerializeField] private string title;

		[SerializeField] private List<Card<Action, Target>> cardList = new List<Card<Action, Target>>();
		[Readonly] private List<int> drawPile = new List<int>();

		private void InitializeDrawPile() {
			foreach (Card<Action, Target> card in cardList) {
				card.currentState = CardState.Unknown;
			}

			this.Shuffle();
		}

		public Card<Action, Target> DrawCard() {
			if (drawPile.Count == 0) {
				if (!this.Shuffle()) {
					return null;
				}
			}

			int index = drawPile[drawPile.Count - 1];
			drawPile.RemoveAt(drawPile.Count - 1);
			cardList[index].GoToHand();
			return cardList[index];
		}

		public virtual bool Shuffle() {
			int i = 0;
			drawPile.Clear();
			foreach (Card<Action, Target> card in cardList) {
				switch (card.currentState) {
					case CardState.Unknown:
					case CardState.InDiscard:
						if (card.GoToDraw()) {
							drawPile.Add(i);
						}
						break;
				}
				i++;
			}

			if (drawPile.Count == 0) {
				return false;
			}

            // Shuffle the drawPile
            // Use: https://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619
            int n = drawPile.Count;
			while (n > 1) {
				n--;
				int k = UnityEngine.Random.Range(0, n + 1);
				int value = drawPile[k];
				drawPile[k] = drawPile[n];
				drawPile[n] = value;
			}

			return true;
		}
	}
}