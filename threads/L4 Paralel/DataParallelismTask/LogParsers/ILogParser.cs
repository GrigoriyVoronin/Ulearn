namespace DataParallelismTask.LogParsers
{
    public interface ILogParser
    {
        string[] GetRequestedIdsFromLogFile();
    }
}