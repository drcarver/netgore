using System.Linq;
using NetGore;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Packets going out from the client / in to the server
    /// </summary>
    public static class ClientPacket
    {
        static readonly PacketWriterPool _writerPool = new PacketWriterPool();

        public static PacketWriter Attack()
        {
            return GetWriter(ClientPacketID.Attack);
        }

        public static PacketWriter BuyFromShop(ShopItemIndex index, byte amount)
        {
            PacketWriter pw = GetWriter(ClientPacketID.BuyFromShop);
            pw.Write(index);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter DropInventoryItem(InventorySlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.DropInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter EndNPCChatDialog()
        {
            return GetWriter(ClientPacketID.EndNPCChatDialog);
        }

        public static PacketWriter GetEquipmentItemInfo(EquipmentSlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.GetEquipmentItemInfo);
            pw.WriteEnum(EquipmentSlotHelper.Instance, slot);
            return pw;
        }

        public static PacketWriter GetInventoryItemInfo(InventorySlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.GetInventoryItemInfo);
            pw.Write(slot);
            return pw;
        }

        static PacketWriter GetWriter(ClientPacketID id)
        {
            PacketWriter pw = _writerPool.Create();
            pw.Write(id);
            return pw;
        }

        public static PacketWriter Jump()
        {
            return GetWriter(ClientPacketID.Jump);
        }

        public static PacketWriter Login(string name, string password)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Login);
            pw.Write(name);
            pw.Write(password);
            return pw;
        }

        public static PacketWriter MoveLeft()
        {
            return GetWriter(ClientPacketID.MoveLeft);
        }

        public static PacketWriter MoveRight()
        {
            return GetWriter(ClientPacketID.MoveRight);
        }

        public static PacketWriter MoveStop()
        {
            return GetWriter(ClientPacketID.MoveStop);
        }

        public static PacketWriter PickupItem(MapEntityIndex mapEntityIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.PickupItem);
            pw.Write(mapEntityIndex);
            return pw;
        }

        public static PacketWriter Ping()
        {
            return GetWriter(ClientPacketID.Ping);
        }

        public static PacketWriter RaiseStat(StatType statType)
        {
            PacketWriter pw = GetWriter(ClientPacketID.RaiseStat);
            pw.WriteEnum(StatTypeHelper.Instance, statType);
            return pw;
        }

        public static PacketWriter Say(string text)
        {
            PacketWriter pw = GetWriter(ClientPacketID.Say);
            pw.Write(text, GameData.MaxClientSayLength);
            return pw;
        }

        public static PacketWriter SelectAccountCharacter(byte index)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SelectAccountCharacter);
            pw.Write(index);
            return pw;
        }

        public static PacketWriter SelectNPCChatDialogResponse(byte responseIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SelectNPCChatDialogResponse);
            pw.Write(responseIndex);
            return pw;
        }

        public static PacketWriter SellInventoryToShop(InventorySlot slot, byte amount)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SellInventoryToShop);
            pw.Write(slot);
            pw.Write(amount);
            return pw;
        }

        public static PacketWriter SetUDPPort(int port)
        {
            PacketWriter pw = GetWriter(ClientPacketID.SetUDPPort);
            pw.Write((ushort)port);
            return pw;
        }

        public static PacketWriter StartNPCChatDialog(MapEntityIndex npcIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.StartNPCChatDialog);
            pw.Write(npcIndex);
            return pw;
        }

        public static PacketWriter StartShopping(MapEntityIndex entityIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.StartShopping);
            pw.Write(entityIndex);
            return pw;
        }

        public static PacketWriter UnequipItem(EquipmentSlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UnequipItem);
            pw.WriteEnum(EquipmentSlotHelper.Instance, slot);
            return pw;
        }

        public static PacketWriter UseInventoryItem(InventorySlot slot)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseInventoryItem);
            pw.Write(slot);
            return pw;
        }

        public static PacketWriter UseSkill(SkillType skillType)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseSkill);
            pw.WriteEnum(SkillTypeHelper.Instance, skillType);
            return pw;
        }

        public static PacketWriter UseWorld(MapEntityIndex useItemIndex)
        {
            PacketWriter pw = GetWriter(ClientPacketID.UseWorld);
            pw.Write(useItemIndex);
            return pw;
        }
    }
}