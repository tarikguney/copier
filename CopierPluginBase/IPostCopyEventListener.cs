namespace CopierPluginBase
{
    public interface IPostCopyEventListener
    {
        void OnPostCopy(string filePath);
    }
}