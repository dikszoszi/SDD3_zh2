using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace HandballTeams.DB
{
    public class Player
    {
        private static readonly Random rnd = new Random();
        private static readonly System.Collections.Generic.IEnumerable<PropertyInfo> langnodeProps = typeof(Player).GetProperties().Where(p => p.GetCustomAttributes(typeof(LanguageNodeAttribute), false).Length > 0);

        public Player()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [LanguageNode("FR", "nom")]
        [LanguageNode("EN", "familyName")]
        [LanguageNode("HU", "vezetekNev")]
        [MaxLength(100)]
        [Required]
        public string FamilyName { get; set; }

        [LanguageNode("FR", "preNom")]
        [LanguageNode("EN", "firstName")]
        [LanguageNode("HU", "keresztNev")]
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }

        [LanguageNode("FR", "poste")]
        [LanguageNode("EN", "position")]
        [LanguageNode("HU", "poszt")]
        [MaxLength(100)]
        [Required]
        public string Position { get; set; }

        public int Salary { get; set; }

        public static Player CreateFromNode(System.Xml.Linq.XElement node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node), "Parametre cannot be null.");
            }

            string language = node.Attribute("lang").Value;
            Player newPlayer = new Player();

            foreach (PropertyInfo property in langnodeProps)
            {
                property.SetValue(newPlayer, node.Element(property.GetCustomAttributes<LanguageNodeAttribute>().Single(attr => attr.Language == language).NodeName).Value);
            }

            newPlayer.Salary = rnd.Next(1000, 99999 + 1);
            return newPlayer;
        }

        public override string ToString()
        {
            return $"[ #{this.Id} | {(this.FamilyName).ToUpperInvariant()}, {this.FirstName} | {this.Position} | {this.Salary:N} ]";
        }
    }
}
