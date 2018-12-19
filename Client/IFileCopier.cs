namespace Copier.Client
{
    public interface IFileCopier
    {
        void CopyFile(CommandOptions options, string fileName);
    }
}