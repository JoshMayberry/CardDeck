using System;
using System.Collections.Generic;
using UnityEngine;

using jmayberry.CustomAttributes;
using UnityEngine.Events;

namespace jmayberry.CardDeck {
	public enum CardRarityType {
		Common,
		Uncommon,
		Rare,
		UltraRare,
		Unique,
	}

	public enum CardHoloType {
		Normal,
		Holo,
		Reverse,
	}

	public enum CardState {
		Unknown,
		InDraw,
		InHand,
		InDiscard,
		Destroyed,
	}
	public enum CardAfterUseType {
		GoToDiscard,
		GoToDraw,
		GoToHand,
		GoToDestroy,
	}

	public abstract class CardData<Action, Target> : ScriptableObject where Action : Enum where Target : Enum {
		[SerializeField] public string title;
		[SerializeField] public string description;
		[SerializeField] public CardRarityType rarity;
		[SerializeField] public CardHoloType holo;
		[SerializeField] public CardAfterUseType afterUse;
		[SerializeField] public int cost;
		[SerializeField] public Sprite artwork;
		[SerializeField] public List<CardAction<Action, Target>> actionList;
	}
}
