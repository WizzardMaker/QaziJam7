using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the size of a canvas to the size of the camera
/// 
/// This class needs to be in a canvas, which is a child of the main Canvas
/// 
/// This class is optional
/// </summary>
[ExecuteInEditMode]
public class CanvasResize : MonoBehaviour {
	
	void Update() {
		//Get the camera reference
		var main = Camera.main;

		//Get the main Canvas scale if you want to use the Canvas Scaler
		float yScale = transform.GetComponentInParent<RectTransform>().lossyScale.y;
		float xScale = transform.GetComponentInParent<RectTransform>().lossyScale.x;

		//Get the left/right/top/bottom values for the offset
		float left = main.rect.x * Screen.width;
		float right = -left;

		float bottom = main.rect.y * Screen.height;
		float top = -bottom;
		
		//Set the offset of the Canvas
		((RectTransform)transform).offsetMin = new Vector2(left / xScale, bottom / yScale);
		((RectTransform)transform).offsetMax = new Vector2(right / xScale, top / yScale);
	}
}
