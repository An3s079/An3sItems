using System;
using System.Collections;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
namespace An3sMod
{
    class OldWarStealthkit : PassiveItem
    {
        
        public static void Init() 
        {
        //The name of the item
        string itemName = "Old War Stealthkit";

        //Refers to an embedded png in the project. 
        string resourceName = "An3sMod/Resources/OldWarStealthKit";

        //Create new GameObject
        GameObject obj = new GameObject(itemName);

        //Add a PassiveItem component to the object
        var item = obj.AddComponent<OldWarStealthkit>();

        //Adds a sprite component to the object and adds your texture to the item sprite collection
        ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "hide in the shadows";
        string longDesc = "chance to become stealthy after taking damage\n\n" +
            "This item was brought to the gungeon from another world. Its seems to fit in. ";

        //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
        //Do this after ItemBuilder.AddSpriteToObject!
        ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");


            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }
        private bool isStealth;
        public void OnReceivedDamage(PlayerController player)
        {
            if(UnityEngine.Random.value <= 0.4f && isStealth == false)
            {
                isStealth = true;
                AlterItemStats.AddStatToPassive(this, PlayerStats.StatType.MovementSpeed, 1.4f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                Owner.stats.RecalculateStats(Owner, false, false);
                this.HandleStealth(player);
           }
        }
        private void HandleStealth(PlayerController player)
        {
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
            player.ChangeSpecialShaderFlag(1, 1f);
            player.SetIsStealthed(true, "smoke");
            player.SetCapableOfStealing(true, "StealthItem", null);
            player.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            
            player.OnItemStolen += this.BreakStealthOnSteal;
            StartCoroutine(WaitaSec(player));
        }
        IEnumerator WaitaSec(PlayerController player)
        {
            
            yield return new WaitForSecondsRealtime(3);
            player.OnDidUnstealthyAction += this.BreakStealth;
        }
        
          public void BreakStealth(PlayerController player)
        {
            isStealth = false;
            player.OnDidUnstealthyAction -= this.BreakStealth;
            player.OnItemStolen -= this.BreakStealthOnSteal;
            player.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            player.ChangeSpecialShaderFlag(1, 0f);
            player.SetIsStealthed(false, "smoke");
            player.SetCapableOfStealing(false, "StealthItem", null);
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
            AlterItemStats.RemoveStatFromPassive(this, PlayerStats.StatType.MovementSpeed);
            Owner.stats.RecalculateStats(Owner, false, false);
        }
            private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
            {
              this.BreakStealth(arg1);
            }

        public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += this.OnReceivedDamage;
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
        }

        public override DebrisObject Drop(PlayerController player)
        {
            Tools.Print($"Player dropped {this.DisplayName}");
            return base.Drop(player);
        }
    }
}
