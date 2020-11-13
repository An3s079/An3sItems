using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using Dungeonator;
using System.Collections;
using System.Reflection;

namespace An3sMod
{
    class LichBullets : PassiveItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Lich Bullets";

            //Refers to an embedded png in the project.
            string resourceName = "An3sMod/Resources/LichBullet";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<LichBullets>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Gives the strength of The Lich";
            string longDesc = "increases damage to bosses";
               

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

            //Adds the actual passive effect to the item
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.DamageToBosses, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 3);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.S;

           
        }
        public void PostProcessProjectile(Projectile proj, float f)
        {

            // makes it so all projectiles shot ignore the boss damage cap
            proj.ignoreDamageCaps = true;

           

        }
        public override void Pickup(PlayerController player)
        {   //makes it so PostProcessProjectile is initialized upon pickup
            player.PostProcessProjectile += this.PostProcessProjectile;
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
        }


        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            // makes it so that PostProcessProjectile is no longer initialized upon dropping LichBullets
            player.PostProcessProjectile -= this.PostProcessProjectile;
            Tools.Print($"Player dropped {this.DisplayName}");
            return base.Drop(player);
        }
       
    }
}
