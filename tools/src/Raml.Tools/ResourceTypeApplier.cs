using System;
using System.Collections.Generic;
using System.Linq;
using Raml.Parser.Expressions;

namespace Raml.Tools
{
    public static class ResourceTypeApplier
    {
        public static void Apply(IEnumerable<IDictionary<string, ResourceType>> resourceTypes, string type,
                ICollection<Method> methods, IEnumerable<IDictionary<string, Method>> traits, Resource resource, string defaultMediaType)
        {
            if (type == null || !resourceTypes.Any(t => t.ContainsKey(type)))
            {
                type = resource.GetSingleType();
            }

            ApplyToMethods(resourceTypes, methods, type, traits, defaultMediaType);

            if (type == null || !resourceTypes.Any(t => t.ContainsKey(type)))
                return;

            var resourceType = resourceTypes.First(t => t.ContainsKey(type))[type];

            if (resourceType.UriParameters != null)
            {
                if (resource.UriParameters == null)
                    resource.UriParameters = new Dictionary<string, Parameter>();

                foreach (var uriParameter in resourceType.UriParameters.Where(u => !resource.UriParameters.ContainsKey(u.Key)))
                {
                    resource.UriParameters.Add(uriParameter);
                }
            }
        }

        private static void ApplyToMethods(IEnumerable<IDictionary<string, ResourceType>> resourceTypes, ICollection<Method> methods, string type,
            IEnumerable<IDictionary<string, Method>> traits, string defaultMediaType)
        {
            if (type == null || !resourceTypes.Any(t => t.ContainsKey(type)))
                return;

            var resourceType = resourceTypes.First(t => t.ContainsKey(type))[type];

            // handle traits
            TraitsApplier.ApplyTraitsToMethods(methods, traits, resourceType.Is);

            AddOrApplyToMethod(methods, "get", resourceType.Get, defaultMediaType);

            AddOrApplyToMethod(methods, "delete", resourceType.Delete, defaultMediaType);

            AddOrApplyToMethod(methods, "options", resourceType.Options, defaultMediaType);

            AddOrApplyToMethod(methods, "patch", resourceType.Patch, defaultMediaType);

            AddOrApplyToMethod(methods, "post", resourceType.Post, defaultMediaType);

            AddOrApplyToMethod(methods, "put", resourceType.Put, defaultMediaType);

            // handle nested resource type
            ApplyToMethods(resourceTypes, methods, resourceType.Type, traits, defaultMediaType);
        }

        private static void AddOrApplyToMethod(ICollection<Method> methods, string methodVerb, Verb resourceTypeVerb, string defaultMediaType)
        {
            if (resourceTypeVerb != null && !methods.Any(m => methodVerb.Equals(m.Verb, StringComparison.OrdinalIgnoreCase)))
            {
                methods.Add(GetMethod(resourceTypeVerb, defaultMediaType));
            }
            else if (resourceTypeVerb != null)
            {
                var method = methods.First(m => methodVerb.Equals(m.Verb, StringComparison.OrdinalIgnoreCase));
                ApplyToMethod(method, resourceTypeVerb, defaultMediaType);
            }
        }

        private static void ApplyToMethod(Method method, Verb resourceTypeVerb, string defaultMediaType)
        {
            if (resourceTypeVerb.Body != null)
            {
                ApplyToMethod(method, resourceTypeVerb.Body, defaultMediaType);
            }
 
            if (resourceTypeVerb.Headers != null)
            {
                if(method.Headers == null)
                    method.Headers = new Dictionary<string, Parameter>();
                
                foreach (var header in resourceTypeVerb.Headers.Where(header => !method.Headers.ContainsKey(header.Key)))
                {
                    method.Headers.Add(header);
                }
            }

            if (resourceTypeVerb.Responses != null)
            {
                if(method.Responses == null)
                    method.Responses = new List<Response>();

                foreach (var response in resourceTypeVerb.Responses)
                {
                    if (method.Responses.Any(r => r.Code == response.Code))
                    {
                        var resp = method.Responses.First(r => r.Code == response.Code);
                        if (response.Body != null)
                        {
                            if(resp.Body == null)
                                resp.Body = new Dictionary<string, MimeType>();

                            foreach (var mimeType in response.Body.Where(mimeType => !resp.Body.ContainsKey(mimeType.Key)))
                            {
                                resp.Body.Add(mimeType);
                            }
                        }

                        if (response.Headers != null)
                        {
                            if(resp.Headers == null)
                                resp.Headers = new Dictionary<string, Parameter>();

                            foreach (var header in response.Headers.Where(header => !resp.Headers.ContainsKey(header.Key)))
                            {
                                resp.Headers.Add(header);
                            }
                        }
                        if (string.IsNullOrWhiteSpace(resp.Description))
                            resp.Description = resourceTypeVerb.Description;
                    }
                    else
                    {
                        var responses = method.Responses.ToList();
                        responses.Add(response);
                        method.Responses = responses;                        
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(method.Description))
                method.Description = resourceTypeVerb.Description;

            if (resourceTypeVerb.QueryParameters != null)
            {
                if(method.QueryParameters == null)
                    method.QueryParameters = new Dictionary<string, Parameter>();

                foreach (var queryParameter in resourceTypeVerb.QueryParameters)
                {
                    method.QueryParameters.Add(queryParameter);
                }
            }
        }

        private static void ApplyToMethod(Method method, MimeType body, string defaultMediaType)
        {
            if (method.Body == null)
            {
                method.Body = new Dictionary<string, MimeType>();
                if(!string.IsNullOrWhiteSpace(defaultMediaType))
                    method.Body.Add(defaultMediaType, body);
                return;
            }

            if (method.Body.Count == 1)
            {
                ApplyToMethod(body, method.Body.First().Value);
                return;
            }

            if (string.IsNullOrWhiteSpace(defaultMediaType) || !method.Body.ContainsKey(defaultMediaType)) 
                return;

            ApplyToMethod(body, method.Body[defaultMediaType]);
        }

        private static void ApplyToMethod(MimeType body, MimeType methodBody)
        {
            if (string.IsNullOrWhiteSpace(methodBody.Schema))
                methodBody.Schema = body.Schema;

            if (string.IsNullOrWhiteSpace(methodBody.Type))
                methodBody.Type = body.Type;

            if (string.IsNullOrWhiteSpace(methodBody.Description))
                methodBody.Description = body.Description;

            if (string.IsNullOrWhiteSpace(methodBody.Example))
                methodBody.Example = body.Example;

            if (body.Annotations == null) return;

            if (methodBody.Annotations == null)
                methodBody.Annotations = new Dictionary<string, object>();

            foreach (var annotation in body.Annotations.Where(annotation => !methodBody.Annotations.ContainsKey(annotation.Key)))
            {
                methodBody.Annotations.Add(annotation);
            }
        }

        private static Method GetMethod(Verb verb, string defaultMediaType)
        {
            if (string.IsNullOrWhiteSpace(defaultMediaType))
                defaultMediaType = "application/json";

            return new Method
            {
                Verb = verb.Type.ToString().ToLowerInvariant(),
                Body = new Dictionary<string, MimeType> { { defaultMediaType, verb.Body } },
                Description = verb.Description,
                Headers = verb.Headers,
                Responses = verb.Responses
            };
        }

    }

}