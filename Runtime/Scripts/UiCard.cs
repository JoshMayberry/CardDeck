using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using jmayberry.CustomAttributes;

namespace jmayberry.CardDeck {
    public class UiCard<Action, Target> : MonoBehaviour where Action : Enum where Target : Enum {
        [Required] [SerializeField] private Card<Action, Target> card;
        [Required] [SerializeField] private Image afterUseImage;
        [Required] [SerializeField] private Image rarityImage;
        [Required] [SerializeField] private Image artworkImage;
        [Required] [SerializeField] private Image holoImage;
        [Required] [SerializeField] private Image actionImage;
        [Required] [SerializeField] private Image targetImage;
        [Required] [SerializeField] private Image destroyedImage;
        [Required] [SerializeField] private Image backImage;
        [Required] [SerializeField] private TMP_Text titleText;
        [Required] [SerializeField] private TMP_Text descriptionText;
        [Required] [SerializeField] private TMP_Text costText;
        [Required] [SerializeField] private CardManager<Action, Target> cardManager;

        public void SetCard(Card<Action, Target> card, bool autoUpdate = true) {
            this.card = card;

            if (autoUpdate) {
                this.UpdateImages();
            }
        }

        public void SetCardManager(CardManager<Action, Target> cardManager) {
            this.cardManager = cardManager;
        }

        public void UpdateImages() {
            if (this.card == null) {
                Debug.LogError("No card set yet");
                return;
            }

            switch (this.card.currentState) {
                case CardState.Unknown:
                case CardState.InDraw:
                    this.afterUseImage.gameObject.SetActive(false);
                    this.rarityImage.gameObject.SetActive(false);
                    this.artworkImage.gameObject.SetActive(false);
                    this.holoImage.gameObject.SetActive(false);
                    this.actionImage.gameObject.SetActive(false);
                    this.targetImage.gameObject.SetActive(false);
                    this.titleText.gameObject.SetActive(false);
                    this.descriptionText.gameObject.SetActive(false);
                    this.costText.gameObject.SetActive(false);
                    this.destroyedImage.gameObject.SetActive(false);
                    this.backImage.gameObject.SetActive(true);
                    return;

                case CardState.Destroyed:
                    this.afterUseImage.gameObject.SetActive(true);
                    this.rarityImage.gameObject.SetActive(true);
                    this.artworkImage.gameObject.SetActive(true);
                    this.holoImage.gameObject.SetActive(true);
                    this.actionImage.gameObject.SetActive(true);
                    this.targetImage.gameObject.SetActive(true);
                    this.titleText.gameObject.SetActive(true);
                    this.descriptionText.gameObject.SetActive(true);
                    this.costText.gameObject.SetActive(true);
                    this.destroyedImage.gameObject.SetActive(true);
                    this.backImage.gameObject.SetActive(false);
                    return;

                case CardState.InHand:
                case CardState.InDiscard:
                    this.titleText.gameObject.SetActive(true);
                    this.descriptionText.gameObject.SetActive(true);
                    this.costText.gameObject.SetActive(true);
                    this.destroyedImage.gameObject.SetActive(false);
                    this.backImage.gameObject.SetActive(false);
                    break;
            }

            this.afterUseImage.sprite = this.cardManager.spriteAfterUse[this.card.afterUse];
            this.rarityImage.sprite = this.cardManager.spriteRarity[this.card.rarity];
            this.holoImage.sprite = this.cardManager.spriteHolo[this.card.holo];
            this.artworkImage.sprite = this.card.artwork;

            this.titleText.text = this.card.title;
            this.costText.text = $"{this.card.cost}";
            this.descriptionText.text = this.card.description;

            if (this.card.actionList.Count <= 0) {
                this.actionImage.sprite = null;
                this.targetImage.sprite = null;
            } else {
                this.actionImage.sprite = this.cardManager.spriteAction[this.card.actionList[0].effectType];
                this.targetImage.sprite = this.cardManager.spriteTarget[this.card.actionList[0].effectTarget];
            }

            this.afterUseImage.gameObject.SetActive(this.afterUseImage.sprite != null);
            this.rarityImage.gameObject.SetActive(this.rarityImage.sprite != null);
            this.artworkImage.gameObject.SetActive(this.artworkImage.sprite != null);
            this.holoImage.gameObject.SetActive(this.holoImage.sprite != null);
            this.actionImage.gameObject.SetActive(this.actionImage.sprite != null);
            this.targetImage.gameObject.SetActive(this.targetImage.sprite != null);
        }
    }
}
