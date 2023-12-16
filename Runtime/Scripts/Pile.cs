using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jmayberry.CardDeck {
	public class Pile<Action, Target> : MonoBehaviour, IEnumerable where Action : Enum where Target : Enum {
		public int maxCards;

		[SerializeField] protected Vector3 stackChaosPositionMargin = new Vector3(0, 5, 5);
        [SerializeField] protected Vector3 stackChaosRotationMargin = new Vector3(0, 0, 10);

		public virtual bool MoveToPile(Card<Action, Target> card) {
			if (this.IsFull()) {
				return false;
			}

			card.gameObject.transform.SetParent(this.transform);
			card.UpdateImages();

			return true;
		}

		public virtual bool IsFull() {
			return (this.maxCards != 0) && (this.transform.childCount >= maxCards);

        }

		public virtual void MoveToPile(Pile<Action, Target> pileDestination, bool shuffle=false) {
			List<Card<Action, Target>> childList = new List<Card<Action, Target>>();
			foreach (Card<Action, Target> card in this) {
				childList.Add(card);
			}

			foreach (Card<Action, Target> card in childList) {
				pileDestination.MoveToPile(card);
			}

			if (shuffle) {
				pileDestination.Shuffle();
			}
		}

		public void Start() {
			// Clean up piles
			for (int i = this.transform.childCount - 1; i >= 0; i--) {
				Destroy(this.transform.GetChild(i).gameObject);
			}
		}

		public int Count() {
			return this.transform.childCount;
		}

		// TODO: Maybe we cache the pile contents?
		public Card<Action, Target> GetCard(int index=0) {
			if (index >= this.transform.childCount) {
				return null;
			}

			Transform cardTransform = this.transform.GetChild(index);
			if (cardTransform == null) {
				return null;
			}

			return cardTransform.gameObject.GetComponent<Card<Action, Target>>();
		}

		// See: https://forum.unity.com/threads/swapping-the-gameobjects-children-in-a-random-way.650128/#post-7686241
		public virtual void Shuffle() {
			foreach (Transform cardTransform in this.transform) {
				cardTransform.SetSiblingIndex(UnityEngine.Random.Range(0, this.transform.childCount));
			}
		}

		protected virtual Vector3 GetStackChaosPosition(float x, float y, float z) {
			if (this.stackChaosPositionMargin == Vector3.zero) {
				return new Vector3(x, y, z);
			}

			return new Vector3(
				x + UnityEngine.Random.Range(-this.stackChaosPositionMargin.x, this.stackChaosPositionMargin.x),
				y + UnityEngine.Random.Range(-this.stackChaosPositionMargin.y, this.stackChaosPositionMargin.y),
				z + UnityEngine.Random.Range(-this.stackChaosPositionMargin.z, this.stackChaosPositionMargin.z)
			);
		}

		protected virtual Quaternion GetStackChaosRotation(float x, float y, float z) {
			if (this.stackChaosRotationMargin == Vector3.zero) {
				return Quaternion.Euler(x, y, z);
			}

			return Quaternion.Euler(
				x + UnityEngine.Random.Range(-this.stackChaosRotationMargin.x, this.stackChaosRotationMargin.x),
				y + UnityEngine.Random.Range(-this.stackChaosRotationMargin.y, this.stackChaosRotationMargin.y),
				z + UnityEngine.Random.Range(-this.stackChaosRotationMargin.z, this.stackChaosRotationMargin.z)
			);
		}

		public IEnumerator<Card<Action, Target>> GetEnumerator() {
			foreach (Transform cardTransform in this.transform) {
				Card<Action, Target> card = cardTransform.gameObject.GetComponent<Card<Action, Target>>();
				yield return card;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
