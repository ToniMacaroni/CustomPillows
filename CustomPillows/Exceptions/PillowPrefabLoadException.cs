using System;

namespace CustomPillows.Exceptions
{
    internal class PillowLoadException : Exception
    {
        public PillowLoadException(EAssetType type, string message) : base($"[{type}] Loading error: {message}")
        {
        }

        internal enum EAssetType
        {
            Prefab,
            Image
        }
    }
}
