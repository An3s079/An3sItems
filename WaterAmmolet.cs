using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace An3sMod
{
    class WaterAmmolet : BlankModificationItem
	{
		public static void Init()
		{
			

			//The name of the item
			string itemName = "Water Ammolet";

			//Refers to an embedded png in the project. 
			string resourceName = "An3sMod/Resources/WaterAmmolet";

			//Create new GameObject
			GameObject obj = new GameObject(itemName);

			//Add a PassiveItem component to the object
			var item = obj.AddComponent<WaterAmmolet>();

			//Adds a sprite component to the object and adds your texture to the item sprite collection
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

			//Ammonomicon entry variables
			string shortDesc = "Why would you ever use this?";
			string longDesc = "Creates a pool of water upon using a blank.\n\n This ammolet was brought to the gungeon by a deep ocean dweller";

			//Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
			//Do this after ItemBuilder.AddSpriteToObject!
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

			//Set the rarity of the item
			item.quality = PickupObject.ItemQuality.D;
		}



		public override void Pickup(PlayerController player)
		{
			//initiates InitHooks
			WaterAmmolet.InitHooks();
			base.Pickup(player);
			Tools.Print($"Player picked up {this.DisplayName}");
		}
		public override DebrisObject Drop(PlayerController player)
		{
			Tools.Print($"Player dropped {this.DisplayName}");
			return base.Drop(player);
		}
		public static void InitHooks()
		{
			Hook hook = new Hook(
				typeof(SilencerInstance).GetMethod("ProcessBlankModificationItemAdditionalEffects", BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(WaterAmmolet).GetMethod("BlankModificationHook")
			);
		}

		public static void BlankModificationHook(Action<SilencerInstance, BlankModificationItem, Vector2, PlayerController> orig, SilencerInstance self, BlankModificationItem bmi, Vector2 centerPoint, PlayerController user)
		{
			orig(self, bmi, centerPoint, user);
			if (bmi is WaterAmmolet)
			{
				(bmi as WaterAmmolet).OnBlank(centerPoint, user);
			}
		}

		protected virtual void OnBlank(Vector2 centerPoint, PlayerController user)
		{
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/water goop.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
			goopManagerForGoopType.TimedAddGoopCircle(base.Owner.sprite.WorldCenter, 20f, 0.35f, false);
		}
	}
}
