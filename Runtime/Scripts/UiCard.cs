using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using jmayberry.Spawner;
using jmayberry.CustomAttributes;
using System.Collections.Generic;

namespace jmayberry.CardDeck {
	public class Card<Action, Target> : MonoBehaviour, ISpawnable where Action : Enum where Target : Enum {
		public CardState currentState = CardState.Unknown; // TODO: Make readonly during playmode only
		[Required] [SerializeField] public CardData<Action, Target> card;
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

		public void SetCard(CardData<Action, Target> card) {
			this.card = card;
		}


		public virtual void PlayCard() {
			this.PlayCard(CardManager<Action, Target>.instance.currentContext);
		}

		public virtual void PlayCard(IGameContext<Action, Target> context) {
			foreach (CardAction<Action, Target> action in this.card.actionList) {
				context.ApplyEffect(action.effectType, action.effectTarget, action.effectMagnitude);
			}

			switch (this.card.afterUse) {
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
			return CardManager<Action, Target>.instance.pileDiscard.MoveToPile(this);
		}

		public virtual bool GoToDestroy() {
			return CardManager<Action, Target>.instance.pileDestroy.MoveToPile(this);
		}

		public virtual bool GoToHand() {
			return CardManager<Action, Target>.instance.pileHand.MoveToPile(this);
		}

		public virtual bool GoToDraw() {
			return CardManager<Action, Target>.instance.pileDraw.MoveToPile(this);
		}

		public void UpdateImages() {
			if (this.card == null) {
				Debug.LogError("No card set yet");
				return;
			}

			switch (this.currentState) {
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

			var cardManager = CardManager<Action, Target>.instance;
			this.afterUseImage.sprite = cardManager.spriteAfterUse.GetValueOrDefault(this.card.afterUse, cardManager.spriteAfterUseDefault);
			this.rarityImage.sprite = cardManager.spriteRarity.GetValueOrDefault(this.card.rarity, cardManager.spriteRarityDefault);
			this.holoImage.sprite = cardManager.spriteHolo.GetValueOrDefault(this.card.holo, cardManager.spriteHoloDefault);
			this.artworkImage.sprite = this.card.artwork;

			this.titleText.text = this.card.title;
			this.costText.text = $"{this.card.cost}";
			this.descriptionText.text = this.card.description;

			if (this.card.actionList.Count <= 0) {
				this.actionImage.sprite = cardManager.spriteActionDefault;
				this.targetImage.sprite = cardManager.spriteTargetDefault;
			} else {
				this.actionImage.sprite = cardManager.spriteAction.GetValueOrDefault(this.card.actionList[0].effectType, cardManager.spriteActionDefault);
				this.targetImage.sprite = cardManager.spriteTarget.GetValueOrDefault(this.card.actionList[0].effectTarget, cardManager.spriteTargetDefault);
			}

			this.afterUseImage.gameObject.SetActive(this.afterUseImage.sprite != null);
			this.rarityImage.gameObject.SetActive(this.rarityImage.sprite != null);
			this.artworkImage.gameObject.SetActive(this.artworkImage.sprite != null);
			this.holoImage.gameObject.SetActive(this.holoImage.sprite != null);
			this.actionImage.gameObject.SetActive(this.actionImage.sprite != null);
			this.targetImage.gameObject.SetActive(this.targetImage.sprite != null);
			this.titleText.gameObject.SetActive(this.titleText.text != "");
			this.costText.gameObject.SetActive(this.costText.text != "");
			this.descriptionText.gameObject.SetActive(this.descriptionText.text != "");
		}

		public void OnSpawn(object spawner) {}

		public void OnDespawn(object spawner) {
			// Move inactive card out of the pile
			this.transform.SetParent(CardManager<Action, Target>.instance.transform);
        }
	}
}
