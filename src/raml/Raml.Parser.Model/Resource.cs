using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Raml.Parser.Expressions
{
	public class Resource : BasicInfo
	{
		public Resource()
		{
			Resources = new Collection<Resource>();
			Methods = new Collection<Method>();
			Protocols = new Collection<Protocol>();
			BaseUriParameters = new Dictionary<string, Parameter>();
			UriParameters = new Dictionary<string, Parameter>();
		}

		public string RelativeUri { get; set; }

		public IDictionary<string, Parameter> BaseUriParameters { get; set; }

		public IDictionary<string, Parameter> UriParameters { get; set; }

		public string DisplayName { get; set; }

		public IEnumerable<Protocol> Protocols { get; set; }

		public IEnumerable<Method> Methods { get; set; }

		public ICollection<Resource> Resources { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }

        public IEnumerable<string> Is { get; set; }

		#region equality operators
		protected bool Equals(Resource other)
		{
			return this.RelativeUri == other.RelativeUri && this.BaseUriParameters == other.BaseUriParameters && this.UriParameters == other.UriParameters && this.Methods.Count() == other.Methods.Count();
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Resource)obj);
		}

		public override int GetHashCode()
		{
			return RelativeUri.GetHashCode();
		}

		public static bool operator ==(Resource left, Resource right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Resource left, Resource right)
		{
			return !Equals(left, right);
		}

		#endregion

	}
}