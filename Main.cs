using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Map = Exiled.Events.Handlers.Map;
using Server = Exiled.Events.Handlers.Server;
using MEC;
using UnityEngine;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using Exiled.API.Features.Items;

namespace EnderPearl
{
    public class Main : Plugin<Config>
    {
        public static Main Instance { get; private set; }
        public override string Name => "EnderPearl";
        public override string Author => "Admiro";
        public override string Prefix => "EnderPearl";
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion => new Version(8, 7, 3);
        public override PluginPriority Priority => PluginPriority.Medium;

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            UnRegisterEvents();
            base.OnDisabled();
        }

        public void RegisterEvents()
        {
            Map.PickupAdded += OnDroppingItem;
            Server.RestartingRound += OnRoundRestart;
        }
        public void UnRegisterEvents()
        {
            Map.PickupAdded -= OnDroppingItem;
            Server.RestartingRound -= OnRoundRestart;
        }

        public static List<Player> ItemTeleportPlayers = new List<Player>()
        {

        };

        public void OnDroppingItem(PickupAddedEventArgs ev)
        {

            if (ItemTeleportPlayers.Contains(ev.Pickup.PreviousOwner))
            {
                Timing.CallDelayed(0.4f, () =>
                {
                    ev.Pickup.GameObject.AddComponent<NewBehaviourScript>();
                });
            }
        }

        public void OnRoundRestart()
        {
            ItemTeleportPlayers.Clear();
        }

    }

    public class NewBehaviourScript : MonoBehaviour
    {
        bool IsGrounded()
        {
            return Physics.Raycast(this.gameObject.transform.position, Vector3.down, 0.2f);
        }
        void Update()
        {
            if (IsGrounded())
            {
                Pickup pickup = Pickup.Get(this.gameObject);
                Item item = pickup.PreviousOwner.AddItem(pickup.Type);
                pickup.PreviousOwner.CurrentItem = item;
                pickup.PreviousOwner.Position = new Vector3(pickup.Position.x, pickup.Position.y + 1f, pickup.Position.z);
                pickup.Destroy();

            }
        }

    }
}

