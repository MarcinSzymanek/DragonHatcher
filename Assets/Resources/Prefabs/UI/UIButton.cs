using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIButton : MonoBehaviour
{
	public class ButtonClickArgs{
		public ButtonClickArgs(int id){
			Id = id;
		}
		public int Id{get;}
	}
	
	public int Id;
	private Button button;
	public event EventHandler<ButtonClickArgs> clicked;
	void Awake()
	{
		button = GetComponent<Button>();
	    button.onClick.AddListener(onClick);
    }

	void onClick(){
		clicked?.Invoke(this, new ButtonClickArgs(Id));
	}
}
