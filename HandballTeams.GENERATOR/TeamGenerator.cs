﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HandballTeams.GENERATOR
{
    public class TeamGenerator
    {
        private static readonly Random rnd = new Random();
        private static readonly string[] familyNames = { "Szucsánszki", "Schatzl", "Márton", "Kovacsics", "Háfra", "Klujber", "Bíró" };
        private static readonly string[] firstNames = { "Zita", "Nadine", "Gréta", "Anikó", "Noémi", "Katrin", "Blanka" };
        private static readonly string[] positions = { "RightWing", "LeftWing", "Pivot", "Centre", "Left Back", "Right Back", "Goalie" };
        private static readonly Dictionary<string, string[]> nodes = new Dictionary<string, string[]>()
        {
            { "EN", new string[] { "firstName", "familyName", "position" } },
            { "HU", new string[] { "keresztNev", "vezetekNev", "poszt" } },
            { "FR", new string[] { "preNom", "nom", "poste" } },
        };

        public static XDocument GenerateTeam(int num, string lang)
        {
            XDocument output = new XDocument(new XElement("players"));

            for (int i = 0; i < num; i++)
            {
                foreach (var item in nodes)
                {
                    if (item.Key == lang)
                    {
                        output.Root.Add(new XElement("player", new XAttribute("lang", item.Key),
                            new XElement(item.Value[0], firstNames[rnd.Next(familyNames.Length)]),
                            new XElement(item.Value[1], familyNames[rnd.Next(firstNames.Length)]),
                            new XElement(item.Value[2], positions[rnd.Next(positions.Length)])));
                    }
                }
            }
            return output;
        }
    }
}