using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jmayberry.CardDeck {
	[RequireComponent(typeof(VerticalLayoutGroup))]
	public class PileDiscard<Action, Target> : Pile<Action, Target> where Action : Enum where Target : Enum {
		public override void MoveToPile(Card<Action, Target> uiCard) {
            uiCard.currentState = CardState.InDiscard;
			base.MoveToPile(uiCard);
            uiCard.gameObject.transform.localPosition = this.GetStackChaosPosition(0, 0, 0);
            uiCard.gameObject.transform.localRotation = this.GetStackChaosRotation(110, 0, 0);
        }
    }
}
