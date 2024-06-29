using AdminToys;
using MapGeneration;
using Mirror;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pluginprank
{
    public class mapeditorunborn
    {
        [PluginEvent(ServerEventType.PlayerChangeRadioRange)]
        public bool PlayerChangeRadioRange(PlayerChangeRadioRangeEvent ev)
        {
            //THIS EVENT IS FOR CHANGING OPTIONS
            if (ev.Player.Role != RoleTypeId.Tutorial) return false;
            if (current == null) { return false; }
            index++;
            if (index > current.children.Count - 1 || index < 0) index = 0;
            List<filesystem> listversion = Handlers.generateListVersion(current);
            //Log.Info($"index {index}/{current.children.Count - 1}");
            //Log.Info($"name is {listversion[index].name}. ");
            //if (listversion[index].parent != null) { Log.Info($"parent name is {listversion[index].parent.name}"); }
            //HintHandlers.InitPlayer(ev.Player);
            //HintHandlers.AddFadingText(ev.Player, 325, $"{listversion[index].name}", 1f);
            string built = $"\n> {current.name}";
            int iter = 0;
            foreach (filesystem file in listversion)
            {
                if (iter == index) { built += $"\n   > {file.name}"; iter++; continue; }
                built += $"\n    {file.name}";
                iter++;
            }
            Log.Info(built);
            return false; // with new passive system everyone needs a radio
        }

        public static filesystem current;
        public static List<filesystem> listversion = new List<filesystem>();
        public static int index = -1;
        public static PrimitiveObjectToy currentprim;
        public static List<PrimitiveObjectToy> allSpawned = new List<PrimitiveObjectToy>();

        [PluginEvent(ServerEventType.PlayerRadioToggle)]
        public bool PlayerRadioToggle(PlayerRadioToggleEvent ev)
        {
            //THIS EVENT IS FOR GOING WITH THE CURRENT OPTION
            if (ev.Player.Role != RoleTypeId.Tutorial) return false;
            if (current == null) { return false; }
            filesystem realcurrent = Handlers.generateListVersion(current)[index];
            if (realcurrent.name.StartsWith("."))
            {
                Log.Info($"Going back from {current.name} to {current.parent.name}. resetting index...");
                current = current.parent;
                index = -1;
            }
            else if (realcurrent.name.StartsWith("_") || realcurrent.name.StartsWith("+") || realcurrent.name.StartsWith("-"))
            {
                if (realcurrent.name == "_confirm" && current.name == "finish")
                {
                    allSpawned.Add(currentprim);
                    currentprim = null; //reset for next add
                }

                if (realcurrent.name == "_confirm" && current.name == "duplicate")
                {
                    allSpawned.Add(currentprim);
                    PrimitiveObjectToy alt = currentprim;
                    currentprim = Handlers.SpawnPrim(alt.transform.localPosition, alt.transform.localScale, alt.transform.localRotation.eulerAngles, alt.MaterialColor, alt.PrimitiveType);
                }

                if (realcurrent.name == "_confirm" && current.name == "destroy")
                {
                    NetworkServer.Destroy(currentprim.gameObject);
                    currentprim = null;
                }

                if (realcurrent.name == "_export")
                {
                    string built = "\n";//starting on one line and ending on the other is ugly
                    foreach (PrimitiveObjectToy prim in allSpawned)
                    {
                        if (prim == null) { continue; } //GOD FUCKING DAMN IT I LOST SO MUCH PROGRESS
                        RoomIdentifier roomOffset = Handlers.NearestRoom(prim.transform.position);
                        Vector3 offset = prim.transform.position - roomOffset.transform.position;
                        Vector3 roomangle = roomOffset.transform.rotation.eulerAngles;
                        //roomangle is CORRECT
                        int digits = Handlers.RangeInt(100, 999);

                        built += ($"\nBuilding exported{prim.netId}{digits} = new Building(rooms[\"{roomOffset.name}\"].transform.position, rooms[\"{roomOffset.name}\"].transform.rotation.eulerAngles + new Vector3({roomangle.x}, {roomangle.y}, {roomangle.z}), Vector3.one, \"EXPORTED{prim.netId}{digits}\");");
                        built += ($"\nexported{prim.netId}{digits}.Add(new Structure(new Vector3({offset.x}f, {offset.y}f, {offset.z}f), new Vector3({prim.transform.localScale.x}f, {prim.transform.localScale.y}f, {prim.transform.localScale.z}f), new Vector3({prim.transform.localRotation.x}f, {prim.transform.localRotation.y}f, {prim.transform.localRotation.z}f), PrimitiveType.{prim.PrimitiveType}, new Color({prim.MaterialColor.r}f, {prim.MaterialColor.g}f, {prim.MaterialColor.b}f, {prim.MaterialColor.a}f), \"exported{prim.netId}{digits}\"));");
                        built += ($"\nexported{prim.netId}{digits}.SpawnBuilding();");
                    }
                    Log.Info(built);
                }
                if (current.name == "create" && currentprim == null) //cant add while current one isnt finalized
                {
                    switch (realcurrent.name)
                    {
                        case "_cube":
                            currentprim = Handlers.SpawnPrim(ev.Player.Position, Vector3.one, Vector3.zero, Color.white, PrimitiveType.Cube, true); break;
                        case "_capsule":
                            currentprim = Handlers.SpawnPrim(ev.Player.Position, Vector3.one, Vector3.zero, Color.white, PrimitiveType.Capsule, true); break;
                        case "_cylinder":
                            currentprim = Handlers.SpawnPrim(ev.Player.Position, Vector3.one, Vector3.zero, Color.white, PrimitiveType.Cylinder, true); break;
                        case "_sphere":
                            currentprim = Handlers.SpawnPrim(ev.Player.Position, Vector3.one, Vector3.zero, Color.white, PrimitiveType.Sphere, true); break;
                    }
                }
                if (current.parent.parent != null)
                {
                    if (current.parent.parent.name == "edit")
                    {
                        if (current.parent.name == "scale")
                        {
                            if (current.name == "x")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+0.05":
                                        currentprim.transform.localScale = new Vector3((float)(currentprim.transform.localScale.x + 0.05), currentprim.transform.localScale.y, currentprim.transform.localScale.z); break;
                                    case "-0.05":
                                        currentprim.transform.localScale = new Vector3((float)(currentprim.transform.localScale.x - 0.05), currentprim.transform.localScale.y, currentprim.transform.localScale.z); break;
                                    case "+0.5":
                                        currentprim.transform.localScale = new Vector3((float)(currentprim.transform.localScale.x + 0.5), currentprim.transform.localScale.y, currentprim.transform.localScale.z); break;
                                    case "-0.5":
                                        currentprim.transform.localScale = new Vector3((float)(currentprim.transform.localScale.x - 0.5), currentprim.transform.localScale.y, currentprim.transform.localScale.z); break;

                                }
                            }
                            if (current.name == "y")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+0.05":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, (float)(currentprim.transform.localScale.y + 0.05), currentprim.transform.localScale.z); break;
                                    case "-0.05":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, (float)(currentprim.transform.localScale.y - 0.05), currentprim.transform.localScale.z); break;
                                    case "+0.5":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, (float)(currentprim.transform.localScale.y + 0.5), currentprim.transform.localScale.z); break;
                                    case "-0.5":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, (float)(currentprim.transform.localScale.y - 0.5), currentprim.transform.localScale.z); break;

                                }
                            }
                            if (current.name == "z")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+0.05":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, currentprim.transform.localScale.y, (float)(currentprim.transform.localScale.z + 0.05)); break;
                                    case "-0.05":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, currentprim.transform.localScale.y, (float)(currentprim.transform.localScale.z - 0.05)); break;
                                    case "+0.5":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, currentprim.transform.localScale.y, (float)(currentprim.transform.localScale.z + 0.5)); break;
                                    case "-0.5":
                                        currentprim.transform.localScale = new Vector3(currentprim.transform.localScale.x, currentprim.transform.localScale.y, (float)(currentprim.transform.localScale.z - 0.5)); break;

                                }
                            }
                        }

                        if (current.parent.name == "rotation")
                        {
                            Vector3 rot = currentprim.transform.localRotation.eulerAngles;
                            if (current.name == "x")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+1":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x + 1f, rot.y, rot.z)); break;
                                    case "-1":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x - 1f, rot.y, rot.z)); break;
                                    case "+5":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x + 5f, rot.y, rot.z)); break;
                                    case "-5":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x - 5f, rot.y, rot.z)); break;
                                }
                            }
                            if (current.name == "y")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+1":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y + 1f, rot.z)); break;
                                    case "-1":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y - 1f, rot.z)); break;
                                    case "+5":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y + 5f, rot.z)); break;
                                    case "-5":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y - 5f, rot.z)); break;

                                }
                            }
                            if (current.name == "z")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+1":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z + 1f)); break;
                                    case "-1":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z - 1f)); break;
                                    case "+5":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z + 5f)); break;
                                    case "-5":
                                        currentprim.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z - 5f)); break;
                                }
                            }
                        }

                        if (current.parent.name == "position")
                        {
                            if (current.name == "x")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+0.05":
                                        currentprim.transform.position = new Vector3((float)(currentprim.transform.position.x + 0.05), currentprim.transform.position.y, currentprim.transform.position.z); break;
                                    case "-0.05":
                                        currentprim.transform.position = new Vector3((float)(currentprim.transform.position.x - 0.05), currentprim.transform.position.y, currentprim.transform.position.z); break;
                                    case "+0.5":
                                        currentprim.transform.position = new Vector3((float)(currentprim.transform.position.x + 0.5), currentprim.transform.position.y, currentprim.transform.position.z); break;
                                    case "-0.5":
                                        currentprim.transform.position = new Vector3((float)(currentprim.transform.position.x - 0.5), currentprim.transform.position.y, currentprim.transform.position.z); break;

                                }
                            }
                            if (current.name == "y")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+0.05":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, (float)(currentprim.transform.position.y + 0.05), currentprim.transform.position.z); break;
                                    case "-0.05":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, (float)(currentprim.transform.position.y - 0.05), currentprim.transform.position.z); break;
                                    case "+0.5":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, (float)(currentprim.transform.position.y + 0.5), currentprim.transform.position.z); break;
                                    case "-0.5":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, (float)(currentprim.transform.position.y - 0.5), currentprim.transform.position.z); break;

                                }
                            }
                            if (current.name == "z")
                            {
                                switch (realcurrent.name)
                                {
                                    case "+0.05":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, currentprim.transform.position.y, (float)(currentprim.transform.position.z + 0.05)); break;
                                    case "-0.05":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, currentprim.transform.position.y, (float)(currentprim.transform.position.z - 0.05)); break;
                                    case "+0.5":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, currentprim.transform.position.y, (float)(currentprim.transform.position.z + 0.5)); break;
                                    case "-0.5":
                                        currentprim.transform.position = new Vector3(currentprim.transform.position.x, currentprim.transform.position.y, (float)(currentprim.transform.position.z - 0.5)); break;

                                }
                            }
                        }
                    }

                    if (current.name == "color")
                    {
                        switch (realcurrent.name)
                        {
                            case "_black": currentprim.NetworkMaterialColor = Color.black; break;
                            case "_blue": currentprim.NetworkMaterialColor = Color.blue; break;
                            case "_clear": currentprim.NetworkMaterialColor = Color.clear; break;
                            case "_cyan": currentprim.NetworkMaterialColor = Color.cyan; break;
                            case "_gray": currentprim.NetworkMaterialColor = Color.gray; break;
                            case "_green": currentprim.NetworkMaterialColor = Color.green; break;
                            case "_magenta": currentprim.NetworkMaterialColor = Color.magenta; break;
                            case "_red": currentprim.NetworkMaterialColor = Color.red; break;
                            case "_white": currentprim.NetworkMaterialColor = Color.white; break;
                            case "_yellow": currentprim.NetworkMaterialColor = Color.yellow; break;
                        }
                    }
                }
            }
            else
            {
                int count = 0;
                foreach (KeyValuePair<string, filesystem> file in current.children)
                {
                    if (count == index) { current = file.Value; Log.Info($"Going into {file.Value.name} from {current.name}. resetting index..."); index = -1; }
                    count++;
                }
            }
            return false; // with new passive system everyone needs a radio
        }
    }
}
