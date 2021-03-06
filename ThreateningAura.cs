﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;
using System.Reflection;
using MonoMod.RuntimeDetour;


namespace An3sMod
{
    class ThreateningAura : PlayerItem
    {
        private static FleePlayerData fleeData;
        public static void Init()
        {
          
            //The name of the item
            string itemName = "Threatening Aura";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "An3sMod/Resources/ThreateningAura";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<ThreateningAura>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "Make them run.";
            string longDesc = "Strikes fear into those who come too close.\n\n" +
                "you usually cant get a threatening aura just from an item, but anything is possible in The Gungeon.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 27f);


            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.B;
        }

        //gives enemies the fear status effect 
        protected override void DoEffect(PlayerController user)
        {
           
            RoomHandler currentRoom = user.CurrentRoom;
            foreach (AIActor aiactor in currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                bool flag3 = aiactor.behaviorSpeculator != null;
                if (flag3)
                {
                    aiactor.behaviorSpeculator.FleePlayerData = ThreateningAura.fleeData;
                    FleePlayerData fleePlayerData = new FleePlayerData();
                    GameManager.Instance.StartCoroutine(ThreateningAura.RemoveFear(aiactor));
                }
            }
        }

//removes the fear status effect from enemies
private static IEnumerator RemoveFear(AIActor aiactor)
        {

            yield return new WaitForSeconds(6f);
            aiactor.behaviorSpeculator.FleePlayerData = null;
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            ThreateningAura.fleeData = new FleePlayerData();
            ThreateningAura.fleeData.Player = player;
            ThreateningAura.fleeData.StartDistance = 9f;
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
        }
       
      
   
    }
}
