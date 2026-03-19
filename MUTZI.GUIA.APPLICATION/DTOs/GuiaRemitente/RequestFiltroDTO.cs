using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente
{
    public class DataTableDTO
    {
        [JsonPropertyName("columns")]
        //[FromQuery(Name ="columns")]
        public List<RequestFiltroDTO> Columns { get; set; }
    }
    public class RequestFiltroDTO
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("search")]
        public SearchDTO Search { get; set; }
    }

    public class SearchDTO
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonPropertyName("regex")]
        public string? Regex { get; set; }
    }
}
