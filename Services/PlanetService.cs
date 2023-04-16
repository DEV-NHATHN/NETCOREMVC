using System.Collections.Generic;
using App.Models;

namespace App.Services
{
   public class PlanetService
   {
      public List<PlanetModel> GetPlanets()
      {
         return new List<PlanetModel>
         {
            new PlanetModel { Id = "1", Name = "Mercury" },
            new PlanetModel { Id = "2", Name = "Venus" },
            new PlanetModel { Id = "3", Name = "Earth" },
            new PlanetModel { Id = "4", Name = "Mars" },
            new PlanetModel { Id = "5", Name = "Jupiter" },
            new PlanetModel { Id = "6", Name = "Saturn" },
            new PlanetModel { Id = "7", Name = "Uranus" },
            new PlanetModel { Id = "8", Name = "Neptune" },
            new PlanetModel { Id = "9", Name = "Pluto" }
         };
      }
   }
}