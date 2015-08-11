using System;
using System.Collections.Generic;
using System.Linq;
using Boids.Shared;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Boids.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		Game1 game;
		public override void FinishedLaunching(UIApplication application)
		{
			game = new Game1();
			game.Run();
		}
	}
}

