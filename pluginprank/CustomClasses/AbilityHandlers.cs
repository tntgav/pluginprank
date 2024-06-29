using AdminToys;
using CustomPlayerEffects;
using InventorySystem;
using Mirror;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pluginprank.CustomClasses
{
    public class AbilityHandlers
    {
        public CustomClass customClass;
        public string playerName;
        public float counttimer = 0;
        public Player plr;
        private DateTime OffAt;

        public AbilityHandlers(Player plrObj)
        {
            playerName = plrObj.Nickname;
            plr = plrObj;
        }

        public async void RunPassives()
        {
            if (customClass.abilities.Contains(Ability.Passive_Decay))
            {
                foreach (Player near in Player.GetPlayers())
                {
                    if (near.Role != plr.Role && Vector3.Distance(near.Position, plr.Position) < 5)
                    {
                        near.Damage(4.2f * Time.deltaTime, "Decayed by the Plagued");
                    }
                }
            }

            if (customClass.abilities.Contains(Ability.Passive_Trappable))
            {
                foreach (KeyValuePair<uint, PrimitiveObjectToy> kvp in EventHandlers.TrapperLandmines)
                {
                    
                    if (Vector3.Distance(kvp.Value.Position, plr.Position) < 1.2f)
                    {

                        ServerConsole.AddLog("Found nearby trap!");
                        NetworkServer.Destroy(kvp.Value.gameObject);
                        NetworkServer.Destroy(EventHandlers.TrapperLights[kvp.Key].gameObject);

                        Handlers.CreateThrowable(ItemType.GrenadeFlash).SpawnActive(kvp.Value.Position, 0.05f);
                        Handlers.SetEffect<Flashed>(plr, 1, 10);
                        Handlers.SetEffect<Concussed>(plr, 1, 10);
                        Handlers.SetEffect<Deafened>(plr, 1, 10);
                    }
                }
            }

            if (customClass.abilities.Contains(Ability.Passive_Speed))
            {
                Handlers.SetEffect<MovementBoost>(plr, 50, 30);
            }

            if (customClass.abilities.Contains(Ability.Passive_Healing))
            {
                plr.Heal(1 * Time.deltaTime);
            }
        }

        public async void RunActives()
        {
            if (customClass.onCooldown)
            {
                HintHandlers.InitPlayer(plr);
                float untilOff = (float)(OffAt - DateTime.Now).TotalSeconds;

                HintHandlers.AddFadingText(plr, 275, $"On cooldown for {untilOff} seconds.", 1.2f);
                return;
            }

            Log.Debug($"running active abilities for {playerName}");
            Log.Debug($"They have {string.Join(", ", customClass.abilities)}");
            if (customClass.abilities.Contains(Ability.Active_Build))
            {
                Log.Debug($"    - running builder code");
                PrimitiveObjectToy prim = Handlers.SpawnPrim(plr.Position, new Vector3(0.85f, 0.4f, 0.2f), plr.Rotation, Color.white, PrimitiveType.Cube);
                //plr.RemoveItem(plr.ReferenceHub.inventory.CurInstance);
                PutOnCooldown(4.5f);
                await Task.Delay(10000);
                NetworkServer.Destroy(prim.gameObject);
            }

            if (customClass.abilities.Contains(Ability.Active_Speed))
            {
                Log.Debug($"    - running speedster code");
                PutOnCooldown(5f);
                Handlers.AddEffect<MovementBoost>(plr, 150, 2);
                Handlers.RemoveEffect<Slowness>(plr);
                await Task.Delay(3000);
                Handlers.SetEffect<Slowness>(plr, 50);
            }

            if (customClass.abilities.Contains(Ability.Active_Effects))
            {
                Log.Debug($"    - running gambler code");
                List<string> gainedEffects = new List<string>();
                PutOnCooldown(15);
                float chance = 30;
                float positiveMod = 1.13f;
                float negativeMod = 0.96f;
                //positive effects
                if (Handlers.RollChance(chance * positiveMod)) { Handlers.AddEffect<Invisible>(plr, 1, 10); gainedEffects.Add("Invisibility"); }
                if (Handlers.RollChance(chance * positiveMod)) { Handlers.AddEffect<MovementBoost>(plr, 75, 10); gainedEffects.Add("Movement boost"); }
                if (Handlers.RollChance(chance * positiveMod)) { Handlers.AddEffect<DamageReduction>(plr, 190, 10); gainedEffects.Add("Damage Reduction"); }
                if (Handlers.RollChance(chance * positiveMod)) { Handlers.AddEffect<AntiScp207>(plr, 3, 10); gainedEffects.Add("AntiSCP-207"); }
                if (Handlers.RollChance(chance * positiveMod)) { Handlers.AddEffect<Ghostly>(plr, 1, 10); gainedEffects.Add("Ghostly"); }
                if (Handlers.RollChance(chance * positiveMod)) { Handlers.AddEffect<Scp207>(plr, 3, 10); gainedEffects.Add("SCP-207"); }
                //negative effects
                if (Handlers.RollChance(chance * negativeMod)) { Handlers.AddEffect<Burned>(plr, 25, 10); gainedEffects.Add("Burned"); }
                if (Handlers.RollChance(chance * negativeMod)) { Handlers.AddEffect<Stained>(plr, 1, 10); gainedEffects.Add("Stained"); }
                if (Handlers.RollChance(chance * negativeMod)) { Handlers.AddEffect<SilentWalk>(plr, 11, 10); gainedEffects.Add("Silent Walk"); }
                if (Handlers.RollChance(chance * negativeMod)) { Handlers.AddEffect<Sinkhole>(plr, 1, 10); gainedEffects.Add("Sinkholed"); }
                if (Handlers.RollChance(chance * negativeMod)) { Handlers.AddEffect<Traumatized>(plr, 1, 10); gainedEffects.Add("Traumatized"); }
                if (Handlers.RollChance(chance * negativeMod)) { Handlers.AddEffect<Flashed>(plr, 1, 2); gainedEffects.Add("Flashed"); }

                if (Handlers.RollChance(chance / 1000)) { Warhead.Detonate(); }
                HintHandlers.InitPlayer(plr);
                string built = $"You gained: {string.Join(", ", gainedEffects)}!";
                HintHandlers.AddFadingText(plr, 275, built, 2f);
            }

            if (customClass.abilities.Contains(Ability.Active_Jam))
            {
                Log.Debug($"    - running jammer code");
                EventHandlers.RadiosJammed = true;
                HintHandlers.InitPlayer(plr);
                HintHandlers.AddFadingText(plr, 275, "Radios Jammed for 30s.", 2f);
                PutOnCooldown(60f);
                await Task.Delay(30000);
                EventHandlers.RadiosJammed = false;
            }

            if (customClass.abilities.Contains(Ability.Active_Invis))
            {
                Log.Debug($"    - running cloaker code");
                PutOnCooldown(90f);
                Handlers.AddEffect<Invisible>(plr, 1, 20);
            }

            if (customClass.abilities.Contains(Ability.Active_Traps))
            {
                Log.Debug($"    - running trapper code");
                Vector3 pos = plr.Position;
                pos.y -= 0.4f;

                PrimitiveObjectToy prim = Handlers.SpawnPrim(pos, new Vector3(0.6f, 0.2f, 0.6f), plr.Rotation, Color.red, PrimitiveType.Cylinder, false);
                PutOnCooldown(120f);
                EventHandlers.TrapperLandmines.Add(prim.netId, prim);
                Vector3 lightpos = pos;
                lightpos.y += 0.4f;
                LightSourceToy light = Handlers.AddLight(lightpos, Color.red, 50, 3f);
                EventHandlers.TrapperLights.Add(prim.netId, light);
            }

            if (customClass.abilities.Contains(Ability.Active_Hack))
            {
                foreach (Player tohack in EventHandlers.SeekerBases.Keys)
                {
                    Handlers.AddEffect<Strangled>(tohack, 150, 3);
                    Handlers.AddEffect<Ensnared>(tohack, 150, 8);
                }
            }
        }

        public async void PutOnCooldown(float time)
        {
            //get [time] seconds into the future
            customClass.onCooldown = true;
            DateTime after = DateTime.Now.AddSeconds(time);
            OffAt = after;
            await Task.Delay((int)time * 1000);

            customClass.onCooldown = false;
        }
    }
}
