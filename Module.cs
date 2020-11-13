using An3sMod;
using An3sMod.Resources;
using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System.Reflection;
using An3sMod.GungeonAPI;

namespace ExampleMod
{
    public class Module : ETGModule
    {
        public static readonly string MOD_NAME = "An3sMod";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";
        
        public override void Start()
        {
            try
            {
                RandomGun.Add();
                //Whip.Add();
                ModdedNPC.Add();
                TheAngel.Add();
                LeatherJacket.Init();
                RubberBullets.Init();
                SkeletonArmy.Init();
                TentacleRounds.Init();
                ItemBuilder.Init();
                ThreateningAura.Init();
                CoolerHeartContainer.Init();
                FearBullets.Init();
                WaterAmmolet.Init();
                LichBullets.Init();
                portableVampire.Init();
                CleansingWater.Init();
                OldWarStealthkit.Init();                
                
                List<string> MoreTentacles = new List<string>
                {
                "abyssal_tentacle",
                "ans:tentacle_rounds"
                };
                List<string> NowYouAreCool = new List<string>
                {
                "sunglasses",
                "ans:leather_jacket"
                };
                CustomSynergies.Add("More Tentacles", MoreTentacles);
                CustomSynergies.Add("Now You Are Cool", NowYouAreCool);

                Hook FoyerHook = new Hook(
                    typeof(Foyer).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(Module).GetMethod("FoyerShrineHook")
                    );
                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }
        }
        public static void FoyerShrineHook(Action<Foyer> orig, Foyer self)
        {
            orig(self);
            ShrineFactory.PlaceBreachShrines();
        }
            public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
