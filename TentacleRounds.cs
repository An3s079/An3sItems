using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using ItemAPI;
using System.Collections;
namespace An3sMod.Resources
{
    class TentacleRounds : BulletStatusEffectItem
    {
        
        public GameObject TelefragVFXPrefab;
       
        public static void Init()
        {  //The name of the item
            string itemName = "Tentacle Rounds";
          
            //Refers to an embedded png in the project.
            string resourceName = "An3sMod/Resources/TentacleRounds";

            //Create new GameObject
            GameObject obj = new GameObject();
            
            //Add a PassiveItem component to the object
            var item = obj.AddComponent<TentacleRounds>();

            //Adds a sprite component to the object and adds your texture to the item sprite collection
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //Ammonomicon entry variables
            string shortDesc = "monster from the depths";
            string longDesc = "Gives your bullets a small chance to summon an ancient crature from the depths.\n\n" +
                "Taken from a deep sea creture that came to the Gungeon, the poor soul of that gungoneer takes back its bullet when it finds itself in an enemy.";

            //Adds the item to the gungeon item list, the ammonomicon, the loot table, etc.
            //Do this after ItemBuilder.AddSpriteToObject!
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ans");

            //Set the rarity of the item
            item.quality = PickupObject.ItemQuality.B;
          

        }
       // holds all of the enemies that are already affected by the status effect
        public static List<AIActor> CurrentlyTentacledEnemies = new List<AIActor>() { };

       
        //makes the OnHitEnemy method play upon hitting an enamy
        public void PostProcessProjectile(Projectile proj, float f)
        {
          
            proj.OnHitEnemy += this.OnHitEnemy;

        }
        
        //is run when an enemy is hit with a beam weapon
        private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidBody, float tickrate)
        {
            
            float procChance = 0.12f;
            GameActor gameActor = hitRigidBody.gameActor;
            if (!gameActor)
            {
                return;
            }
            else if (!CurrentlyTentacledEnemies.Contains(hitRigidBody.aiActor))
            {
                if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
                {

                    EatCharmedEnemy(hitRigidBody.aiActor);
                   //adds the enemy that is affected by the status effect to the list
                    CurrentlyTentacledEnemies.Add(hitRigidBody.aiActor);
                }
            }

        }
        public void DelayedDestroyEnemy(AIActor enemy)
        {   //spawns the tentacles
            this.TelefragVFXPrefab = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Tentacleport");
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TelefragVFXPrefab);
            gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(enemy.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
            gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
            gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            if (enemy)
            {
                StartCoroutine(deleteEnemy(enemy));

            }
        }

        private void EatCharmedEnemy(AIActor a)
        {
            if (!a)
            {
                return;
            }
            if (a.behaviorSpeculator)
            {
                //stuns the hit enemy
                a.behaviorSpeculator.Stun(2f, true);
                //runs the waitForStun cooroutine
                StartCoroutine(WaitForStun(a));
            }
            if (a.knockbackDoer)
            {
                a.knockbackDoer.SetImmobile(true, "YellowChamberItem");
            }

        }
        

        IEnumerator WaitForStun(AIActor a)
        {
            yield return new WaitForSeconds(0.3f);
            DelayedDestroyEnemy(a);
        }

        private  void OnHitEnemy(Projectile proj, SpeculativeRigidbody enemy, bool fatal)
        {
          //checks if the hit enemy is on the list of already tentacled enemies and that the enemy is not a boss, and that the shot was not fatal
            if(!CurrentlyTentacledEnemies.Contains(enemy.aiActor) && !enemy.aiActor.healthHaver.IsBoss && !fatal)
            {
                if (UnityEngine.Random.value <= 0.1)
                {
                    
                    EatCharmedEnemy(enemy.aiActor);
                    CurrentlyTentacledEnemies.Add(enemy.aiActor);
                }
            }
           
        }

        IEnumerator deleteEnemy(AIActor enemy)
        {
            yield return new WaitForSeconds(1.3f);
            enemy.EraseFromExistenceWithRewards(false);
        }

      

       //gives the abyssal tentacle 5 ammo back upon killing an enemy
        public void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (fatal && Owner.CurrentGun.PickupObjectId == 474)
            {
                Owner.CurrentGun.GainAmmo(5);
            }
        }
      
        public override void Pickup(PlayerController player)
        {
            //initializes postProcessBeamTick upon pickup
            player.PostProcessBeamTick += this.PostProcessBeamTick;
            //does the same as above just for PostProcessProjectile
            player.PostProcessProjectile += this.PostProcessProjectile;
            //you know what this does :/
            player.OnAnyEnemyReceivedDamage += this.OnEnemyDamaged;
            base.Pickup(player);
            Tools.Print($"Player picked up {this.DisplayName}");
            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            Tools.Print($"Player dropped {this.DisplayName}");
            // de-inititializes all of these
            player.PostProcessBeamTick -= this.PostProcessBeamTick;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            return base.Drop(player);
        }

       
    }

   
}
