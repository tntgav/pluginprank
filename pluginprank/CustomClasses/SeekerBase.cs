using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;
using InventorySystem.Items.ThrowableProjectiles;
using System.ComponentModel;
using Mirror;
using UnityEngine;
using Footprinting;
using InventorySystem.Items.Pickups;
using InventorySystem.Items;
using InventorySystem;
using AdminToys;
using InventorySystem.Items.Firearms;
using CustomPlayerEffects;
using MapGeneration;
using PlayerRoles;
using System.Security.Policy;

namespace pluginprank.CustomClasses
{
    public class SeekerBase : AbilityHandlers
    { 

        public static Dictionary<string, CustomClass> Classes = new Dictionary<string, CustomClass>
            {
                { "Plagued", new CustomClass(new List<Ability> {Ability.Passive_Decay, Ability.Passive_Trappable}, "You passively deal strong damage to nearby hiders.")},
                { "Falcon", new CustomClass(new List<Ability> {Ability.Passive_Invigorated, Ability.Passive_NoFog, Ability.Passive_Trappable}, "You have infinite sprint and better sight.")},
            };

        public SeekerBase(Player plrObj) : base(plrObj) 
        {
            if (EventHandlers.SeekerBases.ContainsKey(plrObj)) { throw new Exception("Preexisting Class Exception"); }
            if (EventHandlers.HiderBases.ContainsKey(plrObj)) { EventHandlers.HiderBases.Remove(plrObj); }
            playerName = plrObj.Nickname;
            plr = plrObj;
        }

        public async void InstantiateSeeker()
        {
            if (plr == null) { Log.Error("Seeker instantiation failed. Cannot instantiate null player"); throw new NullReferenceException(); }

            plr.SetRole(RoleTypeId.Scientist);
            plr.ClearInventory();
            plr.AddItem(ItemType.Medkit);
            plr.AddItem(ItemType.GunCrossvec);
            plr.AddItem(ItemType.KeycardO5);
            plr.AddItem(ItemType.ArmorHeavy);
            plr.AddAmmo(ItemType.Ammo9x19, 200);

            Handlers.SetEffect<MovementBoost>(plr, Convert.ToByte(25 + (7 * Player.GetPlayers().Count)));
            Handlers.SetScale(plr.ReferenceHub, new Vector3(1, 1, 1));
            plr.Position = new Vector3(-0.15f, 1001f, -41.22f);

            KeyValuePair<string, CustomClass> element = Handlers.RandomKVP(Classes);
            plr.CustomInfo = element.Key;
            customClass = element.Value;
            string classDescription = element.Value.Description;
            HintHandlers.InitPlayer(plr);
            HintHandlers.AddFadingText(plr, 900, $"<color=#ebeb21><size=40>You are a seeker {plr.CustomInfo}</size><size=30><br>Class Specific: {classDescription}<br><br>You naturally track hiders, but drain HP over time.<br>Killing hiders converts them into seekers.</size></color>", 7.5f);
            await Task.Delay(7600);
            plr.AddItem(ItemType.Radio);

            if (customClass.abilities.Contains(Ability.Passive_Invigorated)) Handlers.SetEffect<Invigorated>(plr, 1);
            if (customClass.abilities.Contains(Ability.Passive_NoFog)) Handlers.SetEffect<FogControl>(plr, 1);
        }

        public void PerFrame()
        {
            if (plr == null) { return; } // GOD FUCKING DAMN IT
            counttimer += Time.deltaTime;
            plr.Damage(0.33f * Time.deltaTime, "Seeker Decay");
            bool RadiosJammed = EventHandlers.RadiosJammed;
            if (counttimer > 1.05f)
            {
                counttimer = 0; //resett!!!

                Player closestPlayer = ClosestHider(plr.Position);
                int hidercount = Handlers.ClassCount(RoleTypeId.ClassD);
                string built = "";

                if (closestPlayer != null)
                {
                    float lowest = Vector3.Distance(plr.Position, closestPlayer.Position);
                    FacilityZone zone = closestPlayer.Zone;
                    RoomIdentifier room = closestPlayer.Room;
                    built = BuildText(lowest, hidercount, RadiosJammed, zone, room, lowest / 20);
                } else
                {
                    if (Handlers.ClassCount(RoleTypeId.ClassD) == 0 && Handlers.ClassCount(RoleTypeId.Scp079) > 0) { built = "<color=#eb21eb>Waiting for hiders...</color>"; }
                }

                
                HintHandlers.InitPlayer(plr);
                HintHandlers.AddFadingText(plr, 200, built, 1f);
            }
        }

        private Player ClosestHider(Vector3 pos)
        {
            Dictionary<Player, float> plrdist = Handlers.NearestPlayers(plr.Position, Player.GetPlayers().Count);

            float lowest = float.PositiveInfinity;
            Player closest = null;

            foreach (KeyValuePair<Player, float> kvp in plrdist) { if (Vector3.Distance(plr.Position, kvp.Key.Position) < lowest && kvp.Key.Role == RoleTypeId.ClassD) { lowest = Vector3.Distance(plr.Position, kvp.Key.Position); closest = kvp.Key; } }
            return closest;
        }

        private string BuildText(float lowest, int hidercount, bool RadiosJammed, FacilityZone zone, RoomIdentifier room, float variance)
        {
            lowest += (float)Handlers.RangeDouble(-variance, variance);
            string built = lowest == float.PositiveInfinity ? $"<color=#00ff00>Seekers win!</color>" : $"<color=#eb21eb>{hidercount} hiders alive. Nearest hider is {lowest} units away, in {zone}.</color>";
            if (RadiosJammed)
            {
                built = $"999 hiders alive. Nearest hider is ERR CODE 3114: Cannot connect to hider GPS System.";
                built = $"<color=#{string.Join("", Handlers.RandomChars(6))}>{Handlers.JammedText(built, built.Length / 3)}</color>";
            }
            if (((lowest < 65 && room != null) && !RadiosJammed))
            {
                built += $"<br>They are in {room.name}";
            }
            
            return built;
        }

        
    }
}

