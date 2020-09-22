using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient} with username {_username}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            /*bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }*/
            Vector3 _position = _packet.ReadVector3();
            Quaternion _rotation = _packet.ReadQuaternion();
            try
            {
                Server.clients[_fromClient].player.SetInput(_position, _rotation);
            }
            catch(Exception error)
            {

            }
        }

        // Recieve when a player attack other
        public static void PlayerHealth(int _fromClient, Packet _packet)
        {
            int _health = _packet.ReadInt();
            try
            {
                Server.clients[_fromClient].player.SetHealth(_health);
            }
            catch(Exception error)
            {

            }
        }

        public static void PlayerShoot(int _fromClient, Packet _packet)
        {
            // Server.clients[_fromClient].player.SetShoot();
            try
            {
                ServerSend.PlayerShoot(Server.clients[_fromClient].player);
            }
            catch (Exception error)
            {

            }
        }
    }
}