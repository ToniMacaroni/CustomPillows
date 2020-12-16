using System.IO;

namespace CustomPillows.Helpers
{
    public static class CommonExtensions
    {
        public static FileInfo GetFile(this DirectoryInfo dir, string name)
        {
            return new FileInfo(Path.Combine(dir.FullName, name));
        }
    }
}
