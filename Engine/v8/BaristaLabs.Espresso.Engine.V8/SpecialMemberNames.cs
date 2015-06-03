namespace BaristaLabs.Espresso.Engine.V8
{
    internal static class SpecialDispIDs
    {
        public const int Default = 0;
        public const int Unknown = -1;
        public const int PropertyPut = -3;
        public const int NewEnum = -4;
        public const int This = -613;
    }

    internal static class SpecialMemberNames
    {
        public static string Default = MiscHelpers.GetDispIDName(SpecialDispIDs.Default);
        public static string NewEnum = MiscHelpers.GetDispIDName(SpecialDispIDs.NewEnum);
    }
}
