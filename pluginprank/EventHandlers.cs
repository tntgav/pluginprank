using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using pluginprank;
using PlayerRoles.PlayableScps.Scp079;
using Waits;
using YamlDotNet.Serialization;
using UnityEngine.PlayerLoop;
using GameCore;
using RueI;
using RueI.Displays;
using RueI.Elements;
using InventorySystem.Items.MicroHID;
using MapGeneration;
using PlayerStatsSystem;
using PlayerRoles;
using PlayerRoles.Voice;
using System.Net;
using System.Threading;
using System.Timers;
using PlayerRoles.RoleAssign;
using RemoteAdmin;
using Scp914;
using PlayerRoles.PlayableScps.Scp079.Pinging;
using HarmonyLib;
using PluginAPI.Roles;
using InventorySystem.Items.Armor;
using MapGeneration.Distributors;
using Utf8Json.Formatters;
using Respawning;
using System.Collections;
using UnityEngine.Diagnostics;
using Interactables.Interobjects.DoorUtils;
using pluginprank.CustomClasses;
using Log = PluginAPI.Core.Log;
using UnityEngine.Rendering;

namespace pluginprank
{
    internal class EventHandlers
    {

        [PluginEvent(ServerEventType.RoundStart)]
        public async void RoundStart(RoundStartEvent ev)
        {
            await Task.Delay(150);
            Player scp = Player.GetPlayers()[Handlers.RangeInt(0, Player.GetPlayers().Count - 1)];
            SeekerBases.Add(scp, new SeekerBase(scp));
            foreach (Player plr in Player.GetPlayers())
            {
                if (plr == scp) continue;
                plr.SetRole(RoleTypeId.Scp079);
                HintHandlers.InitPlayer(plr);
                ServerConsole.AddLog($"initialized {plr.Nickname}");
            }
            SeekerBases[scp].InstantiateSeeker();



            //sadly the one part of the code i can't really port
            foreach (Player plr in Player.GetPlayers())
            {
                if (plr.Role == RoleTypeId.Scientist) continue;
                HintHandlers.InitPlayer(plr);
                HintHandlers.AddFadingText(plr, 900, "<color=#ebeb21><size=40>You are a hider.<br><br></size><size=30>Ping a location to begin hiding. If you don't hide within 60s you will die.</size></color>", 7.5f);
                Handlers.KillIf079(plr);
            }

            //FLAGPOLE SPAWNING!!!
            Handlers.GeneratePrimitiveEditor();
            mapeditorunborn.current = Handlers.primitiveEditor;
            mapspawner.spawnMap();
        }


        [PluginEvent(ServerEventType.PlayerDamage)]
        public bool PlayerDamage(PlayerDamageEvent ev)
        {

            float DealtDamage = ((StandardDamageHandler)ev.DamageHandler).Damage;
            return true;
        }

        [PluginEvent(ServerEventType.PlayerInteractDoor)]
        public bool PlayerInteractDoor(PlayerInteractDoorEvent ev)
        {
            try
            {
                if (!HiderBases.ContainsKey(ev.Player)) { return true; }
                if (ev.Player.CurrentItem == null) { return true; }
                if (HiderBases[ev.Player].customClass.abilities.Contains(Ability.Passive_Lock) && ev.Player.CurrentItem.ItemTypeId == ItemType.Coin)
                {
                    Handlers.asynclockdoor(ev.Door);
                    return false;
                }
                return true;
            } catch (Exception e)
            {
                Log.Error(e.ToString());
                return true;
            }
            
        }

        [PluginEvent(ServerEventType.TeamRespawn)]
        public bool TeamRespawn(TeamRespawnEvent ev)
        {
            return false;
        }

        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void PlayerCoinFlip(PlayerCoinFlipEvent ev)
        {
            if (SeekerBases.ContainsKey(ev.Player))
            {
                SeekerBases[ev.Player].RunActives();
            }

            if (HiderBases.ContainsKey(ev.Player))
            {
                HiderBases[ev.Player].RunActives();
            }
        }


        public static Dictionary<uint, PrimitiveObjectToy> TrapperLandmines = new Dictionary<uint, PrimitiveObjectToy>();
        public static Dictionary<uint, LightSourceToy> TrapperLights = new Dictionary<uint, LightSourceToy>();

        [HarmonyPatch(typeof(Scp079PingAbility), nameof(Scp079PingAbility.ServerProcessCmd))]
        public static class Scp079PingPatch
        {
        
            public static bool Prefix(Scp079PingAbility __instance)
            {
                return true;
            }
            public static void Postfix(Scp079PingAbility __instance)
            {
                __instance.Role.TryGetOwner(out ReferenceHub hub);
                Player plr = Player.Get(hub);
                ServerConsole.AddLog("we pinging gangulous pingulous");
                Vector3 pingPos = __instance._syncPos.Position;
                if (Scp079PingAbility.PingProcessors[__instance._syncProcessorIndex] is DefaultPingProcessor)
                {
                    if (HiderBases.ContainsKey(plr)) { HiderBases.Remove(plr); }
                    HiderBases.Add(plr, new HiderBase(plr));
                    HiderBases[plr].InstantiateHider(pingPos);
                }
            }
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void PlayerChangeRole(PlayerChangeRoleEvent ev)
        {
            ev.Player.CustomInfo = "";
        }

        [PluginEvent(ServerEventType.PlayerHandcuff)]
        public bool PlayerHandcuff(PlayerHandcuffEvent ev)
        {
            if (ev.Target.Role == RoleTypeId.ClassD) { return false; } return true;
        }

        [HarmonyPatch(typeof(StandardVoiceModule), nameof(StandardVoiceModule.ValidateReceive))]
        public static class DisableRadio
        {
            public static bool Prefix(StandardVoiceModule __instance)
            {
                if (__instance.CurrentChannel == VoiceChat.VoiceChatChannel.Radio) { return false; }
                return true;
            }
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool PlayerDropItem(PlayerDropItemEvent ev)
        {
            if (ev.Item.ItemTypeId == ItemType.Radio) { return false; } return true;
        }

        [PluginEvent(ServerEventType.PlayerThrowItem)]
        public bool PlayerThrowItem(PlayerThrowItemEvent ev)
        {
            if (ev.Item.ItemTypeId == ItemType.Radio) { return false; } return true;
        }

        [PluginEvent(ServerEventType.PlayerShotWeapon)]
        public bool PlayerShotWeapon(PlayerShotWeaponEvent ev)
        {
            if (ev.Player.Role == RoleTypeId.ClassD) return false;
            return true;
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public bool PlayerEscape(PlayerEscapeEvent ev)
        {
            return false;
        }





        public static bool RadiosJammed = false;

        [PluginEvent(ServerEventType.PlayerUsingRadio)]
        public void PlayerUsingRadio(PlayerUsingRadioEvent ev)
        {
            if (Round.IsRoundEnded) { return; } //doing this saves me from countless headaches and exceptions
            ev.Radio.BatteryPercent = byte.MaxValue;
            if (SeekerBases.ContainsKey(ev.Player))
            {
                SeekerBases[ev.Player].RunPassives();
                SeekerBases[ev.Player].PerFrame(); //seeker bs
            }

            if (HiderBases.ContainsKey(ev.Player))
            {
                HiderBases[ev.Player].RunPassives();
            }
        }

        [PluginEvent(ServerEventType.Scp914Activate)]
        public bool Scp914Activate(Scp914ActivateEvent ev)
        {
            return false; //we call this the ferito patch
        }

        public static Dictionary<Player, SeekerBase> SeekerBases = new Dictionary<Player, SeekerBase>();
        public static Dictionary<Player, HiderBase> HiderBases = new Dictionary<Player, HiderBase>();

        [PluginEvent(ServerEventType.PlayerDeath)]
        public async void PlayerDeath(PlayerDeathEvent ev)
        {
            if (ev.Attacker == null) { return; }

            if (ev.Attacker.Role == RoleTypeId.Scientist)
            {
                ev.Attacker.Health += 30;
                await Task.Delay(50);
                SeekerBases.Add(ev.Player, new SeekerBase(ev.Player));
                SeekerBases[ev.Player].InstantiateSeeker();
            }

        }
    }
}
