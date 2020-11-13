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
    public class SkeletonArmy : PlayerItem
    {
        public static void Init()
        {  // The name of the item
            string itemName = "Book of Necromancy";

            //Refers to an embedded png in the project. Make sure to embed your resources!
            string resourceName = "An3sMod/Resources/BookofNecromancy";

            //Create new GameObject
            GameObject obj = new GameObject();

            //Add a ActiveItem component to the object
            var item = obj.AddComponent<SkeletonArmy>();

            //Adds a tk2dSprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "summon the dead";
            string longDesc = "the book of an ancient ammomancer who was able to summon revolvenants.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //"example_pool" here is the item pool. In the console you'd type "give example_pool:sweating_bullets"
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

            //Set the cooldown type and duration of the cooldown
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 700f);

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2);

            //Set some other fields
            item.consumable = false;
            item.quality = PickupObject.ItemQuality.A;


        }
       



        public AIActor buddy1;
        public AIActor buddy2;
        public AIActor buddy3;
        protected override void DoEffect(PlayerController owner)
        {
            
                try
                {
                   
                    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("d5a7b95774cd41f080e517bea07bf495");
                    IntVector2? intVector = new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
                    bool flag = intVector != null;
                    if (flag)
                    {
                        
                        
                            AIActor one = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
                            one.IgnoreForRoomClear = true;
                            CompanionController companionComponent = one.gameObject.GetOrAddComponent<CompanionController>();
                            companionComponent.companionID = CompanionController.CompanionIdentifier.NONE;
                            companionComponent.Initialize(owner);
                            CompanionisedEnemyBulletModifiers companionisedBullets = one.gameObject.GetOrAddComponent<CompanionisedEnemyBulletModifiers>();
                            companionisedBullets.jammedDamageMultiplier = 2f;
                            companionisedBullets.TintBullets = true;
                            companionisedBullets.TintColor = Color.yellow;
                            companionisedBullets.baseBulletDamage = 3f;
                            one.gameObject.AddComponent<KillOnRoomClear>();

                        
                    }
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log(ex.Message, false);
                }
            



        }


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
        }
    }
}