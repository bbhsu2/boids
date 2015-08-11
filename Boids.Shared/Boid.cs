using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Boids.Shared
{
	public class Boid
	{
		Texture2D texture;
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector2 Acceleration;

		const float BORDER_OFFSET = 30.0f;
		const float MAX_SPEED = 4f;

		float angle;
		float RotationAngle {
			get {
				//why i have to ADD Math.PI / 2 blows my mind
				float ang = (float)Math.Atan2((float)this.Velocity.Y, (float)this.Velocity.X) + (float)Math.PI / 2;
				if (ang < 0) {
					ang += (float)Math.PI * 2;
				}
				return ang;
			}
		}

		public Boid(Texture2D tex, double randomRot, int x, int y)
		{
			this.texture = tex;
			this.Position = new Vector2(x, y);
			this.angle = (float)(randomRot * 2 * Math.PI);
			this.Velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			this.Acceleration = Vector2.Zero;
		}

		public void Update(GameTime gameTime)
		{
			this.Velocity += this.Acceleration;
			this.Velocity.Normalize();
			this.Velocity *= MAX_SPEED;
			this.Position += Velocity;
			this.CheckBounds();
		}

		public void Draw(SpriteBatch sb)
		{
			Vector2 origin = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2);
			sb.Draw(texture, this.Position, null, Color.Wheat, this.RotationAngle, origin, 1.0f, SpriteEffects.None, 0.0f);
			this.Acceleration = Vector2.Zero;
		}

		public void CheckBounds()
		{
			if (this.Position.X > Game1.ScreenBounds.Width + BORDER_OFFSET) {
				this.Position.X = -BORDER_OFFSET;
			}

			if (this.Position.X < -BORDER_OFFSET) {
				this.Position.X = Game1.ScreenBounds.Width + BORDER_OFFSET;
			}

			if (this.Position.Y > Game1.ScreenBounds.Height + BORDER_OFFSET) {
				this.Position.Y = -BORDER_OFFSET;
			}

			if (this.Position.Y < -BORDER_OFFSET) {
				this.Position.Y = Game1.ScreenBounds.Height + BORDER_OFFSET;
			}
		}
	}

}

