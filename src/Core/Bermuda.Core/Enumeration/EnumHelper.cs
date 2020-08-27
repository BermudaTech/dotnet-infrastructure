using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bermuda.Core.Enumeration
{
    public static class EnumHelper
    {
        public static List<EnumModel> GetEnums<TEnum>()
        {
            List<EnumModel> enumModels = new List<EnumModel>();

            Type type = typeof(TEnum);

            Array values = System.Enum.GetValues(type);

            foreach (int val in values)
            {
                var memInfo = type.GetMember(type.GetEnumName(val));

                var attribute = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                if (attribute == null)
                {
                    continue;
                }

                enumModels.Add(new EnumModel
                {
                    Value = val,
                    Description = attribute.Description
                });
            }

            return enumModels;
        }

        public static string GetEnumDescription<TEnum>(this TEnum enumValue) where TEnum : IConvertible
        {
            return GetEnums<TEnum>().Where(x => x.Value == (int)enumValue.xToInt()).FirstOrDefault()?.Description;
        }
    }
}
