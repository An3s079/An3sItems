using System;
using System.Collections;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
namespace An3sMod.Resources
{
    class portableVampire : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Portable Vampire";

            //Refers to an embedded png in the project. 
            string resourceName = "An3sMod/Resources/portableVampire";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<portableVampire>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "gives money for health.";
            string longDesc = "this creature is part of the vampire species, for some reson this one is small enough to fit in your pocket.";
           
            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 70);

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2);

            item.consumable = false;

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.A;
        }
       
        //removes half a heart and gives the player 15 shells
        protected override void DoEffect(PlayerController user)
        {
            if (CanBeUsed(user))
            {
                user.healthHaver.ApplyHealing(-.5f);
                user.carriedConsumables.Currency += 15;
                StartCoroutine(PlaySound());
            }
        }
       

        //plays a sound, duh
        IEnumerator PlaySound()
        {
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
            yield return new WaitForSeconds(0.1f);
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
            yield return new WaitForSeconds(0.1f);
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
            yield return new WaitForSeconds(0.1f);
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
            yield return new WaitForSeconds(0.1f);
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
            yield return new WaitForSeconds(0.1f);
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
            yield return new WaitForSeconds(0.1f);
            AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject); AkSoundEngine.PostEvent("Play_WPN_ak47_shells_01", base.gameObject);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return user.healthHaver.GetCurrentHealth() > .5f;
        }

    }
}
