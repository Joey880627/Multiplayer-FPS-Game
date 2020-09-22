using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }
        

        private static void SendUDPDataToViewBase(int _exceptClient, Packet _packet)//send to those can see me
        {
            //Vector3 _forward = new Vector3(0, 0, 1);
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    try
                    {
                        //Vector3 A = (Server.clients[_exceptClient].player.position - Server.clients[i].player.position);
                        //Vector3 B = Vector3.Transform(_forward, Server.clients[i].player.rotation);
                        //Console.WriteLine(Math.Acos(Vector3.Dot(A, B) / A.Length() / B.Length()));
                        //Console.WriteLine(A.Length());
                        
                        if (Server.clients[i].player != null && ((Server.clients[_exceptClient].player.position - Server.clients[i].player.position).Length() <= 100 || 1.15 >= Math.Acos(Vector3.Dot((Server.clients[_exceptClient].player.position - Server.clients[i].player.position), Vector3.Transform(Vector3.UnitZ, Server.clients[i].player.rotation)) / (Server.clients[_exceptClient].player.position - Server.clients[i].player.position).Length() / Vector3.Transform(Vector3.UnitZ, Server.clients[i].player.rotation).Length())))
                        {//can see met
                            Server.clients[i].udp.SendData(_packet);
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }
        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);
                SendUDPDataToViewBase(_player.id, _packet);
                //SendUDPDataToAll(_player.id, _packet);
            }
        }

        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.rotation);

                SendUDPDataToAll(_player.id, _packet);
            }
        }

        public static void PlayerHealth(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerHealth))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.health);
                // Console.WriteLine($"Player{_player.id}'s health is {_player.health}");
                SendUDPDataToAll(_player.id, _packet);
            }
        }

        public static void PlayerShoot(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerShoot))
            {
                _packet.Write(_player.id);
                // Console.WriteLine($"Player{_player.id} shoot");
                SendUDPDataToAll(_player.id, _packet);
            }
        }

        public static void PlayerDisconnect(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnect))
            {
                _packet.Write(_player.id);
                // Console.WriteLine($"Player{_player.id} shoot");
                SendUDPDataToAll(_player.id, _packet);
            }
        }

        #endregion
    }
}