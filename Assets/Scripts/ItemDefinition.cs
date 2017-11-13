using UnityEngine;
using System.Linq;
using System.Collections;

public class ItemDefinition {

	#region Public fields 
	
	public string Name;
	public string Type;
	public int Cost;
	public string Icon;
	public float Recharge;
	public float Delay;
	public string Description;
	
	#endregion
	
	#region Static members
	
	public static ItemDefinition[] AllItems = new ItemDefinition[]
	{
		// Make up some ridiculous spells just to have something to work with :-)
		new ItemDefinition { Name = "생명잎사귀", Icon = "spell-5", Cost = 15, Delay = 0.25f, Recharge = 6f, Type = "Earth", Description = "[color #00FF00]30[/color]초동안 [color #00FF00]110%[/color]의 회복력이 적용된다" },
		new ItemDefinition { Name = "운석비", Icon = "spell-2", Cost = 5, Delay = 1f, Recharge = 4f, Type = "Fire", Description = "하늘에서 운석비가 내리면서 [color #00FF00]5[/color]초동안 그안의 모든것에 대하여 초당 [color #00FF00]3[/color]의 공격력을 발휘한다" },
		new ItemDefinition { Name = "불소나기", Icon = "spell-1", Cost = 15, Delay = 0.5f, Recharge = 4f, Type = "Fire", Description = "불소나기가 목표물에 대하여 [color #00FF00]25[/color]의 공격력을 발휘한다" },
		new ItemDefinition { Name = "보호광채", Icon = "spell-4", Cost = 10, Delay = 1f, Recharge = 10f, Type = "Spirit", Description = "모든 물리적공격들이 [color #00FF00]2[/color]초동안 [color #00ff00]15%[/color] 감소된다" },
		new ItemDefinition { Name = "조화광채", Icon = "spell-9", Cost = 25, Delay = 1f, Recharge = 10f, Type = "Spirit", Description = "다음번 두개의 마법이 [color #00ff00]15%[/color] 더 효과적으로 작용한다" },
		new ItemDefinition { Name = "집중공격", Icon = "spell-3", Cost = 5, Delay = 1f, Recharge = 6f, Type = "Spirit", Description = "칼을 가지고 진행하는 공격력이 [color #00FF00]5[/color]초동안 [color #00ff00]15%[/color] 더 커진다" },
		new ItemDefinition { Name = "불칼", Icon = "spell-7", Cost = 15, Delay = 1f, Recharge = 20f, Type = "Fire", Description = "칼을 가지고 진행하는 불공격이 [color #00FF00]5[/color]초동안 [color #ff0000]+5[/color]만큼 증가한다" },
		new ItemDefinition { Name = "돌풍", Icon = "spell-6", Cost = 15, Delay = 1f, Recharge = 10f, Type = "Air", Description = "주인공과 주변의 모든 동료들에 대한 불에 의한 공격력이 [color #00FF00]2[/color]초동안 [color #00FF00]10%[/color]감소된다" },
	};
	
	internal static ItemDefinition FindByName( string Name )
	{
		return AllItems.FirstOrDefault( item => item.Name == Name );
	}
	
	#endregion
}
