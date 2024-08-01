using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jmayberry.CardDeck {

	// TODO: Look at a curved layout instead (https://gist.github.com/baba-s/cf7df94aa8be8412b25246b57aaef175)

	public class PileHand<Action, Target> : Pile<Action, Target> where Action : Enum where Target : Enum {
		public override bool MoveToPile(Card<Action, Target> uiCard) {
			var previousState = uiCard.currentState;
			uiCard.currentState = CardState.InHand;
			if (!base.MoveToPile(uiCard)) {
				uiCard.currentState = previousState;
				return false;
			}

			uiCard.gameObject.transform.localPosition = this.GetStackChaosPosition(0, 0, 0);
			uiCard.gameObject.transform.localRotation = this.GetStackChaosRotation(0, 0, 0);
			uiCard.gameObject.transform.localScale = this.initialCardScale;
            return true;
		}
	}
}
