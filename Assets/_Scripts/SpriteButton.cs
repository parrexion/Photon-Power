using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteButton : MonoBehaviour, IPointerClickHandler {

	private System.Action leftClick;


	public void Setup(System.Action onLeftClick) {
		leftClick = onLeftClick;
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Left) {
			leftClick?.Invoke();
		}
	}
}
