// Guids.cs
// MUST match guids.h
using System;

namespace TheDevStop.StudioBash_Package
{
    static class GuidList
    {
        public const string guidStudioBash_PackagePkgString = "84c9f1f6-363b-4fdf-92ba-b49cddef6796";
        public const string guidStudioBash_PackageCmdSetString = "53e06e98-844c-432f-9d77-47c6e2c4ce40";
        public const string guidToolWindowPersistanceString = "6e40be5b-ee19-4f2d-9a93-32f2235c9aed";

        public static readonly Guid guidStudioBash_PackageCmdSet = new Guid(guidStudioBash_PackageCmdSetString);
    };
}