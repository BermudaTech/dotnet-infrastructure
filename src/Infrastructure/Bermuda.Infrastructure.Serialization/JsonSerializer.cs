using Bermuda.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bermuda.Infrastructure.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        public TModel Deserialize<TModel>(string model) where TModel : class
        {
            return JsonConvert.DeserializeObject<TModel>(model);
        }

        public string Serialize(object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public string Serialize(object model, CaseStyleType caseStyleType)
        {
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = caseStyleType == CaseStyleType.CamelCase
                                                 ? new CamelCasePropertyNamesContractResolver()
                                                 : new DefaultContractResolver()
            });
        }

        public string Serialize(object model, bool ignoreReferenceLoopHandling)
        {
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
