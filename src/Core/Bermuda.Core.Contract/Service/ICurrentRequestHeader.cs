using System.Globalization;

namespace Bermuda.Core.Contract.Service
{
    public interface ICurrentRequestHeader
    {
        string GetCurrentLang();
        RequestBase GetRequestBase();
        CultureInfo GetCurrentCultureInfo();
    }
}
