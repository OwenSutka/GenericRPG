using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameLibrary {
  // This is the enemy class. It was used by Cherry to determine and use the enemies. Needs to be completely redone.
  public class Enemy : Mortal {
    // Constants
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private const float MAX_XP_DROP = 35;
    private const float MIN_XP_DROP = 15;
    private const float WEAKEN_MIN = 1.25f;
    private const float WEAKEN_MAX = 1.85f;
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // getters and setters
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Bitmap Img { get; private set; }
    public float XpDropped { get; private set; }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // readonlys for random and names
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private static readonly Random rand = new Random();
    private static readonly List<string> names = new List<string>() {
      "Wily", "Bob", "Dr. Light", "WallCrusher"
    };
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    // this is the enemy creator. Need to add a link to the map. Sets all the stats and img
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Enemy(int level, Bitmap img) : base(RandName(), level) {
      Img = img;

      // weaken so player has a chance
      Health /= (float)rand.NextDouble() * (WEAKEN_MAX - WEAKEN_MIN) + WEAKEN_MIN;
      Mana /= (float)rand.NextDouble() * (WEAKEN_MAX - WEAKEN_MIN) + WEAKEN_MIN;
      Str /= (float)rand.NextDouble() * (WEAKEN_MAX - WEAKEN_MIN) + WEAKEN_MIN;
      Def /= (float)rand.NextDouble() * (WEAKEN_MAX - WEAKEN_MIN) + WEAKEN_MIN;

      XpDropped = (float)rand.NextDouble() * (MAX_XP_DROP - MIN_XP_DROP) + MIN_XP_DROP;
    }


    // sets random name to the creature
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static string RandName() {
      return names[rand.Next(names.Count)];
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  }
}
