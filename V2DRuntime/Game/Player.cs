﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !(WINDOWS_PHONE)
using Microsoft.Xna.Framework.Net;
#endif
using DDW.V2D;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Enums;

namespace V2DRuntime.Game
{
	public class Player : V2DSprite
	{
		public bool isLocal;
		public PlayerIndex gamePadIndex;
#if !(WINDOWS_PHONE)
		public NetworkGamer NetworkGamer;
#endif
		public byte NetworkId;


		public Player(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
		}

        private LivingState livingState;
        public virtual LivingState LivingState
        {
            get
            {
                return livingState;
            }
            set
            {
                livingState = value;
            }
        }
        //private bool isAlive = true;
        //public virtual bool IsAlive { get { return isAlive; } set { isAlive = value; } }
#if !(WINDOWS_PHONE)

		public void WriteNetworkPacket(PacketWriter packetWriter, GameTime gameTime)
		{
			// Send our current time.
			packetWriter.Write((float)gameTime.TotalGameTime.TotalSeconds);
            packetWriter.Write(livingState == LivingState.Alive);
            if (livingState == LivingState.Alive)
			{
				// Send the current state of the tank.
				packetWriter.Write(body.GetPosition().X);
				packetWriter.Write(body.GetPosition().Y);
				packetWriter.Write(body.GetLinearVelocity().X);
				packetWriter.Write(body.GetLinearVelocity().Y);
			}
		}
		protected Vector2 rawPostion = Vector2.Zero;
		protected Vector2 rawVelocity = Vector2.Zero;
		public void ReadNetworkPacket(PacketReader packetReader, GameTime gameTime, TimeSpan latency)
		{
			float packetSendTime = packetReader.ReadSingle();
			bool pIsAlive = packetReader.ReadBoolean();
			if (pIsAlive)
			{
				float px = packetReader.ReadSingle();
				float py = packetReader.ReadSingle();
				float vx = packetReader.ReadSingle();
				float vy = packetReader.ReadSingle();
				rawPostion = new Vector2(px, py);
				rawVelocity = new Vector2(vx, vy);
			}

			//if (enableSmoothing)
			//{
			//    // Start a new smoothing interpolation from our current
			//    // state toward this new state we just received.
			//    previousState = displayState;
			//    currentSmoothing = 1;
			//}
			//else
			//{
			//    currentSmoothing = 0;
			//}

			//// Read what time this packet was sent.
			//float packetSendTime = packetReader.ReadSingle();

			//// Read simulation state from the network packet.
			//simulationState.Position = packetReader.ReadVector2();
			//simulationState.Velocity = packetReader.ReadVector2();
			//simulationState.TankRotation = packetReader.ReadSingle();
			//simulationState.TurretRotation = packetReader.ReadSingle();

			//// Read remote inputs from the network packet.
			//tankInput = packetReader.ReadVector2();
			//turretInput = packetReader.ReadVector2();

			//// Optionally apply prediction to compensate for
			//// how long it took this packet to reach us.
			//if (enablePrediction)
			//{
			//    ApplyPrediction(gameTime, latency, packetSendTime);
			//}
		}
#endif

		public virtual void UpdateLocalPlayer(Microsoft.Xna.Framework.GameTime gameTime)
		{
		}
		public virtual void UpdateRemotePlayer(int framesBetweenPackets, bool enablePrediction)
		{
		}
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			UpdateLocalPlayer(gameTime);
			base.Update(gameTime);
		}
		public override void Initialize()
		{
			base.Initialize();
		}
    }

    public struct PlayerState
    {
        public Vector2 Position;
        public Vector2 LinearVelocity;
    }
}
