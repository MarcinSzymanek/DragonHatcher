using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils{
	
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
	public class TypeRestrictionAttribute : PropertyAttribute
	{
		public System.Type InheritsFromType;
		public bool HideTypeDropDown;
		public bool AllowProxy;

		public TypeRestrictionAttribute(System.Type inheritsFromType)
		{
			this.InheritsFromType = inheritsFromType;
		}

	}
}
