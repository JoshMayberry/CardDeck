using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using jmayberry.Spawner;
using jmayberry.CustomAttributes;
using AYellowpaper.SerializedCollections;

namespace jmayberry.CardDeck {
	public class CardManager<Action, Target> : MonoBehaviour where Action : Enum where Target : Enum {

		[Header("Sprites")]
		[Required] public Sprite spriteBack;
		[SerializedDictionary("Rarity", "Sprite")] public SerializedDictionary<CardRarityType, Sprite> spriteRarity;
		[SerializedDictionary("Holo", "Sprite")] public SerializedDictionary<CardHoloType, Sprite> spriteHolo;
		[SerializedDictionary("After Use", "Sprite")]  public SerializedDictionary<CardAfterUseType, Sprite> spriteAfterUse;
		[SerializedDictionary("Action", "Sprite")]  public SerializedDictionary<Action, Sprite> spriteAction;
		[SerializedDictionary("Target", "Sprite")]  public SerializedDictionary<Target, Sprite> spriteTarget;

		public Sprite spriteRarityDefault;
		public Sprite spriteHoloDefault;
		public Sprite spriteAfterUseDefault;
		public Sprite spriteActionDefault;
		public Sprite spriteTargetDefault;

		[Header("Piles")]
		public int cardsPerRound = 3;
		[Required] public Deck<Action, Target> currentDeck;
		[Required] public IGameContext<Action, Target> currentContext;
		[Required] public PileDraw<Action, Target> pileDraw;
		[Required] public PileHand<Action, Target> pileHand;
		[Required] public PileDiscard<Action, Target> pileDiscard;
		[Required] public PileDestroy<Action, Target> pileDestroy;

		[Required] public Card<Action, Target> uiCardPrefab;
		internal UnitySpawner<Card<Action, Target>> uiCardSpawner { get; private set; }

		[Header("Events")]
		public UnityEvent<Deck<Action, Target>> onOutOfCards = new UnityEvent<Deck<Action, Target>>();
		public UnityEvent<Deck<Action, Target>> onHandFull = new UnityEvent<Deck<Action, Target>>();

        public static CardManager<Action, Target> instance { get; private set; }

		public virtual void Awake() {
			if (instance != null && instance != this) {
				Debug.LogError("More than 1 'CardManager<Action, Target> found in scene");
				Destroy(gameObject);
				return;
			}

			instance = this;

			this.uiCardSpawner = new UnitySpawner<Card<Action, Target>>(this.uiCardPrefab);
		}

		public virtual void SetContext(IGameContext<Action, Target> context) {
			this.currentContext = context;
		}

		public virtual void SetDeck(Deck<Action, Target> deck) {
			this.currentDeck = deck;
		}

		public virtual void OnNewGame() {
			if (this.currentDeck == null) {
				Debug.LogError("No Deck Set");
				return;
			}

			this.currentDeck.InitializeDrawPile();
		}

		public virtual void OnDrawCards() {
			for (int i = 0; i < this.cardsPerRound; i++) {
				var card = this.currentDeck.DrawCard();

				if (card == null) {
					if (this.pileHand.IsFull()) {
						Debug.Log("Hand Full");
						this.onHandFull.Invoke(this.currentDeck);
						return;
                    }
					Debug.Log("Out of cards");
					this.onOutOfCards.Invoke(this.currentDeck);
					return;
				}
			}
		}

		public virtual void OnDiscardHand() {
			this.pileHand.MoveToPile(this.pileDiscard);
		}

		public virtual void OnPlayCard(Card<Action, Target> card) {
			card.PlayCard(this.currentContext);
		}

		public virtual void OnPlayCard(Card<Action, Target> card, IGameContext<Action, Target> context) {
			card.PlayCard(context);
		}
	}
}