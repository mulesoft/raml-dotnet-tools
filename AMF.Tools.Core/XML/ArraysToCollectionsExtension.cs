using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace AMF.Tools.Core.XML
{
    /// <summary>
    /// Converts array-based properties into collection-based ones, and 
    /// creates a typed <see cref="CollectionBase"/> inherited class for it.
    /// </summary>
    public class ArraysToCollectionsExtension : ICodeExtension
    {
        #region ICodeExtension Members

        private readonly IDictionary<string, string> systemCollections = new Dictionary<string, string>();

        public void Process(CodeNamespace code, XmlSchema schema)
        {
            // Copy as we will be adding types.
            var types = new CodeTypeDeclaration[code.Types.Count];
            code.Types.CopyTo(types, 0);

            foreach (var type in types)
            {
                if (!type.IsClass && !type.IsStruct) continue;

                foreach (CodeTypeMember member in type.Members)
                {
                    if ((!(member is CodeMemberField) || ((CodeMemberField) member).Type.ArrayElementType == null) &&
                        (!(member is CodeMemberProperty) || ((CodeMemberProperty) member).Type.ArrayElementType == null))
                        continue;

                    ProcessFieldsAndProperties(code, member);
                }
            }
        }

        private void ProcessFieldsAndProperties(CodeNamespace code, CodeTypeMember member)
        {
            if (member is CodeMemberProperty)
            {
                ProcessProperty(member);
            }
            else
            {
                ProcessField(code, member);
            }
        }

        private void ProcessField(CodeNamespace code, CodeTypeMember member)
        {
            var field = (CodeMemberField) member;
            var col = GetCollection(field.Type.ArrayElementType);
            // Change field type to collection.
            if (col.Name.Contains("."))
            {
                GetValidName(col);

                if (systemCollections.ContainsKey(col.Name))
                {
                    field.Type = new CodeTypeReference(col.Name);
                    return;
                }

                systemCollections.Add(col.Name, col.Name);
            }

            field.Type = new CodeTypeReference(col.Name);
            code.Types.Add(col);
        }

        private void ProcessProperty(CodeTypeMember member)
        {
            var prop = (CodeMemberProperty) member;
            var col = GetCollection(prop.Type.ArrayElementType);
            if (col.Name.Contains("."))
            {
                GetValidName(col);
            }
            prop.Type = new CodeTypeReference(col.Name);
        }

        private static void GetValidName(CodeTypeDeclaration col)
        {
            col.Name = col.Name.StartsWith("System.") ? col.Name.Substring("System.".Length) : col.Name;
            col.Name = col.Name.Replace(".", string.Empty);
        }

        #endregion

        public CodeTypeDeclaration GetCollection(CodeTypeReference forType)
        {
            var col = new CodeTypeDeclaration(
                forType.BaseType + "Collection");
            col.BaseTypes.Add(typeof(CollectionBase));
            col.Attributes = MemberAttributes.Final | MemberAttributes.Public;

            // Add method
            var add = new CodeMemberMethod();
            add.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            add.Name = "Add";
            add.ReturnType = new CodeTypeReference(typeof(int));
            add.Parameters.Add(new CodeParameterDeclarationExpression(
                forType, "value"));
            // Generates: return base.InnerList.Add( value );
            add.Statements.Add(new CodeMethodReturnStatement(
                new CodeMethodInvokeExpression(
                    new CodePropertyReferenceExpression(
                        new CodeBaseReferenceExpression(), "InnerList"),
                    "Add",
                    new CodeExpression[] { new CodeArgumentReferenceExpression("value") }
                     )
                 )
             );

            // Add to type.
            col.Members.Add(add);

            // Indexer property ( 'this' )
            var indexer = new CodeMemberProperty();
            indexer.Attributes = MemberAttributes.Final | MemberAttributes.Public;
            indexer.Name = "Item";
            indexer.Type = forType;
            indexer.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(int), "idx"));
            indexer.HasGet = true;
            indexer.HasSet = true;
            // Generates: return ( theType ) base.InnerList[idx];
            indexer.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeCastExpression(
                        forType,
                        new CodeIndexerExpression(
                            new CodePropertyReferenceExpression(
                                new CodeBaseReferenceExpression(),
                                "InnerList"),
                            new CodeExpression[] { new CodeArgumentReferenceExpression("idx") })
                         )
                     )
                 );
            // Generates: base.InnerList[idx] = value;
            indexer.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeIndexerExpression(
                        new CodePropertyReferenceExpression(
                            new CodeBaseReferenceExpression(),
                            "InnerList"),
                        new CodeExpression[] { new CodeArgumentReferenceExpression("idx") }),
                    new CodeArgumentReferenceExpression("value")
                 )
             );

            // Add to type.
            col.Members.Add(indexer);

            return col;
        }
    }
}