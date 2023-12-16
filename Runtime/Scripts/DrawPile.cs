using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jmayberry.CardDeck {
	[RequireComponent(typeof(VerticalLayoutGroup))]
	public class PileDraw<Action, Target> : Pile<Action, Target> where Action : Enum where Target : Enum {
		public override bool MoveToPile(Card<Action, Target> uiCard) {
			var previousState = uiCard.currentState;
			uiCard.currentState = CardState.InDraw;
			if (!base.MoveToPile(uiCard)) {
				uiCard.currentState = previousState;
				return false;
			}

            uiCard.gameObject.transform.localPosition = this.GetStackChaosPosition(0, 0, 0);
			uiCard.gameObject.transform.localRotation = this.GetStackChaosRotation(110, 0, 0);
			return true;
		}
	}
}
