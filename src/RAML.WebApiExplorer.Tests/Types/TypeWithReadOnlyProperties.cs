using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAML.WebApiExplorer.Tests.Types {
	public class TypeWithReadOnlyProperties 
	{
	    public Uri BaseUri { get; set; }
		public string Location { get; set; }

        private string readOnly;
	    public string ReadOnlyProp
	    {
	        get { return readOnly; }
	    }

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
