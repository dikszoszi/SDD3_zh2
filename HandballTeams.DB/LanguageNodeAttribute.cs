using System;

namespace HandballTeams.DB
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LanguageNodeAttribute : Attribute
    {
        public LanguageNodeAttribute(string Language, string NodeName)
        {
            this.Language = Language;
            this.NodeName = NodeName;
        }

        public string Language { get; set; }

        public string NodeName { get; set; }
    }
}
