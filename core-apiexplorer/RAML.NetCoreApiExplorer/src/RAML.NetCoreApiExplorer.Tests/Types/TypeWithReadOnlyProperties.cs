using System;

namespace RAML.NetCoreApiExplorer.Tests.Types
{
	public class TypeWithReadOnlyProperties 
	{
	    public TypeWithReadOnlyProperties(string readOnlyProp)
	    {
	        ReadOnlyProp = readOnlyProp;
	    }

	    public Uri BaseUri { get; set; }
		public string Location { get; set; }

	    public string ReadOnlyProp { get; }

	    public ReadOnlyObject ReadOnlyObject { get; set; }
	}

    public class ReadOnlyObject
    {
        private readonly int id;
        private readonly string name;

        public ReadOnlyObject(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}
