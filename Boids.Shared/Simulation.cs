using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boids.Shared
{
	public class Simulation
	{
		public const int DESIRED_SEPARATION = 700;
		public const int NEIGHBOR_DISTANCE = 100;
		public const float BORDER_OFFSET = 3.0f;
		List<Boid> Boids = new List<Boid>();
		Random rnd = new Random();
		Texture2D boidTexture;
		//scaling factors for rules
		int m1 = 1;
		int m2 = 1;
		int m3 = 1;

		public Simulation(Texture2D tex)
		{
			this.boidTexture = tex;
			for (int i = 0; i < 50; i++) {
				AddBoid();
			}
		}

		public void AddBoid()
		{
			var x = rnd.Next(Game1.ScreenBounds.Width / 2, Game1.ScreenBounds.Width / 2);
			var y = rnd.Next(Game1.ScreenBounds.Height / 2, Game1.ScreenBounds.Height / 2);
			//var x = rnd.Next(Game1.ScreenBounds.Width / 2 - 100, Game1.ScreenBounds.Width / 2 + 200);
			//var y = rnd.Next(Game1.ScreenBounds.Height / 2 - 100, Game1.ScreenBounds.Height / 2 + 200);
			Boid boid = new Boid(this.boidTexture, rnd.NextDouble(), x, y);
			Boids.Add(boid);
		}

		public void AddBoid(Point point)
		{
			Boid boid = new Boid(this.boidTexture, 0.0, point.X, point.Y);
			Boids.Add(boid);
		}

		//Fly to the center of mass of other boids
		public Vector2 Rule1(Boid boid, bool isSimple)
		{
			Vector2 pcj = Vector2.Zero;
			int neighborCount = 0;
			foreach (Boid b in Boids) {
				if (boid != b && (b.Position-boid.Position).Length() < 700) {
					pcj += b.Position;
					neighborCount++;
				}
			}
			pcj /= neighborCount + 1; //(Boids.Count - 1);
			return (pcj - boid.Position) * 0.01f;
		}

		//Keep a small distance away from other objects
		public Vector2 Rule2(Boid boid)
		{
			Vector2 vec = Vector2.Zero;
			int neighborCount = 0;
			foreach (Boid b in Boids) {
				if (b != boid) {
					var distance = (boid.Position - b.Position).Length();
					if (0 < distance && distance < DESIRED_SEPARATION) {
						var deltaVector = boid.Position - b.Position;
						deltaVector.Normalize();
						deltaVector /= distance;
						vec += deltaVector;
						neighborCount++;
					}
				}
			}
			Vector2 averageSteeringVector = (neighborCount > 0) ? vec / neighborCount : Vector2.Zero;
			return averageSteeringVector;
		}

		//Maintain an average speed equal to that of the other boids
		public Vector2 Rule3(Boid boid)
		{
			Vector2 pvj = Vector2.Zero;
			foreach (Boid b in Boids) {
				if (boid != b) {
					pvj += b.Velocity;
				}
			}
			pvj /= (Boids.Count - 1);
			return (pvj - boid.Velocity) / 8;
		}

		public void ToggleAggregation()
		{
			m1 = (m1 == 1) ? -1 : 1;
		}

		public void ToggleM2()
		{
			m2 = (m2 == 1) ? -1 : 1;
		}

		public void ToggleM3()
		{
			m3 = (m3 == 1) ? -1 : 1;
		}

		public void Update(GameTime gameTime)
		{
			Boids.ForEach((f) => {
				Vector2 v1 = m1 * Rule1(f, true);
				Vector2 v2 = m2 * Rule2(f) * DESIRED_SEPARATION;
				Vector2 v3 = m3 * Rule3(f);
				f.Acceleration = v1 + v2 + v3;
				f.Update(gameTime);
			});
		}

		public void UpdateWithoutAccel(GameTime gameTime)
		{
			Boids.ForEach((f) => {
				f.Update(gameTime);
			});
		}

		public void Draw(SpriteBatch sb)
		{
			Boids.ForEach((f) => {
				f.Draw(sb);
			});
		}
	}
}