namespace QuizPond.Infrastructure
{
    public interface ISessionUtility
    {
        string GetSession(string key);
        void SetSession(string key, string value);
    }
}