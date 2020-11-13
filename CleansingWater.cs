using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace An3sMod
{
    class CleansingWater : PlayerItem
    {
        public static void Init()
        {
            //The name of the item
            string itemName = "Cleansing Water";

            //Refers to an embedded png in the project. 
            string resourceName = "An3sMod/Resources/CleansingWater";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<CleansingWater>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Removes curse when used";
            string longDesc = "taken from the water of a cleansing shrine\n\n" +
                "This water is extremely powerful and can remove large amounts of curse... but like the shrine, it has limits.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

            item.consumable = true;

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
        }

        //removes curse depending on how much they have, there is probably a better way to do this but idk how
        protected override void DoEffect(PlayerController user)
        {
            float Curse = user.stats.GetStatValue(PlayerStats.StatType.Curse);
           if (CanBeUsed(user) && Curse <= 9)
            {
                StatModifier statModifier = new StatModifier();
                user.ownerlessStatModifiers.Add(statModifier);
                statModifier.statToBoost = PlayerStats.StatType.Curse;
                statModifier.amount = (Curse * -1);
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                user.stats.RecalculateStats(user, false, false);
                AkSoundEngine.PostEvent("Play_UI_secret_reveal_01", base.gameObject);
            }
        }

        private void RemoveStat(PlayerStats.StatType statType)
        {
            var newModifiers = new List<StatModifier>();
            for (int i = 0; i < passiveStatModifiers.Length; i++)
            {
                if (passiveStatModifiers[i].statToBoost != statType)
                    newModifiers.Add(passiveStatModifiers[i]);
            }
            this.passiveStatModifiers = newModifiers.ToArray();
        }
     
        //checks if the active item is able to be used.
        public override bool CanBeUsed(PlayerController user)
        {
            float Curse = user.stats.GetStatValue(PlayerStats.StatType.Curse);

            return Curse > 0 && Curse < 10;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
        }
    }
}
