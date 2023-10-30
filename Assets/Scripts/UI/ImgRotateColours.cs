using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgRotateColours : MonoBehaviour
{
	const float THIGH = 114/255f;
	const float TLOW = 76/255f;
	Image img_;
	enum RotateState{
		green,
		yellow,
		red,
		purple,
		blue,
		cyan
		
	};
	
	float r = TLOW;
	float g = THIGH;
	float b = TLOW;
	Color c;
	
	RotateState state_ = RotateState.green;
    void Awake()
	{
		c = new Color(r, g, b);
		img_ = GetComponent<Image>();
		setRgb();	
	}
    
	void setRgb(){
		c.r = r;
		c.g = g;
		c.b = b;
		img_.color = new Color(r, g, b);
	}
	
	void printColor(Color col){
		Debug.Log("rgb: " + col.r.ToString() + col.g.ToString() + col.b.ToString());
	}
	
	void Update()
	{
		switch(state_){
		case RotateState.green:
			r += 0.0002f;
			setRgb();
			if(r > THIGH){
				state_ = RotateState.yellow;
			}
			break;
		case RotateState.yellow:
			g -= 0.0002f;
			setRgb();
			if(g < TLOW){
				state_ = RotateState.red;
			}
			break;
		case RotateState.red:
			b += 0.0002f;
			setRgb();
			if(b > THIGH){
				state_ = RotateState.purple;
			}
			break;
		case RotateState.purple:
			r -= 0.0002f;
			setRgb();
			if(r < TLOW){
				state_ = RotateState.blue;
			}
			break;
		case RotateState.blue:
			g += 0.0002f;
			setRgb();
			if(g > THIGH){
				state_ = RotateState.cyan;
			}
			break;
		case RotateState.cyan:
			b -= 0.0002f;
			setRgb();
			if(b < TLOW){
				state_ = RotateState.green;
			}
			break;
		}
	}
}
