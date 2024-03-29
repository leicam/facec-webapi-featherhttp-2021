﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Facec.WebApi
{
    public class Aluno
    {
        [JsonPropertyName("registroAcademico")]
        [XmlAttribute(AttributeName = "registroAcademico")]
        [Key]
        public string RegistroAcademico { get; set; }

        [JsonPropertyName("nome")]
        [XmlAttribute(AttributeName = "nome")]
        public string Nome { get; set; }
    }
}