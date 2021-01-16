using HandballTeams.DB;
using HandballTeams.GENERATOR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

[assembly: CLSCompliant(false)]
namespace HandballTeams
{
    internal class Program
    {
        private static void Main()
        {
            PlayerContext ctx = new PlayerContext();
            ProcessXmls(ctx);
            QueryDatabase(ctx);
            ctx.Dispose();
        }

        private static void ProcessXmls(PlayerContext ctx)
        {
            XDocument teamEN = TeamGenerator.GenerateTeam(28, "EN");
            XDocument teamHU = TeamGenerator.GenerateTeam(40, "HU");
            XDocument teamFR = TeamGenerator.GenerateTeam(2, "FR");

            teamEN.Save(nameof(teamEN) + ".xml");
            teamHU.Save(nameof(teamHU) + ".xml");
            teamFR.Save(nameof(teamFR) + ".xml");

            IEnumerable<XElement> allPlayers = teamEN.Root.Elements("player")
                .Concat(teamHU.Root.Elements("player"))
                .Concat(teamFR.Root.Elements("player"));

            IEnumerable<Player> players = allPlayers.Select(playerElement => Player.CreateFromNode(playerElement));

            foreach (Player player in players)
            {
                ctx.Set<Player>().Add(player);
            }
            ctx.SaveChanges();
        }

        private static void QueryDatabase(PlayerContext ctx)
        {
            var q1 = new string[]
            {
                "Number of players: " + ctx.Players.Count(),
                "Details of the first player: " + ctx.Players.OrderBy(player => player.Id).First().ToString(),
                "Details of the last player: " + ctx.Players.OrderBy(player => player.Id).Last().ToString(),
            };
            q1.PrintToConsole("Q1"); //1. The number of players, all data of the first and last players 

            var groupbyPos = ctx.Players.GroupBy(player => player.Position);

            var q2 = from grp in groupbyPos
                     select new
                     {
                         Position = grp.Key,
                         Average = grp.Average(player => player.Salary),
                         Maximum = grp.Max(player => player.Salary)
                     };
            q2.PrintToConsole("Q2"); //2. The maximum and the average salary for every position 

            var q3 = from grp in groupbyPos
                     let posMaxSal = new { Position = grp.Key, MaxSalary = grp.Max(player => player.Salary) }
                     join player in ctx.Players on posMaxSal.MaxSalary equals player.Salary
                     select new { posMaxSal.Position, posMaxSal.MaxSalary, player };
            q3.PrintToConsole("Q3"); //3. For every position, the players who earn the most

            var q4 = from p1 in ctx.Players
                     join p2 in ctx.Players on p1.Position equals p2.Position
                     let playersSum = new { p1, p2, Sum = p1.Salary + p2.Salary }
                     where p1.Id < p2.Id && p1.Salary - p2.Salary < 1000
                     orderby playersSum.Sum descending
                     select playersSum;
            q4.Take(10).PrintToConsole("Q4"); //4. The post-pairs with close salaries: ordered by the descending order of salaries, we want to see the top 10 player - pair, who play in the same position, and the difference between their salaries is smaller than 1000. The result should be a collection of anonymous instances that contain both players and the third property is the addition of the two salaries.

            var q5 = from q in q4
                     group q by q.p1.Position into grp
                     let MaxPerPos = new { Pos = grp.Key, Max = grp.Max(query => query.Sum) }
                     join q in q4 on MaxPerPos.Max equals q.Sum
                     select new { MaxPerPos.Pos, MaxPerPos.Max, q };
            q5.PrintToConsole("Q5"); //5. For every position, the most expensive post-pair

            XDocument teamQ2result = new XDocument(new XElement("Results"));
            foreach (var item in q2)
            {
                teamQ2result.Root.Add(new XElement("Position", new XAttribute("name", item.Position),
                    new XElement("avg", item.Average),
                    new XElement("max", item.Maximum)));
            }
            teamQ2result.Save(nameof(teamQ2result) + ".xml");
        }
    }
}
