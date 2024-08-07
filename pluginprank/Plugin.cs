﻿using HarmonyLib;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using RueI;
using RueI.Events;
using System;

namespace pluginprank
{
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }

        private static EventHandlers EventHandlers { get; set; }

        [PluginEntryPoint("silly", "1.0.0", "[PLUGIN DESCRIPTION]", "cat")]
        void LoadPluginAPI()
        {
            Instance = this;

            EventHandlers = new EventHandlers();
            EventManager.RegisterEvents(EventHandlers);
            EventManager.RegisterAllEvents(EventHandlers);
            EventManager.RegisterEvents(this);
            EventManager.RegisterEvents<mapeditorunborn>(this);
            RueIMain.EnsureInit();
            Harmony _harmony;
            _harmony = new Harmony("com.tpd.patches");
            _harmony.PatchAll();
            ServerConsole.AddLog("Normalcat's plugin has been loaded... unfortunately...", ConsoleColor.Magenta);
        }
        [PluginUnload]
        void UnloadPluginAPI()
        {
            EventManager.UnregisterEvents(EventHandlers);
        }
    }
}