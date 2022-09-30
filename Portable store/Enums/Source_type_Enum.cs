namespace Portable_store.Enums
{
    public enum Source_type_Enum
    {
        /// <summary> Application inside a GitHub repository </summary>
        GitHub,
        /// <summary> Application inside a SourceForge repository </summary>
        SourceForge,
        /// <summary> Redirection link to the application </summary>
        RedirectLink,
        /// <summary> Direct link to the application </summary>
        DirectLink,
        /// <summary> Application inside a portable setup wizard </summary>
        PortableSetupWizard,
        /// <summary> Application inside a setup wizard </summary>
        SetupWizard
    }
}
