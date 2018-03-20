using System;

namespace Naylah.Core.DTOs.Mappings.Configuration
{
    public static class DTOMappings
    {
        public static Action ExternalDomainMappings;
        public static bool Registered { get; private set; }
        public static Exception RegisterProblems { get; private set; }


        public static void RegisterMappings()
        {
            try
            {
                ///Register mapps under this...

                #region Mapps Register


                //var typesMappersTypes =
                //    from a in AppDomain.CurrentDomain.GetAssemblies()
                //    from t in a.GetTypes()
                //    let attributes = t.GetCustomAttributes(typeof(DTOMappingAutoDiscoverAttribute), true)
                //    where attributes != null && attributes.Length > 0
                //    select new { Type = t, Attributes = attributes.Cast<DTOMappingAutoDiscoverAttribute>() };

                //foreach (var type in typesMappersTypes)
                //{
                //    var instanceMethod = type.Type.GetMethod("Register");

                //    if (instanceMethod != null)
                //    {
                //        instanceMethod.Invoke(null, null);
                //    }
                //}

                #endregion Mapps Register

                ///Register mapps above this...

                if (ExternalDomainMappings != null)
                {
                    ExternalDomainMappings.Invoke();
                }

                //Mapper.AssertConfigurationIsValid();

                Registered = true;
            }
            catch (Exception ex)
            {
                Registered = false;
                RegisterProblems = ex;
            }
        }
    }
}