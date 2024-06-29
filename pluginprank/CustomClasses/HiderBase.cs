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
    public class HiderBase : AbilityHandlers
    {

        public static Dictionary<string, CustomClass> Classes = new Dictionary<string, CustomClass>
        {
            { "Builder", new CustomClass(new List<Ability> { Ability.Active_Build }, "Flip your coins to build temporary walls!") },
            { "Speedster", new CustomClass(new List<Ability> { Ability.Active_Speed }, "Flip your coins to gain temporary speed!") },
            { "Gambler", new CustomClass(new List<Ability> { Ability.Active_Effects }, "Flip your coins to gain random effects!") },
            { "Jammer", new CustomClass(new List<Ability> { Ability.Active_Jam }, "Flip your coins to jam seeker trackers for 20s!") },
            { "Cloaker", new CustomClass(new List<Ability> { Ability.Active_Invis }, "Flip your coin to go invisible for 20s") },
            { "Trapper", new CustomClass(new List<Ability> { Ability.Active_Traps }, "Flip your coin to create a trap that severely cripples seekers.") },
            { "Hacker", new CustomClass(new List<Ability> { Ability.Passive_Glowing, Ability.Passive_Lock, Ability.Active_Hack }, "You passively glow, lock doors by interacting with them while holding your coin, and hack seekers with your coin.") },
        };

        public HiderBase(Player plrObj) : base(plrObj)
        {
            if (EventHandlers.HiderBases.ContainsKey(plrObj)) { throw new Exception("Preexisting Class Exception"); }
            if (EventHandlers.SeekerBases.ContainsKey(plrObj)) { EventHandlers.SeekerBases.Remove(plrObj); }
            playerName = plrObj.Nickname;
            plr = plrObj;
        }

        public void InstantiateHider(Vector3 spawnLocation)
        {
            if (plr == null) { Log.Error("Seeker instantiation failed. Cannot instantiate null player"); throw new NullReferenceException(); }
            plr.SetRole(RoleTypeId.ClassD);
            plr.IsGodModeEnabled = true;
            spawnLocation.y += 0.7f;
            plr.Position = spawnLocation;
            plr.Health = 100;

            KeyValuePair<string, CustomClass> element = Handlers.RandomKVP(Classes);
            //string k = "Hacker";
            //KeyValuePair<string, CustomClass> element = new KeyValuePair<string, CustomClass>(k, Classes[k]);
            plr.CustomInfo = element.Key;
            string classDescription = element.Value.Description;
            customClass = element.Value;


            //spawn text
            HintHandlers.InitPlayer(plr);
            HintHandlers.AddFadingText(plr, 875, $"<color=#eb8321><size=40>You are a hider {element.Key}.<br><br></size><size=30>Class Specific: {classDescription}<br>If you're killed by a seeker, you join them.</size></color>", 7.5f);
            Handlers.AddEffect<Slowness>(plr, 50);
            plr.ClearInventory();
            plr.AddItem(ItemType.KeycardMTFCaptain);
            plr.AddItem(ItemType.Radio);
            Handlers.SetScale(plr.ReferenceHub, new Vector3(0.25f, 0.25f, 0.25f));
            plr.IsGodModeEnabled = false;

            plr.AddItem(ItemType.Coin);
            customClass.onCooldown = false;
            if (customClass.abilities.Contains(Ability.Passive_Glowing)) { Handlers.AddLight(plr.ReferenceHub.transform, Color.green, 5, 2f) ; }
        }

        public void PerFrame()
        {
            if (plr == null) { return; } // GOD FUCKING DAMN IT
            //counttimer += Time.deltaTime;
            //if (counttimer > 1.05f)
            //{
            //    counttimer = 0; //resett!!!
            //}
        }
    }
}