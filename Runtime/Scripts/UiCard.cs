using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using jmayberry.CustomAttributes;

namespace jmayberry.CardDeck {
    [RequireComponent(typeof(Image))]
    public class UiCard<Action, Target> : MonoBehaviour where Action : Enum where Target : Enum {
        [SerializeField] private Card<Action, Target> card;

        private Image image;

        private void Awake() {
            this.image = GetComponent<Image>();
        }

        public void SetCard(Card<Action, Target> card) {


            this.card = card;
        }

        public void UpdateImage() {
            if (this.card == null) {
                Debug.LogError("No card set yet");
                return;
            }

            switch (this.card.currentState) {
                case CardState.Unknown:
                case CardState.InDraw:
                    //this.image.sprite = this.backSprite;
                    break;

                case CardState.InHand:
                case CardState.InDiscard:
                    //this.image.sprite = this.frontSprite;
                    break;

                case CardState.Destroyed:
                    this.image.sprite = null;
                    break;
            }
        }
    }
}
