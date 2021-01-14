using System;

namespace HandballTeams.DB
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class LanguageNodeAttribute : Attribute
    {
        public LanguageNodeAttribute(string Language, string NodeName)
        {
            this.Language = Language;
            this.NodeName = NodeName;
        }

        public string Language { get; }

        public string NodeName { get; }
    }
}
