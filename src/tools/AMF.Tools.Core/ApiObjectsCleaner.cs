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
                || IsUsedAsReferenceInAnyObject(obj);
        }

        public bool IsUsedAnywhere(IEnumerable<Core.ClientGenerator.ClassObject> classes, ApiObject obj)
        {
            return IsUsedAsParameterInAnyMethod(classes, obj) || IsUsedAsResponseInAnyMethod(classes, obj)
                || IsUsedAsReferenceInAnyObject(obj);
        }

        public bool IsUsedAsResponseInAnyMethod(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, ApiObject requestObj)
        {
            return controllers.Any(c => c.Methods.Any(m => m.ReturnType == requestObj.Name ||   m.ReturnType == CollectionTypeHelper.GetCollectionType(requestObj.Name)));
        }

        public bool IsUsedAsParameterInAnyMethod(IEnumerable<Core.WebApiGenerator.ControllerObject> controllers, ApiObject requestObj)
        {
            return controllers.Any(c => c.Methods
                .Any(m => m.Parameter != null
                          && (m.Parameter.Type == requestObj.Name || m.Parameter.Type == CollectionTypeHelper.GetCollectionType(requestObj.Name))));
        }

        public bool IsUsedAsResponseInAnyMethod(IEnumerable<ClassObject> classes, ApiObject requestObj)
        {
            return classes.Any(c => c.Methods.Any(m => m.ReturnType == requestObj.Name || m.ReturnType == CollectionTypeHelper.GetCollectionType(requestObj.Name)));
        }

        public bool IsUsedAsParameterInAnyMethod(IEnumerable<ClassObject> classes, ApiObject requestObj)
        {
            return classes.Any(c => c.Methods
                .Any(m => m.Parameter != null
                          && (m.Parameter.Type == requestObj.Name || m.Parameter.Type == CollectionTypeHelper.GetCollectionType(requestObj.Name))));
        }
        
        private bool IsUsedAsReferenceInAnyObject(ApiObject obj)
        {
            return schemaObjects.SelectMany(o => o.Value.Properties).Any(x => x.Type == obj.Name ||
                     x.Type == CollectionTypeHelper.GetCollectionType(obj.Name) ||
                     x.Type == obj.BaseClass ||
                     x.Type == CollectionTypeHelper.GetCollectionType(obj.BaseClass))

                   || schemaObjects.Values.Any(o => o.BaseClass == obj.Type)

                   || schemaRequestObjects.SelectMany(o => o.Value.Properties).Any(x => x.Type == obj.Name || 
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.Name) || 
                        x.Type == obj.BaseClass || 
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.BaseClass))

                   || schemaResponseObjects.SelectMany(o => o.Value.Properties).Any(x => x.Type == obj.Name || 
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.Name) ||
                        x.Type == obj.BaseClass ||
                        x.Type == CollectionTypeHelper.GetCollectionType(obj.BaseClass));
        }

    }
}