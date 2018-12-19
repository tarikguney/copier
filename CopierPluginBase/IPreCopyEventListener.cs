namespace CopierPluginBase
{
    public interface IPreCopyEventListener
    {
        void OnPostCopy(string filePath);
    }
}