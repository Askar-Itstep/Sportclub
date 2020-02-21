﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sportclub.App_Start
{
    public class UriToStringConverter : ITypeConverter<Uri, string>
    {
        public string Convert(Uri source, string destination, ResolutionContext context)
        {
            return source.ToString();
        }
    }
}