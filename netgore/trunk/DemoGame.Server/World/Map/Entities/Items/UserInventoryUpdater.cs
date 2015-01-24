using System;
using System.Collections;
using System.Linq;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles tracking which slots of the UserInventory have changed, along with notifying the User of the
    /// new information for their inventory items.
    /// </summary>
    class UserInventoryUpdater
    {
        readonly BitArray _slotChanged = new BitArray(CharacterInventory.MaxInventorySize);

        readonly UserInventory _userInventory;

        /// <summary>
        /// UserInventoryUpdater constructor.
        /// </summary>
        /// <param name="userInventory">UserInventory that this UserInventoryUpdater will manage.</param>
        public UserInventoryUpdater(UserInventory userInventory)
        {
            if (userInventory == null)
                throw new ArgumentNullException("userInventory");

            _userInventory = userInventory;
        }

        /// <summary>
        /// Gets the User to send the updates to.
        /// </summary>
        User OwnerUser
        {
            get { return UserInventory.Character as User; }
        }

        /// <summary>
        /// Gets the UserInventory that this UserInventoryUpdater manages.
        /// </summary>
        public UserInventory UserInventory
        {
            get { return _userInventory; }
        }

        /// <summary>
        /// Lets the UserInventoryUpdater know that an Inventory slot has changed and needs to be updated.
        /// </summary>
        /// <param name="slot">Slot that changed.</param>
        public void SlotChanged(InventorySlot slot)
        {
            _slotChanged[(int)slot] = true;
        }

        /// <summary>
        /// Updates the client with the changes that have been made to the UserInventory.
        /// </summary>
        public void Update()
        {
            // Don't actually grab the PacketWriter from the pool until we know we will need it
            PacketWriter pw = null;

            try
            {
                // Loop through all slots
                for (InventorySlot slot = new InventorySlot(0); slot < CharacterInventory.MaxInventorySize; slot++)
                {
                    // Skip unchanged slots
                    if (!_slotChanged[(int)slot])
                        continue;

                    // Get the item in the slot
                    ItemEntity item = UserInventory[slot];

                    // Get the values to send, which depends on if the slot is empty (item == null) or not
                    GrhIndex sendItemGraphic;
                    byte sendItemAmount;

                    if (item == null)
                    {
                        sendItemGraphic = new GrhIndex(0);
                        sendItemAmount = 0;
                    }
                    else
                    {
                        sendItemGraphic = item.GraphicIndex;
                        sendItemAmount = item.Amount;
                    }

                    // Grab the PacketWriter if we have not already, or clear it if we have
                    if (pw == null)
                        pw = ServerPacket.GetWriter();
                    else
                        pw.Reset();

                    // Pack the data and send it
                    ServerPacket.SetInventorySlot(pw, slot, sendItemGraphic, sendItemAmount);
                    OwnerUser.Send(pw);
                }
            }
            finally
            {
                // If we grabbed a PacketWriter, make sure we dispose of it!
                if (pw != null)
                    pw.Dispose();
            }

            // Changes complete
            _slotChanged.SetAll(false);
        }
    }
}