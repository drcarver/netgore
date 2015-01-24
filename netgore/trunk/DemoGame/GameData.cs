using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Contains static data for the game.
    /// </summary>
    public static class GameData
    {
        /// <summary>
        /// If a User is allowed to move while they have a chat dialog open with a NPC.
        /// </summary>
        public const bool AllowMovementWhileChattingToNPC = false;

        /// <summary>
        /// Maximum number of characters allowed in a single account.
        /// </summary>
        public const byte MaxCharactersPerAccount = 10;

        /// <summary>
        /// Maximum length of a Say packet's string from the client to the server.
        /// </summary>
        public const int MaxClientSayLength = 255;

        /// <summary>
        /// Maximum length of each parameter string in the server's SendMessage.
        /// </summary>
        public const int MaxServerMessageParameterLength = 250;

        /// <summary>
        /// Maximum length of a Say packet's string from the server to the client.
        /// </summary>
        public const int MaxServerSayLength = 500;

        /// <summary>
        /// Maximum length of the Name string used by the server's Say messages.
        /// </summary>
        public const int MaxServerSayNameLength = 60;

        /// <summary>
        /// The maximum number of items allowed in a shop
        /// </summary>
        public const byte MaxShopItems = 6 * 6; // TODO: Use value on shop loading in server. Update value in database.

        /// <summary>
        /// The maximum power of a StatusEffect.
        /// </summary>
        public const ushort MaxStatusEffectPower = 500;

        /// <summary>
        /// The rules for the account names.
        /// </summary>
        public static readonly StringRules AccountName = new StringRules(3, 30, CharType.Alpha | CharType.Numeric);

        /// <summary>
        /// The rules for the account passwords.
        /// </summary>
        public static readonly StringRules AccountPassword = new StringRules(3, 30,
                                                                             CharType.Alpha | CharType.Numeric |
                                                                             CharType.Punctuation);

        /// <summary>
        /// The rules for the character names.
        /// </summary>
        public static readonly StringRules CharacterName = new StringRules(3, 15, CharType.Alpha);

        /// <summary>
        /// Size of the screen (ScreenWidth / ScreenHeight) represented in a Vector2
        /// </summary>
        public static Vector2 ScreenSize = new Vector2(800, 600);

        /// <summary>
        /// Gets the maximum delta time between draws for any kind of drawable component. If the delta time between
        /// draw calls on the component exceeds this value, the delta time should then be reduced to be equal to this value.
        /// </summary>
        public static int MaxDrawDeltaTime
        {
            get { return 100; }
        }

        /// <summary>
        /// Gets the IP address of the server.
        /// </summary>
        public static string ServerIP
        {
            get { return "127.0.0.1"; }
        }

        /// <summary>
        /// Gets the port used by the server for handling pings.
        /// </summary>
        public static int ServerPingPort
        {
            get { return 44446; }
        }

        /// <summary>
        /// Gets the port used by the server for TCP connections.
        /// </summary>
        public static int ServerTCPPort
        {
            get { return 44445; }
        }

        /// <summary>
        /// Gets the number of milliseconds between each World update step. This only applies to the synchronized
        /// physics, not client-side visuals.
        /// </summary>
        public static int WorldPhysicsUpdateRate
        {
            get { return 20; }
        }

        /// <summary>
        /// Gets the default amount of money a Character will pay for buying the given <paramref name="item"/> from
        /// a shop.
        /// </summary>
        /// <param name="item">The item to purchase.</param>
        /// <returns>the default amount of money a Character will pay for buying the given <paramref name="item"/>
        /// from a shop.</returns>
        public static int GetItemBuyValue(ItemEntityBase item)
        {
            return item.Value;
        }

        /// <summary>
        /// Gets the default amount of money a Character will get for selling the given <paramref name="item"/> to
        /// a shop.
        /// </summary>
        /// <param name="item">The item to sell.</param>
        /// <returns>the default amount of money a Character will get for selling the given <paramref name="item"/>
        /// to a shop.</returns>
        public static int GetItemSellValue(ItemEntityBase item)
        {
            return Math.Max(item.Value / 2, 1);
        }

        /// <summary>
        /// Gets if the <paramref name="shopper"/> is close enough to the <paramref name="shopOwner"/> to shop.
        /// </summary>
        /// <param name="shopper">The Entity doing the shopping.</param>
        /// <param name="shopOwner">The Entity that owns the shop.</param>
        /// <returns>True if the <paramref name="shopper"/> is close enough to the <paramref name="shopOwner"/> to
        /// shop; otherwise false.</returns>
        public static bool IsValidDistanceToShop(Entity shopper, Entity shopOwner)
        {
            return shopper.Intersect(shopOwner);
        }

        /// <summary>
        /// Gets the experience required for a given level.
        /// </summary>
        /// <param name="x">Level to check (current level).</param>
        /// <returns>Experience required for the given level.</returns>
        public static int LevelCost(int x)
        {
            return x * 30;
        }

        /// <summary>
        /// Gets the experience required for a given stat level.
        /// </summary>
        /// <param name="x">Stat level to check (current stat level).</param>
        /// <returns>Experience required for the given stat level.</returns>
        public static int StatCost(int x)
        {
            return (x / 10) + 1;
        }

        /// <summary>
        /// Gets if the distance between two points is short enough to allow picking-up.
        /// </summary>
        /// <param name="source">The Entity doing the picking-up.</param>
        /// <param name="target">The Entity to be picked-up.</param>
        /// <returns>True if the <paramref name="source"/> is close enough to the <paramref name="target"/> to
        /// pick it up, otherwise false.</returns>
        public static bool ValidServerPickupDistance(Entity source, Entity target)
        {
            // TODO: Make use of this!
            const float maxDistance = 200.0f;
            float dist = source.Position.QuickDistance(target.Position);
            return dist < maxDistance;
        }
    }
}