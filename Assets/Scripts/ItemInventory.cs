using UnityEngine;
using System;
using System.Collections;

[ExecuteInEditMode]
public class ItemInventory : MonoBehaviour {

	public float coolDownTime = 1.0f;

	[SerializeField]
	protected string itemName = "";

	private bool needRefresh = true;

	public string Spell
	{
		get { return this.itemName; }
		set
		{
			this.itemName = value;
			refresh();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate()
	{
		if( needRefresh )
		{
			needRefresh = false;
			refresh();
		}
	}
	
	public void OnResolutionChanged()
	{
		needRefresh = true;
	}

	void OnEnable()
	{
		
		refresh();
		
		var control = gameObject.GetComponent<dfPanel>();
		control.SizeChanged += delegate( dfControl source, Vector2 value )
		{
			// Queue the refresh to be processed in LateUpdate after the
			// control and its child controls have recalculated their 
			// new render size
			needRefresh = true;
		};
		
	}

	public void OnBuyItem() {
		StartCoroutine (showCooldown ());
	}

	private IEnumerator showCooldown()
	{
		var sprite = GetComponentInChildren<dfControl> ().Find ("CoolDown") as dfSprite;
		sprite.IsVisible = true;

		var startTime = Time.realtimeSinceStartup;
		var endTime = startTime + coolDownTime;

		while (Time.realtimeSinceStartup < endTime) {
			var elapsed = Time.realtimeSinceStartup - startTime;
			var lerp = 1.0f - elapsed / coolDownTime;

			sprite.FillAmount = lerp;

			yield return null;
		}

		sprite.FillAmount = 1.0f;
		sprite.IsVisible = false;
	}

	private void refresh()
	{
		var control = gameObject.GetComponent<dfPanel> ();
		var container = control.Parent as dfScrollPanel;

		if (container != null) {
			control.Width = container.Width - container.ScrollPadding.horizontal;
		}

		var icon = control.Find<dfSprite> ("icon");
		var lblName = control.Find<dfLabel> ("lblName");
		var lblDescription = control.Find<dfLabel> ("lblDescription");
		var lblCost = control.Find<dfLabel> ("lblCost");

		if( icon == null ) throw new Exception( "Not found: icon" );
		if( lblCost == null ) throw new Exception( "Not found: lblCosts" );
		if( lblName == null ) throw new Exception( "Not found: lblName" );
		if( lblDescription == null ) throw new Exception( "Not found: lblDescription" );

		var assignedItem = ItemDefinition.FindByName (this.Spell);
		if (assignedItem == null) {
			icon.SpriteName = "";
			lblName.Text = "";
			lblDescription.Text = "";
			lblCost.Text = "";
			return;
		} else {
			icon.SpriteName = assignedItem.Icon;
			lblName.Text = assignedItem.Name;
			lblDescription.Text = assignedItem.Description;
			lblCost.Text = string.Format( "{0}개/{1}초/{2}초", assignedItem.Cost, assignedItem.Recharge, assignedItem.Delay );
		}

		// Resize this control to match the size of the contents
		var descriptionHeight = lblDescription.RelativePosition.y + lblDescription.Size.y;
		var costsHeight = lblCost.RelativePosition.y + lblCost.Size.y;
		control.Height = Mathf.Max( descriptionHeight, costsHeight ) + 5;
	}
}
