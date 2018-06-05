﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gridly
{
    public class Neuron
    {
        public Vector2 Position { get; private set; }
        private static Vector2 Origin = Resources.NeuronTexture.Bounds.Size.ToVector2() / 2;

        private List<Neuron> connecting;

        public bool Activated { get; private set; }
        private bool shouldActivate = false;

        public Neuron(Vector2 pos)
        {
            Position = pos;
            connecting = new List<Neuron>();
            Activated = false;
        }

        public void UpdateSynapse()
        {
            if (Activated)
                foreach (var n in connecting)
                    n.Activate();
            Activated = false;
        }

        public void UpdateState()
        {
            if (shouldActivate)
            {
                Activated = true;
                shouldActivate = false;
            }
        }

        public Rectangle GetBounds()
            => new Rectangle((Position - Origin).ToPoint(), Resources.NeuronTexture.Bounds.Size);

        public void Draw(SpriteBatch sb)
        {
            foreach (var n in connecting)
                GUI.DrawLine(sb, Position, n.Position, 1f, Color.Blue);
            sb.Draw(
                Resources.NeuronTexture,
                position: Position,
                origin: Origin,
                color: Activated ? Color.Blue : Color.White,
                layerDepth: .5f);
        }

        public void ConnectTo(Neuron n)
        {
            if (!connecting.Contains(n))
                connecting.Add(n);
        }

        public void Activate()
        {
            shouldActivate = true;
        }

        public void ActivateImmediate()
        {
            Activated = true;
        }
    }
}