using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class Player
    {
        public int id;
        public string username;

        public Vector3 position = new Vector3(0);
        public Quaternion rotation;
        // public int lastHealth = 100;
        public int health = 100;
        public bool shooting = false;

        // private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        //private bool[] inputs;

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;

            //inputs = new bool[4];
        }

        public void Update()
        {
            /*Vector2 _inputDirection = Vector2.Zero;
            if (inputs[0])
            {
                _inputDirection.Y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.Y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.X += 1;
            }
            if (inputs[3])
            {
                _inputDirection.X -= 1;
            }*/

            Move(/*_inputDirection*/);
            Health();
        }

        private void Move(/*Vector2 _inputDirection*/)
        {
            /*Vector3 _forward = Vector3.Transform(new Vector3(0, 0, 1), rotation);
            Vector3 _right = Vector3.Normalize(Vector3.Cross(_forward, new Vector3(0, 1, 0)));

            Vector3 _moveDirection = _right * _inputDirection.X + _forward * _inputDirection.Y;
            position += _moveDirection * moveSpeed;*/

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }

        private void Health()
        {
            ServerSend.PlayerHealth(this);
            /*if (health != lastHealth)
            {
                ServerSend.PlayerHealth(this);
                lastHealth = health;
            }*/
        }

        public void SetInput(Vector3 _position, Quaternion _rotation)
        {
            position = _position;
            rotation = _rotation;
        }
        public void SetHealth(int _health)
        {

            health = _health;
        }

    }
}