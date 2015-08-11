using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace Boids.Shared
{
	public class Game1 : Game
	{
		public static Rectangle ScreenBounds;

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Simulation sim;
		TouchCollection previousTouches;
		TouchCollection currentTouches;

		public Game1() : base()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsFixedTimeStep = false;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			this.IsMouseVisible = true;
			ScreenBounds = Window.ClientBounds;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Texture2D boidTexture = Content.Load<Texture2D>("boid");
			sim = new Simulation(boidTexture);
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			previousTouches = currentTouches;
			currentTouches = TouchPanel.GetState();
			bool shouldActOnTouch = ToggleTapped;
			if(shouldActOnTouch){
//			foreach (var tl in currentTouches){
//				var mousePosition = new Point((int)tl.Position.X, (int)tl.Position.Y);
				sim.ToggleAggregation();
//				sim.ToggleM2 ();
//				sim.ToggleM3();
//				sim.AddBoid(mousePosition);
			}
			sim.Update(gameTime);
			base.Update(gameTime);
		}

		bool ToggleTapped
		{
			get{ return !previousTouches.Any () && currentTouches.Any (); }
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin();
			sim.Draw(spriteBatch);
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}

