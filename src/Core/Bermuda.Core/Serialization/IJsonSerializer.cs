namespace Bermuda.Core.Serialization
{
    public interface IJsonSerializer
    {
        TModel Deserialize<TModel>(string model) where TModel : class;

        string Serialize(object model);

        string Serialize(object model, CaseStyleType caseStyleType);

        string Serialize(object model, bool ignoreReferenceLoopHandling);
    }

    public enum CaseStyleType
    {
        PascalCase,
        CamelCase
    }
}
