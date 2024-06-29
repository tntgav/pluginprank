using Footprinting;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using InventorySystem.Items;
using Mirror;
using PluginAPI.Core;
using System.Collections.ObjectModel;
using UnityEngine;
using CustomPlayerEffects;
using System;
using MapGeneration;
using System.Collections.Generic;
using InventorySystem;
using Utf8Json.Formatters;
using System.Linq;
using PlayerRoles.PlayableScps.Scp079;
using AdminToys;
using PlayerRoles;
using System.Threading.Tasks;
using Interactables.Interobjects.DoorUtils;
using PluginAPI.Core.Items;
using System.Reflection;

namespace pluginprank {

    public static class Handlers {
        public static void SpawnActive(
                    this ThrowableItem item,
                    Vector3 position,
                    float fuseTime = -1f,
                    Player owner = null
        )
        {
            TimeGrenade grenade = (TimeGrenade)UnityEngine.Object.Instantiate(item.Projectile, position, Quaternion.identity);
            if (fuseTime >= 0)
                grenade._fuseTime = fuseTime;
            grenade.NetworkInfo = new PickupSyncInfo(item.ItemTypeId, item.Weight, item.ItemSerial);
            grenade.PreviousOwner = new Footprint(owner != null ? owner.ReferenceHub : ReferenceHub.HostHub);
            NetworkServer.Spawn(grenade.gameObject);
            grenade.ServerActivate();
        }

        public static ThrowableItem CreateThrowable(ItemType type, Player player = null) => (player != null ? player.ReferenceHub : ReferenceHub.HostHub)
            .inventory.CreateItemInstance(new ItemIdentifier(type, ItemSerialGenerator.GenerateNext()), false) as ThrowableItem;

        // public static ReadOnlyCollection<ItemPickupBase> GetPickups() => Object.FindObjectsOfType<ItemPickupBase>().ToList().AsReadOnly();

        public static void AddEffect<T>(Player player, byte intensity, int addedDuration = 0) where T : StatusEffectBase
        {
            foreach (StatusEffectBase effect in player.ReferenceHub.playerEffectsController.AllEffects)
            {
                if (effect.GetType() == typeof(T))
                {
                    byte inten = effect.Intensity;
                    float duration = effect.Duration;
                    byte newIntensity = Math.Min(System.Convert.ToByte(intensity + inten), System.Convert.ToByte(200));
                    player.ReferenceHub.playerEffectsController.ChangeState<T>(newIntensity, duration + addedDuration);
                    ServerConsole.AddLog($"{player.Nickname} has been given/added {effect.name} of intensity {newIntensity} for {duration+addedDuration} seconds");
                }
            }
        }

        public static void SetEffect<T>(Player player, byte intensity, int addedDuration = 0) where T : StatusEffectBase
        {
            foreach (StatusEffectBase effect in player.ReferenceHub.playerEffectsController.AllEffects)
            {
                if (effect.GetType() == typeof(T))
                {
                    byte inten = effect.Intensity;
                    float duration = effect.Duration;

                    player.ReferenceHub.playerEffectsController.ChangeState<T>(intensity, duration + addedDuration);
                }
            }
        }

        public static void RemoveEffect<T>(Player player) where T : StatusEffectBase
        {
            foreach (StatusEffectBase effect in player.ReferenceHub.playerEffectsController.AllEffects)
            {
                if (effect.GetType() == typeof(T))
                {
                    player.ReferenceHub.playerEffectsController.DisableEffect<T>();
                }
            }
        }

        public static string GetPlayerEffectList(Player player)
        {
            StatusEffectBase[] plreffects = player.ReferenceHub.playerEffectsController.AllEffects;
            int iter = 0;
            string built = string.Empty;
            foreach (var eff in plreffects)
            {
                iter++;
                string colorTag = string.Empty;
                switch (eff.Classification)
                {
                    case StatusEffectBase.EffectClassification.Negative:
                        colorTag = "<color=red>";
                        break;
                    case StatusEffectBase.EffectClassification.Positive:
                        colorTag = "<color=#32CD32>";
                        break;
                    case StatusEffectBase.EffectClassification.Mixed:
                        colorTag = "<color=#FF00FF>";
                        break;
                }
                if (eff.Intensity > 0) { built += String.Format("<align=left><size=15>{2} {1}x {0}</color></size>\n", eff.name, eff.Intensity, colorTag); }

            }
            return built;

        }

        public static async void asynclockdoor(DoorVariant door)
        {
            door.ServerChangeLock(DoorLockReason.AdminCommand, true);
            await Task.Delay(3000);
            door.ServerChangeLock(DoorLockReason.AdminCommand, false);
        }
        public static void GrenadePosition(Vector3 position)
        {
            CreateThrowable(ItemType.GrenadeHE).SpawnActive(position, 0.05f);
        }

        public static void GrenadePosition(Vector3 position, int amount)
        {
            for (int i = 0; i < amount;)
            {
                CreateThrowable(ItemType.GrenadeHE).SpawnActive(position, 0.05f);
            }
        }

        public static void GrenadePosition(Vector3 position, Player plr)
        {
            CreateThrowable(ItemType.GrenadeHE, plr).SpawnActive(position, 0.05f);
        }

        public static void GrenadePosition(Vector3 position, int amount, Player plr)
        {
            for (int i = 0; i < amount;)
            {
                CreateThrowable(ItemType.GrenadeHE, plr).SpawnActive(position, 0.05f);
            }
        }

        public static ItemPickupBase CreatePickup(Vector3 position, ItemBase prefab)
        {
            ItemPickupBase clone = UnityEngine.Object.Instantiate(prefab.PickupDropModel, position, Quaternion.identity);
            clone.NetworkInfo = new PickupSyncInfo(prefab.ItemTypeId, prefab.Weight);
            clone.PreviousOwner = new Footprint(ReferenceHub.HostHub);
            return clone;
        }

        public static RoomIdentifier RandomRoom()
        {
            //note to future self. this code does not work, i dont wanna deal with it rn
            List<RoomIdentifier> RoomCollection = new List<RoomIdentifier>();
            foreach (var room in Map.Rooms)
            {
                RoomCollection.Add(room);
            }
            RoomIdentifier NewRoom = RoomCollection[new System.Random().Next(RoomCollection.Count)];
            return NewRoom;
        }

        public static List<RoomIdentifier> AllRooms()
        {
            //note to future self. this code does not work, i dont wanna deal with it rn
            List<RoomIdentifier> RoomCollection = new List<RoomIdentifier>();
            foreach (var room in Map.Rooms)
            {
                RoomCollection.Add(room);
            }
            return RoomCollection;
        }

        public static Vector3 moveRand(Vector3 originalPosition, float distance)
        {
            // Generate a random direction
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere.normalized;

            float randomDistance = UnityEngine.Random.Range(0f, distance);

            // Calculate the new position by moving the original position along the random direction by the random distance
            Vector3 newPosition = originalPosition + randomDirection * randomDistance;

            return newPosition;
        }

        public static void spawnItem(Vector3 pos, ItemType nonbaseType, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                InventoryItemLoader.AvailableItems.TryGetValue(nonbaseType, out ItemBase item);
                ItemPickupBase fnunyitem = Handlers.CreatePickup(pos, item);
                NetworkServer.Spawn(fnunyitem.gameObject);
            }
        }

        public static void SpawnGrid3D(Vector3 centerPosition, int itemsPerRow, int itemsPerColumn, float distanceBetweenItems, ItemType itemId)
        {
            // Calculate the starting position to center the grid around the center position
            float totalWidth = (itemsPerRow - 1) * distanceBetweenItems;
            float totalHeight = (itemsPerColumn - 1) * distanceBetweenItems;
            Vector3 startPosition = centerPosition - new Vector3(totalWidth / 2, totalHeight / 2, totalWidth / 2);

            // Loop to spawn items in a 3D grid
            for (int row = 0; row < itemsPerRow; row++)
            {
                for (int col = 0; col < itemsPerRow; col++)
                {
                    for (int depth = 0; depth < itemsPerColumn; depth++)
                    {
                        // Calculate the position for each item
                        Vector3 position = startPosition + new Vector3(col * distanceBetweenItems, depth * distanceBetweenItems, row * distanceBetweenItems);
                        position.y += itemsPerColumn * distanceBetweenItems;
                        // Call the Handlers.spawnItem method to spawn the item at the calculated position
                        spawnItem(position, itemId);
                    }
                }
            }
        }

        public static Dictionary<Player, float> NearestPlayers(Vector3 position, int players = 1)
        {
            //99% chance theres a better way to do this. i do not care.
            List<Player> allPlayers = Player.GetPlayers();
            List<Player> nearestPlayers = allPlayers
            .OrderBy(player => Vector3.Distance(player.Position, position))
            .Take(players)
            .ToList();

            Dictionary<Player, float> Final = new Dictionary<Player, float>();
            foreach (Player player in nearestPlayers)
            {
                Final.Add(player, Vector3.Distance(position, player.Position));
            }
            return Final;

        }

        public static float getBaseAuxRegen(Player plr)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                return auxManager.RegenSpeed;
            }
            return 0;
        }

        public static void setAux(Player plr, float amount)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                auxManager.CurrentAux = amount;
            }
        }

        public static float getAux(Player plr)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                return auxManager.CurrentAux;
            }
            return 0f;
        }

        public static float getMaxAux(Player plr)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                return auxManager.MaxAux;
            }
            return 0f;
        }

        public static void addAux(Player plr, float amount)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                auxManager.CurrentAux += amount;
            }
        }

        public static Vector3 cameraPosition(Player plr)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                return scp079Role.CameraPosition;
            }
            return Vector3.zero;
        }

        public static Vector3 cameraAngle(Player plr)
        {
            if (plr.RoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager auxManager))
            {
                Quaternion rotation = scp079Role.CurrentCamera.transform.rotation;
                return new Vector3(rotation.x, rotation.y, rotation.z);
            }
            return Vector3.zero;
        }

        public static Vector3 raycastFrom(Vector3 position, Vector3 rotation)
        {
            RaycastHit hit;
            Physics.Raycast(position, rotation, out hit);
            return hit.point;
        }

        private static System.Random random = new System.Random();

        public static bool RollChance(float chance)
        {
            if (chance < 0f || chance > 100f)
            {
                return false;
            }
            return (random.NextDouble() * 100.0) < chance;
        }

        public static int RangeInt(int min, int max)
        {
            return random.Next(min, max+1); //max is exclusive and i want this to be inclusive
        }
        
        public static double RangeDouble(double min, double max)
        {
            return (random.NextDouble() + min) * max;
        }


        public static LightSourceToy AddLight(Vector3 position, Color clr, float range, float intensity)
        {
            LightSourceToy adminToy;
            LightSourceToy light;
            Dictionary<uint, GameObject>.ValueCollection.Enumerator Enumerator = NetworkClient.prefabs.Values.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                if (Enumerator.Current.TryGetComponent(out adminToy))
                {
                    try
                    {
                        light = UnityEngine.Object.Instantiate(adminToy, position, Quaternion.identity);
                        light.LightColor = clr;
                        light.LightRange = range;
                        light.LightIntensity = intensity;

                        NetworkServer.Spawn(light.gameObject);
                        return light;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{e}");
                    }

                }
            }
            return null;
        }

        public static LightSourceToy AddLight(Transform trans, Color clr, float range, float intensity)
        {
            LightSourceToy adminToy;
            LightSourceToy light;
            Dictionary<uint, GameObject>.ValueCollection.Enumerator Enumerator = NetworkClient.prefabs.Values.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                if (Enumerator.Current.TryGetComponent(out adminToy))
                {
                    try
                    {
                        light = UnityEngine.Object.Instantiate(adminToy, trans);
                        light.LightColor = clr;
                        light.LightRange = range;
                        light.LightIntensity = intensity;

                        NetworkServer.Spawn(light.gameObject);
                        return light;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"{e}");
                    }

                }
            }
            return null;
        }

        public static Dictionary<int, LightSourceToy> lights = new Dictionary<int, LightSourceToy>();

        public static void DeleteLight(string LightSourceName)
        {
            if (ActiveLights.ContainsKey(LightSourceName))
            {
                NetworkServer.Destroy(ActiveLights[LightSourceName].gameObject);
                ActiveLights.Remove(LightSourceName);
            }
        }

        public static int ClassCount(RoleTypeId ClassType)
        {
            int amount = 0;
            foreach (Player plr in Player.GetPlayers())
            {
                if (plr.Role == ClassType) amount++;
            }
            return amount;
        }

        public static List<Player> OfClass(RoleTypeId ClassType)
        {
            List<Player> list = new List<Player>();
            foreach (Player plr in Player.GetPlayers())
            {
                if (plr.Role == ClassType) list.Add(plr);
            }
            return list;
        }

        public static void SetScale(this ReferenceHub player, Vector3 scale)
        {
            try
            {
                if (player.gameObject.transform.localScale == scale) return;

                player.gameObject.transform.localScale = scale;
                foreach (ReferenceHub plr in ReferenceHub.AllHubs.Where(n => n.authManager.InstanceMode == CentralAuth.ClientInstanceMode.ReadyClient))
                {
                    NetworkServer.SendSpawnMessage(player.networkIdentity, plr.connectionToClient);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static async void KillIf079(Player plr)
        {
            await Task.Delay(60000);
            if (plr.Role == RoleTypeId.Scp079)
            {
                plr.Kill();
            }
        }

        public static PrimitiveObjectToy SpawnPrim(Transform trans, Color clr, PrimitiveType primtype, bool collision = true)
        {
            foreach (GameObject value in NetworkClient.prefabs.Values)
            {
                if (value.TryGetComponent(out PrimitiveObjectToy toy))
                {
                    //instantiate the cube
                    PrimitiveObjectToy prim = UnityEngine.Object.Instantiate(toy, trans.position, Quaternion.Euler(trans.rotation.eulerAngles));
                    prim.PrimitiveType = primtype;
                    prim.MaterialColor = clr;
                    prim.transform.localScale = trans.localScale;

                    //give the cube a collider
                    if (collision)
                    {
                        var cubeCollider = prim.gameObject.AddComponent<BoxCollider>();
                        prim.GetComponent<BoxCollider>().isTrigger = false;
                        prim.GetComponent<BoxCollider>().center = trans.position;
                        prim.GetComponent<BoxCollider>().size = trans.localScale;
                        prim.GetComponent<BoxCollider>().enabled = true;
                    }

                    NetworkServer.Spawn(prim.gameObject);
                    placedPrims.Add(placedPrims.Count, prim);
                    Log.Info("Object spawned!");
                    return prim;
                }
            }
            return null;
        }

      
        public static string JammedText(string input, int replacements)
        {
            if (string.IsNullOrEmpty(input) || replacements <= 0)
            {
                return input;
            }

            System.Random random = new System.Random();
            char[] result = input.ToCharArray();
            int length = input.Length;
            char[] symbols = new char[] { '=', '-', '_', '#' };

            for (int i = 0; i < replacements; i++)
            {
                int index;
                do
                {
                    index = random.Next(length);
                } while (symbols.Contains(result[index]));

                char symbol = symbols[random.Next(symbols.Length)];
                result[index] = symbol;
            }

            return string.Join("", result);
        }

        public static List<char> RandomChars(int amount)
        {
            char[] symbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            List<char> built = new List<char>();
            for (int i = 0; i < amount; i++)
            {
                built.Add(symbols.RandomItem());
            }
            return built;
        }

        public static PrimitiveObjectToy SpawnPrim(Vector3 pos, Vector3 scale, Vector3 rotation, Color clr, PrimitiveType primtype, bool collision = true)
        {
            foreach (GameObject value in NetworkClient.prefabs.Values)
            {
                if (value.TryGetComponent(out PrimitiveObjectToy toy))
                {
                    //instantiate the cube
                    PrimitiveObjectToy prim = UnityEngine.Object.Instantiate(toy, pos, Quaternion.Euler(rotation));
                    prim.PrimitiveType = primtype;
                    prim.MaterialColor = clr;
                    prim.transform.localScale = scale;
                    prim.PrimitiveFlags = PrimitiveFlags.Visible;
                    prim.gameObject.AddComponent<BoxCollider>();
                    prim.GetComponent<BoxCollider>().isTrigger = false;
                    prim.GetComponent<BoxCollider>().center = pos;
                    prim.GetComponent<BoxCollider>().size = scale;
                    prim.GetComponent<BoxCollider>().enabled = true;

                    if (collision) { prim.PrimitiveFlags = PrimitiveFlags.Collidable | PrimitiveFlags.Visible; } else { prim.PrimitiveFlags = PrimitiveFlags.Visible; } 

                    NetworkServer.Spawn(prim.gameObject);
                    placedPrims.Add(placedPrims.Count, prim);
                    Log.Info("Object spawned!");
                    return prim;
                }
            }
            return null;
        }

        public static KeyValuePair<string, T> RandomKVP<T>(Dictionary<string, T> dictionary) where T : class
        {
            // Create an instance of the Random class
            System.Random random = new System.Random();

            // Get the list of keys from the dictionary
            List<string> keys = new List<string>(dictionary.Keys);

            // Select a random key
            string randomKey = keys[random.Next(keys.Count)];

            // Get the corresponding value from the dictionary
            var randomValue = dictionary[randomKey];

            // Return the key-value pair
            return new KeyValuePair<string, T>(randomKey, randomValue);
        }

        public static RoomIdentifier NearestRoom(Vector3 pos)
        {
            float lowest = float.PositiveInfinity;
            RoomIdentifier nearest = null;
            foreach (RoomIdentifier room in RoomIdentifier.AllRoomIdentifiers)
            {
                if (room == null) continue;
                if (Vector3.Distance(pos, room.transform.position) < lowest)
                {
                    lowest = Vector3.Distance(pos, room.transform.position);
                    nearest = room;
                }
            }
            return nearest;
        }

        public static void GeneratePrimitiveEditor()
        {
            Log.Info("Generating primitive editor properly...");
            primitiveEditor = new filesystem("main");
            primitiveEditor.AddChild(new filesystem("create"));
            primitiveEditor.AddChild(new filesystem("destroy"));
            primitiveEditor.AddChild(new filesystem("edit"));
            primitiveEditor.AddChild(new filesystem("export"));
            primitiveEditor.AddChild(new filesystem("finish"));
            primitiveEditor.AddChild(new filesystem("duplicate"));

            primitiveEditor.children["create"].AddChild(new filesystem("_cube"));
            primitiveEditor.children["create"].AddChild(new filesystem("_capsule"));
            primitiveEditor.children["create"].AddChild(new filesystem("_cylinder"));
            primitiveEditor.children["create"].AddChild(new filesystem("_sphere"));
            primitiveEditor.children["create"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["destroy"].AddChild(new filesystem("_confirm"));
            primitiveEditor.children["destroy"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].AddChild(new filesystem("scale"));
            primitiveEditor.children["edit"].AddChild(new filesystem("position"));
            primitiveEditor.children["edit"].AddChild(new filesystem("rotation"));
            primitiveEditor.children["edit"].AddChild(new filesystem("color"));
            primitiveEditor.children["edit"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["export"].AddChild(new filesystem("_export"));
            primitiveEditor.children["export"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["finish"].AddChild(new filesystem("_confirm"));
            primitiveEditor.children["finish"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["duplicate"].AddChild(new filesystem("_confirm"));
            primitiveEditor.children["duplicate"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].children["scale"].AddChild(new filesystem("x"));
            primitiveEditor.children["edit"].children["scale"].AddChild(new filesystem("y"));
            primitiveEditor.children["edit"].children["scale"].AddChild(new filesystem("z"));
            primitiveEditor.children["edit"].children["scale"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].children["position"].AddChild(new filesystem("x"));
            primitiveEditor.children["edit"].children["position"].AddChild(new filesystem("y"));
            primitiveEditor.children["edit"].children["position"].AddChild(new filesystem("z"));
            primitiveEditor.children["edit"].children["position"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].children["rotation"].AddChild(new filesystem("x"));
            primitiveEditor.children["edit"].children["rotation"].AddChild(new filesystem("y"));
            primitiveEditor.children["edit"].children["rotation"].AddChild(new filesystem("z"));
            primitiveEditor.children["edit"].children["rotation"].AddChild(new filesystem(".BACK"));

            gooningtonthethird("scale");
            gooningtonthethird("position");

            primitiveEditor.children["edit"].children["rotation"].children["x"].AddChild(new filesystem("+1"));
            primitiveEditor.children["edit"].children["rotation"].children["x"].AddChild(new filesystem("-1"));
            primitiveEditor.children["edit"].children["rotation"].children["x"].AddChild(new filesystem("+5"));
            primitiveEditor.children["edit"].children["rotation"].children["x"].AddChild(new filesystem("-5"));
            primitiveEditor.children["edit"].children["rotation"].children["x"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].children["rotation"].children["y"].AddChild(new filesystem("+1"));
            primitiveEditor.children["edit"].children["rotation"].children["y"].AddChild(new filesystem("-1"));
            primitiveEditor.children["edit"].children["rotation"].children["y"].AddChild(new filesystem("+5"));
            primitiveEditor.children["edit"].children["rotation"].children["y"].AddChild(new filesystem("-5"));
            primitiveEditor.children["edit"].children["rotation"].children["y"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].children["rotation"].children["z"].AddChild(new filesystem("+1"));
            primitiveEditor.children["edit"].children["rotation"].children["z"].AddChild(new filesystem("-1"));
            primitiveEditor.children["edit"].children["rotation"].children["z"].AddChild(new filesystem("+5"));
            primitiveEditor.children["edit"].children["rotation"].children["z"].AddChild(new filesystem("-5"));
            primitiveEditor.children["edit"].children["rotation"].children["z"].AddChild(new filesystem(".BACK"));

            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_black"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_blue"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_clear"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_cyan"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_gray"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_green"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_magenta"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_red"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_white"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem("_yellow"));
            primitiveEditor.children["edit"].children["color"].AddChild(new filesystem(".BACK"));
            Log.Info("finished generating primitive editor");
        }

        public static void gooningtonthethird(string a)
        {
            primitiveEditor.children["edit"].children[a].children["x"].AddChild(new filesystem("+0.05"));
            primitiveEditor.children["edit"].children[a].children["x"].AddChild(new filesystem("-0.05"));
            primitiveEditor.children["edit"].children[a].children["x"].AddChild(new filesystem("+0.5"));
            primitiveEditor.children["edit"].children[a].children["x"].AddChild(new filesystem("-0.5"));
            primitiveEditor.children["edit"].children[a].children["x"].AddChild(new filesystem(".BACK"));
            
            primitiveEditor.children["edit"].children[a].children["y"].AddChild(new filesystem("+0.05"));
            primitiveEditor.children["edit"].children[a].children["y"].AddChild(new filesystem("-0.05"));
            primitiveEditor.children["edit"].children[a].children["y"].AddChild(new filesystem("+0.5"));
            primitiveEditor.children["edit"].children[a].children["y"].AddChild(new filesystem("-0.5"));
            primitiveEditor.children["edit"].children[a].children["y"].AddChild(new filesystem(".BACK"));
            
            primitiveEditor.children["edit"].children[a].children["z"].AddChild(new filesystem("+0.05"));
            primitiveEditor.children["edit"].children[a].children["z"].AddChild(new filesystem("-0.05"));
            primitiveEditor.children["edit"].children[a].children["z"].AddChild(new filesystem("+0.5"));
            primitiveEditor.children["edit"].children[a].children["z"].AddChild(new filesystem("-0.5"));
            primitiveEditor.children["edit"].children[a].children["z"].AddChild(new filesystem(".BACK"));
        }

        public static List<filesystem> generateListVersion(filesystem current)
        {
            List<filesystem> listver = new List<filesystem>();
            foreach (KeyValuePair<string, filesystem> file in current.children)
            {
                listver.Add(file.Value);
            }
            return listver;

        }
        public static filesystem primitiveEditor;

        public static Dictionary<int, PrimitiveObjectToy> placedPrims = new Dictionary<int, PrimitiveObjectToy>();
        public static List<RoleTypeId> AllScps = new List<RoleTypeId>();
        public static Dictionary<string, LightSourceToy> ActiveLights = new Dictionary<string, LightSourceToy>();
        public static Dictionary<Player, string> PlayerData = new Dictionary<Player, string>();
    }
}