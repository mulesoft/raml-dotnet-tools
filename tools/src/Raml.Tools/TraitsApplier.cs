using System.Collections.Generic;
using System.Linq;
using Raml.Parser.Expressions;

namespace Raml.Tools
{
    public class TraitsApplier
    {
        public static void ApplyTraitsToMethods(ICollection<Method> methods, IEnumerable<IDictionary<string, Method>> traits, IEnumerable<string> isArray)
        {
            foreach (var @is in isArray)
            {
                if (traits.Any(t => t.ContainsKey(@is)))
                {
                    var trait = traits.First(t => t.ContainsKey(@is))[@is];
                    ApplyTraitToMethods(methods, trait);
                }
            }
        }

        public static void ApplyTraitsToMethod(Method method, IEnumerable<IDictionary<string, Method>> traits, IEnumerable<string> isArray)
        {
            foreach (var @is in isArray)
            {
                if (traits.Any(t => t.ContainsKey(@is)))
                {
                    var trait = traits.First(t => t.ContainsKey(@is))[@is];
                    ApplyTraitToMethod(method, trait);
                }
            }
        }


        private static void ApplyTraitToMethods(ICollection<Method> methods, Method trait)
        {
            foreach (var method in methods)
            {
                ApplyTraitToMethod(method, trait);
            }
        }

        private static void ApplyTraitToMethod(Method method, Method trait)
        {
            if (trait.BaseUriParameters != null)
                ApplyBaseUriParameters(method, trait);

            if (trait.Body != null)
                ApplyBody(method, trait);

            if (trait.Headers != null)
                ApplyHeader(method, trait);

            if (trait.Is != null)
                ApplyIs(method, trait);

            if (trait.Protocols != null)
                ApplyProtocols(method, trait);

            if (trait.QueryParameters != null)
                ApplyQueryParameters(method, trait);

            if (trait.Responses != null)
                ApplyResponses(method, trait);

            if (trait.SecuredBy != null)
                ApplySecuredBy(method, trait);

            //method.Verb = trait.Verb;
        }

        private static void ApplyQueryParameters(Method method, Method trait)
        {
            if (method.QueryParameters == null)
            {
                method.QueryParameters = trait.QueryParameters;
            }
            else
            {
                foreach (var element in trait.QueryParameters.Where(kv => !method.QueryParameters.ContainsKey(kv.Key)))
                {
                    method.QueryParameters.Add(element);
                }
            }
        }

        private static void ApplyProtocols(Method method, Method trait)
        {
            if (method.Protocols == null)
            {
                method.Protocols = trait.Protocols;
            }
            else
            {
                var list = method.Protocols.ToList();
                list.AddRange(trait.Protocols.Where(protocol => method.Protocols.All(p => p != protocol)));
                method.Protocols = list;
            }
        }

        private static void ApplySecuredBy(Method method, Method trait)
        {
            if (method.SecuredBy == null)
            {
                method.SecuredBy = trait.SecuredBy;
            }
            else
            {
                var secured = method.SecuredBy.ToList();
                secured.AddRange(trait.SecuredBy.Where(securedBy => !method.SecuredBy.Contains(securedBy)));
                method.SecuredBy = secured;
            }
        }

        private static void ApplyResponses(Method method, Method trait)
        {
            if (method.Responses == null)
            {
                method.Responses = trait.Responses;
            }
            else
            {
                var responses = method.Responses.ToList();
                responses.AddRange(trait.Responses.Where(response => method.Responses.All(r => r.Code != response.Code)));
                method.Responses = responses;
            }
        }

        private static void ApplyIs(Method method, Method trait)
        {
            if (method.Is == null)
            {
                method.Is = trait.Is;
            }
            else
            {
                var list = method.Is.ToList();
                list.AddRange(trait.Is.Where(@is => !method.Is.Contains(@is)));
                method.Is = list;
            }
        }

        private static void ApplyHeader(Method method, Method trait)
        {
            if (method.Headers == null)
            {
                method.Headers = trait.Headers;
            }
            else
            {
                foreach (var element in trait.Headers.Where(kv => !method.Headers.ContainsKey(kv.Key)))
                {
                    method.Headers.Add(element);
                }
            }
        }

        private static void ApplyBody(Method method, Method trait)
        {
            if (method.Body == null)
            {
                method.Body = trait.Body;
            }
            else
            {
                foreach (var body in trait.Body.Where(kv => !method.Body.ContainsKey(kv.Key)))
                {
                    method.Body.Add(body);
                }
            }
        }

        private static void ApplyBaseUriParameters(Method method, Method trait)
        {
            if (method.BaseUriParameters == null)
            {
                method.BaseUriParameters = trait.BaseUriParameters;
            }
            else
            {
                foreach (var baseUriParameter in trait.BaseUriParameters
                    .Where(baseUriParameter => !method.BaseUriParameters.ContainsKey(baseUriParameter.Key)))
                {
                    method.BaseUriParameters.Add(baseUriParameter);
                }
            }
        }
    }
}