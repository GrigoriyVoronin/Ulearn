using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            return (typeof(T)
                    .GetCustomAttributes(typeof(ApiDescriptionAttribute))
                    .FirstOrDefault() as ApiDescriptionAttribute)?
                .Description;
        }

        public string[] GetApiMethodNames()
        {
            return typeof(T).GetMethods()
                .Where(x
                    => x.CustomAttributes
                           .FirstOrDefault(y => y.AttributeType == typeof(ApiMethodAttribute))
                       != null)
                .Select(x => x.Name)
                .ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            return (typeof(T)
                    .GetMethod(methodName)?
                    .GetCustomAttribute(typeof(ApiDescriptionAttribute)) as ApiDescriptionAttribute)?
                .Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            return typeof(T)
                .GetMethod(methodName)?
                .GetParameters()
                .Select(x => x.Name)
                .ToArray();
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            return (typeof(T)
                    .GetMethod(methodName)?
                    .GetParameters()
                    .FirstOrDefault(x => x.Name == paramName)?
                    .GetCustomAttribute(typeof(ApiDescriptionAttribute)) as ApiDescriptionAttribute)?
                .Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var parametr = typeof(T)
                .GetMethod(methodName)?
                .GetParameters()
                .FirstOrDefault(x => x.Name == paramName);
            return parametr == null
                ? new ApiParamDescription {ParamDescription = new CommonDescription(paramName)}
                : CreateParamDescription(parametr, methodName, paramName);
        }

        private ApiParamDescription CreateParamDescription(ParameterInfo parametr, string methodName, string paramName)
        {
            var atrIntValidation = parametr
                .GetCustomAttribute(typeof(ApiIntValidationAttribute)) as ApiIntValidationAttribute;
            var atrRequired = parametr
                .GetCustomAttribute(typeof(ApiRequiredAttribute)) as ApiRequiredAttribute;
            var parametrDescription = new ApiParamDescription
            {
                MaxValue = atrIntValidation?.MaxValue,
                MinValue = atrIntValidation?.MinValue,
                ParamDescription = new CommonDescription
                {
                    Description = GetApiMethodParamDescription(methodName, paramName),
                    Name = paramName
                }
            };
            if (atrRequired != null)
                parametrDescription.Required = atrRequired.Required;

            return parametrDescription;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            return typeof(T)
                .GetMethod(methodName)?
                .GetCustomAttribute(typeof(ApiMethodAttribute)) == null
                ? null
                : new ApiMethodDescription
                {
                    ParamDescriptions = GetApiMethodParamNames(methodName)
                        .Select(x => GetApiMethodParamFullDescription(methodName, x))
                        .ToArray(),
                    MethodDescription = new CommonDescription
                    {
                        Description = GetApiMethodDescription(methodName),
                        Name = methodName
                    },
                    ReturnDescription = GetApiMethodReturnParam(methodName)
                };
        }

        private ApiParamDescription GetApiMethodReturnParam(string methodName)
        {
            var returnParametr = typeof(T)
                .GetMethod(methodName)?
                .ReturnParameter;
            if (returnParametr?.Name == null)
                return null;

            var returnDescription = CreateParamDescription(returnParametr, methodName, returnParametr.Name);
            returnDescription.ParamDescription.Name = returnDescription.ParamDescription.Name == ""
                ? null
                : returnDescription.ParamDescription.Name;
            return returnDescription;
        }
    }
}