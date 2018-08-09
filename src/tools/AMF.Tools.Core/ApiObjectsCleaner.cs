using System;
using System.Collections.Generic;
using System.Linq;
using AMF.Tools.Core.ClientGenerator;

namespace AMF.Tools.Core
{
    public class ApiObjectsCleaner
    {
        private readonly IDictionary<string, ApiObject> schemaRequestObjects;
        private readonly IDictionary<string, ApiObject> schemaResponseObjects;
        private readonly IDictionary<string, ApiObject> schemaObjects;

        public ApiObjectsCleaner(IDictionary<string, ApiObject> schemaRequestObjects, IDictionary<string, ApiObject> schemaResponseObjects, 
            IDictionary<string, ApiObject> schemaObjects)
        {
            this.schemaRequestObjects = schemaRequestObjects;
            this.schemaResponseObjects = schemaResponseObjects;
            this.schemaObjects = schemaObjects;
        }

        public void CleanObjects(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, IDictionary<string, ApiObject> objects, 
            Func<IEnumerable<Core.WebApiGenerator.ControllerObject>, ApiObject, bool> checkAction)
        {
            var keys = objects.Keys.ToArray();
            foreach (var key in keys)
            {
                var apiObject = objects[key];

                if (!string.IsNullOrWhiteSpace(apiObject.GeneratedCode))
                    continue;

                if (checkAction(controllers, apiObject))
                    continue;

                if (IsUsedAsReferenceInAnyObject(apiObject))
                    continue;

                objects.Remove(key);
            }
        }

        public void CleanObjects(IEnumerable<ClassObject> classes, IDictionary<string, ApiObject> objects, Func<IEnumerable<ClassObject>, ApiObject, bool> checkAction)
        {
            var keys = objects.Keys.ToArray();
            foreach (var key in keys)
            {
                var apiObject = objects[key];
                
                if(!string.IsNullOrWhiteSpace(apiObject.GeneratedCode))
                    continue;

                if (checkAction(classes, apiObject))
                    continue;

                if (IsUsedAsReferenceInAnyObject(apiObject))
                    continue;

                objects.Remove(key);
            }
        }

        public bool IsUsedAnywhere(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, ApiObject obj)
        {
            return IsUsedAsParameterInAnyMethod(controllers, obj) || IsUsedAsResponseInAnyMethod(controllers, obj)
                || IsUsedAsReferenceInAnyObject(obj) || IsUsedAsQueryOrUriParameter(controllers, obj);
        }

        private bool IsUsedAsQueryOrUriParameter(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, ApiObject obj)
        {
            return controllers.Any(c => c.Methods.Any(m => (m.QueryParameters != null && m.QueryParameters.Any(qp => qp.Type == obj.Type)) 
            || (m.UriParameters != null && m.UriParameters.Any(up => up.Type == obj.Type))));
        }


        public bool IsUsedAnywhere(IEnumerable<Core.ClientGenerator.ClassObject> classes, ApiObject obj)
        {
            return IsUsedAsParameterInAnyMethod(classes, obj) || IsUsedAsResponseInAnyMethod(classes, obj)
                || IsUsedAsReferenceInAnyObject(obj) || IsUsedAsUriParameter(classes, obj);
        }

        private bool IsUsedAsUriParameter(IEnumerable<Core.ClientGenerator.ClassObject> classes, ApiObject obj)
        {
            return classes.Any(c => c.Methods.Any(m => (m.UriParameters != null && m.UriParameters.Any(up => up.Type == obj.Type))));
        }

        public bool IsUsedAsResponseInAnyMethod(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, ApiObject requestObj)
        {
            return controllers.Any(c => c.Methods.Any(m => m.ReturnType == requestObj.Type ||
                m.ReturnType == CollectionTypeHelper.GetCollectionType(requestObj.Type) ||
                CollectionTypeHelper.GetCollectionType(m.ReturnType) == requestObj.Type ||
                m.ReturnType == requestObj.Name ||
                m.ReturnType == CollectionTypeHelper.GetCollectionType(requestObj.Name) ||
                CollectionTypeHelper.GetCollectionType(m.ReturnType) == requestObj.Name));
        }

        public bool IsUsedAsParameterInAnyMethod(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, ApiObject requestObj)
        {
            return controllers.Any(c => c.Methods
                .Any(m => m.Parameter != null
                          && (m.Parameter.Type == requestObj.Type || m.Parameter.Type == CollectionTypeHelper.GetCollectionType(requestObj.Type)
                          || CollectionTypeHelper.GetCollectionType(m.Parameter.Type) == requestObj.Type) ));
        }

        public bool IsUsedAsResponseInAnyMethod(IEnumerable<ClassObject> classes, ApiObject requestObj)
        {
            return classes.Any(c => c.Methods.Any(m => m.ReturnType == requestObj.Type 
            || m.ReturnType == CollectionTypeHelper.GetCollectionType(requestObj.Type) 
            || CollectionTypeHelper.GetCollectionType(m.ReturnType) == requestObj.Type
            || (m.ReturnTypeObject != null && m.ReturnTypeObject.Properties != null 
            && m.ReturnTypeObject.Properties.Any(x => x.Type == requestObj.Type || x.Type == CollectionTypeHelper.GetCollectionType(requestObj.Type)
            || CollectionTypeHelper.GetCollectionType(x.Type) == requestObj.Type) )));
        }

        public bool IsUsedAsParameterInAnyMethod(IEnumerable<ClassObject> classes, ApiObject requestObj)
        {
            return classes.Any(c => c.Methods
                .Any(m => m.Parameter != null
                          && (m.Parameter.Type == requestObj.Type || m.Parameter.Type == CollectionTypeHelper.GetCollectionType(requestObj.Type)
                          || CollectionTypeHelper.GetCollectionType(m.Parameter.Type) == requestObj.Type) ));
        }
        
        private bool IsUsedAsReferenceInAnyObject(ApiObject obj)
        {
            return schemaObjects.SelectMany(o => o.Value.Properties).Any(x => x.Type == obj.Type ||
                     x.Type == CollectionTypeHelper.GetCollectionType(obj.Type) ||
                     x.Type == obj.BaseClass ||
                     CollectionTypeHelper.GetCollectionType(x.Type) == obj.Type ||
                     x.Type == CollectionTypeHelper.GetCollectionType(obj.BaseClass) ||
                     CollectionTypeHelper.GetCollectionType(x.Type) == obj.BaseClass
                     )

                   || schemaObjects.Values.Any(o => o.BaseClass == obj.Type)

                   || schemaRequestObjects.SelectMany(o => o.Value.Properties).Any(x => x.Type == obj.Type || 
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.Type) || 
                        x.Type == obj.BaseClass ||
                        CollectionTypeHelper.GetCollectionType(x.Type) == obj.Type ||
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.BaseClass) ||
                        CollectionTypeHelper.GetCollectionType(x.Type) == obj.BaseClass)

                   || schemaResponseObjects.SelectMany(o => o.Value.Properties).Any(x => x.Type == obj.Type || 
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.Type) ||
                        x.Type == obj.BaseClass ||
                        CollectionTypeHelper.GetCollectionType(x.Type) == obj.Type ||
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.BaseClass) ||
                        CollectionTypeHelper.GetCollectionType(x.Type) == obj.BaseClass);
        }

    }
}