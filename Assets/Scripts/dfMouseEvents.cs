using UnityEngine;
using System.Collections;

public class dfMouseEvents : MonoBehaviour {

	public bool isMouseDown = false;
	public bool isMouseClick = false;
	
	public void OnMouseDown( dfControl control, dfMouseEventArgs mouseEvent )
	{
		isMouseDown = true;
		isMouseClick = false;
	}
	
	public void OnMouseUp( dfControl control, dfMouseEventArgs mouseEvent )
	{
		isMouseDown = false;
		isMouseClick = true;
	}
}
