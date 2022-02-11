using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationServer
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    //public class Keys
    //{
    //    [JsonProperty("field1")]
    //    public string Field1 { get; set; }

    //    [JsonProperty("field2")]
    //    public string Field2 { get; set; }

    //    [JsonProperty("field3")]
    //    public string Field3 { get; set; }

    //    [JsonProperty("field4")]
    //    public string Field4 { get; set; }

    //    [JsonProperty("field5")]
    //    public string Field5 { get; set; }

    //    [JsonProperty("documentTypeId")]
    //    public string DocumentTypeId { get; set; }

    //    [JsonProperty("drawer")]
    //    public string Drawer { get; set; }

    //    [JsonProperty("documentType")]
    //    public string DocumentType { get; set; }
    //}

    //public class Info
    //{
    //    [JsonProperty("locationId")]
    //    public string LocationId { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("keys")]
    //    public Keys Keys { get; set; }

    //    [JsonProperty("notes")]
    //    public string Notes { get; set; }
    //}

    //public class ChildProperty
    //{
    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("elementId")]
    //    public object ElementId { get; set; }

    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("value")]
    //    public string Value { get; set; }
    //}

    //public class Property
    //{
    //    [JsonProperty("id")]
    //    public string Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("elementId")]
    //    public string ElementId { get; set; }

    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("value")]
    //    public string Value { get; set; }

    //    [JsonProperty("updateAction")]
    //    public string UpdateAction { get; set; }

    //    [JsonProperty("childProperties")]
    //    public List<ChildProperty> ChildProperties { get; set; }
    //}

    //public class DocumentRequest
    //{
    //    [JsonProperty("info")]
    //    public Info Info { get; set; }

    //    [JsonProperty("properties")]
    //    public List<Property> Properties { get; set; }
    //}




    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Keys
    {
        public string drawer { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string documentType { get; set; }
    }

    public class Info
    {
        public string id { get; set; }
        public string name { get; set; }
        public Keys keys { get; set; }
        public int version { get; set; }
        public string locationid { get; set; }
    }

    public class Page
    {
        public string id { get; set; }
        public string name { get; set; }
        public string extension { get; set; }
        public int pageNumber { get; set; }
    }

    public class DocumentRequest
    {
        public Info info { get; set; }
        public List<object> workflowItems { get; set; }
        public List<Page> pages { get; set; }
        public List<Property> properties { get; set; }
    }

    public class ChildProperty
    {
        public string id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Property
    {
        public string id { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public List<ChildProperty> childProperties { get; set; }
    }


    public class CProperty
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class CustomProperties
    {
        public List<CProperty> properties { get; set; }
    }

    public class Pages
    {
        public List<Page> pages { get; set; }
    }

}
