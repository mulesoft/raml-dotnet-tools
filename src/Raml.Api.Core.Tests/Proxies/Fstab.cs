
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RAML.Api.Core;


namespace Fstab
{
    public partial class Entries
    {
        private readonly FstabApi proxy;

        internal Entries(FstabApi proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Retrieve a list of fstab entries - All fstab entries
        /// </summary>
        public virtual async Task<Models.EntriesGetResponse> Get()
        {

            var url = "entries";
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content);
                }

            }

            return new Models.EntriesGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content), true)
            };

        }

        /// <summary>
        /// Retrieve a list of fstab entries - All fstab entries
        /// </summary>
        /// <param name="request">ApiRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.EntriesGetResponse> Get(ApiRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "entries";
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            var response = await proxy.Client.SendAsync(req);
            if (proxy.SchemaValidation.Enabled && proxy.SchemaValidation.RaiseExceptions)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content);
                }

            }
            return new Models.EntriesGetResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content), true)
            };
        }

        /// <summary>
        /// Add an fstab entry - All fstab entries
        /// </summary>
        /// <param name="entry"></param>
        public virtual async Task<Models.EntriesPostResponse> Post(Models.Entry entry)
        {

            var url = "entries";
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new ObjectContent(typeof(Models.Entry), entry, new JsonMediaTypeFormatter());
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content);
                }

            }

            var headers = new Models.PostEntriesCreatedResponseHeader();
            headers.SetProperties(response.Headers);
            return new Models.EntriesPostResponse
            {
                RawContent = response.Content,
                Headers = headers,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content), true)
            };

        }

        /// <summary>
        /// Add an fstab entry - All fstab entries
        /// </summary>
        /// <param name="request">Models.EntriesPostRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.EntriesPostResponse> Post(Models.EntriesPostRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "entries";
            var req = new HttpRequestMessage(HttpMethod.Post, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            if (request.Formatter == null)
                request.Formatter = new JsonMediaTypeFormatter();

            req.Content = new ObjectContent(typeof(Models.Entry), request.Content, request.Formatter);
            var response = await proxy.Client.SendAsync(req);
            if (proxy.SchemaValidation.Enabled && proxy.SchemaValidation.RaiseExceptions)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content);
                }

            }
            var headers = new Models.PostEntriesCreatedResponseHeader();
            headers.SetProperties(response.Headers);
            return new Models.EntriesPostResponse
            {
                RawContent = response.Content,
                Headers = headers,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content), true)
            };
        }

        /// <summary>
        /// Retrieve a single fstab entry for the specfied device - A single fstab entry
        /// </summary>
        /// <param name="device"></param>
        public virtual async Task<Models.EntriesGetByDeviceResponse> GetByDevice(string device)
        {

            var url = "entries/{device}";
            url = url.Replace("{device}", device.ToString());
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await proxy.Client.SendAsync(req);

            if (proxy.SchemaValidation.Enabled)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content);
                }

            }

            return new Models.EntriesGetByDeviceResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content), true)
            };

        }

        /// <summary>
        /// Retrieve a single fstab entry for the specfied device - A single fstab entry
        /// </summary>
        /// <param name="request">Models.EntriesGetByDeviceRequest</param>
        /// <param name="responseFormatters">response formmaters</param>
        public virtual async Task<Models.EntriesGetByDeviceResponse> GetByDevice(Models.EntriesGetByDeviceRequest request, IEnumerable<MediaTypeFormatter> responseFormatters = null)
        {

            var url = "entries/{device}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Device == null)
                throw new InvalidOperationException("Uri Parameter Device cannot be null");

            url = url.Replace("{device}", request.UriParameters.Device.ToString());
            var req = new HttpRequestMessage(HttpMethod.Get, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            var response = await proxy.Client.SendAsync(req);
            if (proxy.SchemaValidation.Enabled && proxy.SchemaValidation.RaiseExceptions)
            {
                if (proxy.SchemaValidation.RaiseExceptions)
                {
                    await SchemaValidator.ValidateWithExceptionAsync("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content);
                }

            }
            return new Models.EntriesGetByDeviceResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                Formatters = responseFormatters,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => SchemaValidator.IsValid("{    \"id\": \"http://some.site.somewhere/entry-schema#\",    \"$schema\": \"http://json-schema.org/draft-04/schema#\",    \"description\": \"schema for an fstab entry\",    \"type\": \"object\",    \"required\": [ \"storage\" ],    \"properties\": {        \"storage\": {            \"type\": \"object\",            \"oneOf\": [                { \"$ref\": \"#/definitions/diskDevice\" },                { \"$ref\": \"#/definitions/diskUUID\" },                { \"$ref\": \"#/definitions/nfs\" },                { \"$ref\": \"#/definitions/tmpfs\" }            ]        },        \"fstype\": {            \"enum\": [ \"ext3\", \"ext4\", \"btrfs\" ]        },        \"options\": {            \"type\": \"array\",            \"minItems\": 1,            \"items\": { \"type\": \"string\" },            \"uniqueItems\": true        },        \"readonly\": { \"type\": \"boolean\" }    },    \"definitions\": {        \"diskDevice\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"device\": {                    \"type\": \"string\",                    \"pattern\": \"^/dev/[^/]+(/[^/]+)*$\"                }            },            \"required\": [ \"type\", \"device\" ],            \"additionalProperties\": false        },        \"diskUUID\": {            \"properties\": {                \"type\": { \"enum\": [ \"disk\" ] },                \"label\": {                    \"type\": \"string\",                    \"pattern\": \"^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$\"                }            },            \"required\": [ \"type\", \"label\" ],            \"additionalProperties\": false        },        \"nfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"nfs\" ] },                \"remotePath\": {                    \"type\": \"string\",                    \"pattern\": \"^(/[^/]+)+$\"                },                \"server\": {                    \"type\": \"string\",                    \"oneOf\": [                        { \"format\": \"host-name\" },                        { \"format\": \"ipv4\" },                        { \"format\": \"ipv6\" }                    ]                }            },            \"required\": [ \"type\", \"server\", \"remotePath\" ],            \"additionalProperties\": false        },        \"tmpfs\": {            \"properties\": {                \"type\": { \"enum\": [ \"tmpfs\" ] },                \"sizeInMB\": {                    \"type\": \"integer\",                    \"minimum\": 16,                    \"maximum\": 512                }            },            \"required\": [ \"type\", \"sizeInMB\" ],            \"additionalProperties\": false        }    }}", response.Content), true)
            };
        }

        /// <summary>
        /// Remove a single fstab entry for the specified device - A single fstab entry
        /// </summary>
        /// <param name="device"></param>
        public virtual async Task<ApiResponse> Delete(string device)
        {

            var url = "entries/{device}";
            url = url.Replace("{device}", device.ToString());
            var req = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await proxy.Client.SendAsync(req);

            return new ApiResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
            };

        }

        /// <summary>
        /// Remove a single fstab entry for the specified device - A single fstab entry
        /// </summary>
        /// <param name="request">Models.EntriesDeleteRequest</param>
        public virtual async Task<ApiResponse> Delete(Models.EntriesDeleteRequest request)
        {

            var url = "entries/{device}";
            if (request.UriParameters == null)
                throw new InvalidOperationException("Uri Parameters cannot be null");

            if (request.UriParameters.Device == null)
                throw new InvalidOperationException("Uri Parameter Device cannot be null");

            url = url.Replace("{device}", request.UriParameters.Device.ToString());
            var req = new HttpRequestMessage(HttpMethod.Delete, url);

            if (request.RawHeaders != null)
            {
                foreach (var header in request.RawHeaders)
                {
                    req.Headers.TryAddWithoutValidation(header.Key, string.Join(",", header.Value));
                }
            }
            var response = await proxy.Client.SendAsync(req);
            return new ApiResponse
            {
                RawContent = response.Content,
                RawHeaders = response.Headers,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                SchemaValidation = new Lazy<SchemaValidationResults>(() => new SchemaValidationResults(true), true)
            };
        }

    }

    /// <summary>
    /// Main class for grouping root resources. Nested resources are defined as properties. The constructor can optionally receive an URL and HttpClient instance to override the default ones.
    /// </summary>
    public partial class FstabApi
    {

        protected readonly HttpClient client;
        public const string BaseUri = "http://localhost/fstab/v1/";

        internal HttpClient Client { get { return client; } }

        public SchemaValidationSettings SchemaValidation { get; private set; }

        public FstabApi(string endpointUrl)
        {
            SchemaValidation = new SchemaValidationSettings
            {
                Enabled = true,
                RaiseExceptions = true
            };


            if (string.IsNullOrWhiteSpace(endpointUrl))
                throw new ArgumentException("You must specify the endpoint URL", "endpointUrl");

            if (endpointUrl.Contains("{"))
            {
                var regex = new Regex(@"\{([^\}]+)\}");
                var matches = regex.Matches(endpointUrl);
                var parameters = new List<string>();
                foreach (Match match in matches)
                {
                    parameters.Add(match.Groups[1].Value);
                }
                throw new InvalidOperationException("Please replace parameter/s " + string.Join(", ", parameters) + " in the URL before passing it to the constructor ");
            }

            client = new HttpClient { BaseAddress = new Uri(endpointUrl) };
        }

        public FstabApi(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
                throw new InvalidOperationException("You must set the BaseAddress property of the HttpClient instance");


            client = httpClient;

            SchemaValidation = new SchemaValidationSettings
            {
                Enabled = true,
                RaiseExceptions = true
            };
        }


        public virtual Entries Entries
        {
            get { return new Entries(this); }
        }



        public void AddDefaultRequestHeader(string name, string value)
        {
            client.DefaultRequestHeaders.Add(name, value);
        }

        public void AddDefaultRequestHeader(string name, IEnumerable<string> values)
        {
            client.DefaultRequestHeaders.Add(name, values);
        }


    }

} // end namespace


namespace Fstab.Models
{
    public abstract class Storage
    {

    } // end class

    public partial class StoragediskDevice : Storage
    {
        public string Device { get; set; }

    } // end class

    public partial class StoragediskUUID : Storage
    {
        public string Label { get; set; }

    } // end class

    public partial class Storagenfs : Storage
    {
        public string RemotePath { get; set; }
        public string Server { get; set; }

    } // end class

    public partial class Storagetmpfs : Storage
    {
        public int SizeInMB { get; set; }

    } // end class

    public partial class Entry
    {
        public Storage Storage { get; set; }
        public string[] Options { get; set; }
        public bool Readonly { get; set; }

    } // end class

    /// <summary>
    /// Uri Parameters for resource /{device}
    /// </summary>
    public partial class EntriesDeviceUriParameters
    {
        public string Device { get; set; }

    } // end class

    public partial class PostEntriesCreatedResponseHeader : ApiResponseHeader
    {
        public string Location { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Post of class Entries
    /// </summary>
    public partial class EntriesPostRequest : ApiRequest
    {
        public EntriesPostRequest(Entry Content = null, MediaTypeFormatter Formatter = null)
        {
            this.Content = Content;
            this.Formatter = Formatter;
        }

        /// <summary>
        /// Request content
        /// </summary>
        public Entry Content { get; set; }
        /// <summary>
        /// Request formatter
        /// </summary>
        public MediaTypeFormatter Formatter { get; set; }

    } // end class

    /// <summary>
    /// Request object for method GetByDevice of class Entries
    /// </summary>
    public partial class EntriesGetByDeviceRequest : ApiRequest
    {
        public EntriesGetByDeviceRequest(EntriesDeviceUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public EntriesDeviceUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Request object for method Delete of class Entries
    /// </summary>
    public partial class EntriesDeleteRequest : ApiRequest
    {
        public EntriesDeleteRequest(EntriesDeviceUriParameters UriParameters)
        {
            this.UriParameters = UriParameters;
        }

        /// <summary>
        /// Request Uri Parameters
        /// </summary>
        public EntriesDeviceUriParameters UriParameters { get; set; }

    } // end class

    /// <summary>
    /// Response object for method Get of class Entries
    /// </summary>

    public partial class EntriesGetResponse : ApiResponse
    {


        private Entry typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Entry Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<Entry>(Formatters).ConfigureAwait(false)
                            : RawContent.ReadAsAsync<Entry>().ConfigureAwait(false);

                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class

    /// <summary>
    /// Response object for method Post of class Entries
    /// </summary>

    public partial class EntriesPostResponse : ApiResponse
    {

        /// <summary>
        /// Typed Response headers (defined in RAML)
        /// </summary>
        public Models.PostEntriesCreatedResponseHeader Headers { get; set; }

        private Entry typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Entry Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<Entry>(Formatters).ConfigureAwait(false)
                            : RawContent.ReadAsAsync<Entry>().ConfigureAwait(false);

                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class

    /// <summary>
    /// Response object for method GetByDevice of class Entries
    /// </summary>

    public partial class EntriesGetByDeviceResponse : ApiResponse
    {


        private Entry typedContent;
        /// <summary>
        /// Typed Response content
        /// </summary>
        public Entry Content
        {
            get
            {
                if (typedContent != null)
                    return typedContent;

                var task = Formatters != null && Formatters.Any()
                            ? RawContent.ReadAsAsync<Entry>(Formatters).ConfigureAwait(false)
                            : RawContent.ReadAsAsync<Entry>().ConfigureAwait(false);

                typedContent = task.GetAwaiter().GetResult();
                return typedContent;
            }
        }

    } // end class


} // end Models namespace