using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;
namespace An3sMod
{
    class LeatherJacket : PassiveItem
    {
        private static FleePlayerData fleeData;
        public static void Init()
        {
            //The name of the item
            string itemName = "Leather Jacket";

            //Refers to an embedded png in the project. 
            string resourceName = "An3sMod/Resources/jacket";

            //Create new GameObject
            GameObject obj = new GameObject(itemName);

            //Add a PassiveItem component to the object
            var item = obj.AddComponent<LeatherJacket>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "wow, so cool";
            string longDesc = "Leather jackets have always been a sign of coolness\n" +
                "you still arent very cool though.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

           
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 3);

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.D;
        }
        protected override void Update()
        {
            
            base.Update();
            if (base.Owner.HasPassiveItem(290))
            {
                RoomHandler currentRoom = base.Owner.CurrentRoom;
                foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    bool flag3 = aiactor.behaviorSpeculator != null;
                    if (flag3)
                    {
                        aiactor.behaviorSpeculator.FleePlayerData = LeatherJacket.fleeData;
                        FleePlayerData fleePlayerData = new FleePlayerData();
                        GameManager.Instance.StartCoroutine(LeatherJacket.RemoveFear(aiactor));
                    }

                }
            }
        }
        
        private static IEnumerator RemoveFear(AIActor aiactor)
        {

            yield return new WaitForSeconds(3f);
            aiactor.behaviorSpeculator.FleePlayerData = null;
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            
                bool flag = LeatherJacket.fleeData == null || LeatherJacket.fleeData.Player != player;
                if (flag)
                {
                    LeatherJacket.fleeData = new FleePlayerData();
                    LeatherJacket.fleeData.Player = player;
                    LeatherJacket.fleeData.StartDistance = 6f;
                }
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
