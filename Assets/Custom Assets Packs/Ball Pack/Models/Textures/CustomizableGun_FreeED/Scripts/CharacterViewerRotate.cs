using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewerRotate : MonoBehaviour
{
    public Vector2 clickPos;
    public Vector2 offsetPos;
    public float divider = 80;

    void Start()
    {
        clickPos = Vector2.zero;
        offsetPos = Vector2.zero;
    }

    void Update()
    {
		offsetPos = Vector2.zero;

		if (Input.GetKeyDown(leftClick()))
		{
			clickPos = mouseXY();
		}

		if (Input.GetKey(leftClick()))
		{
			offsetPos = clickPos - mouseXY();
		}


		transform.Rotate(new Vector3(-(offsetPos.y / divider), offsetPos.x / divider, 0f), Space.World);
	}

	KeyCode leftClick()
	{
		return KeyCode.Mouse0;
	}

	//Immediate location of the mouse
	Vector2 mouseXY()
	{
		return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}

	//Immediate location of the mouse's X coordinate
	float mouseX()
	{
		return Input.mousePosition.x;
	}

	//Immediate location of the mouse's Y coordinate
	float mouseY()
	{
		return Input.mousePosition.y;
	}
}
