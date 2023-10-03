using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShopOnline.Helper.Validate
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class CheckFileExtensions: DataTypeAttribute
    {
        private string? _extensions;

        public CheckFileExtensions()
            : base(DataType.Upload)
        {
            // Set DefaultErrorMessage, allowing user to set
            // ErrorMessageResourceType and ErrorMessageResourceName to use localized messages.
            ErrorMessage = "Choose file extension in {1}";
        }

        public string Extensions
        {
            // Default file extensions match those from jquery validate.
            get => string.IsNullOrWhiteSpace(_extensions) ? "png,jpg,jpeg,gif" : _extensions;
            set => _extensions = value;
        }

        private string ExtensionsFormatted => ExtensionsParsed.Aggregate((left, right) => left + ", " + right);

        private string ExtensionsNormalized =>
            Extensions.Replace(" ", string.Empty).Replace(".", string.Empty).ToLowerInvariant();

        private IEnumerable<string> ExtensionsParsed
        {
            get { return ExtensionsNormalized.Split(',').Select(e => "." + e); }
        }

        public override string FormatErrorMessage(string name) =>
            string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, ExtensionsFormatted);

        public override bool IsValid(object? value) {
            if (value == null) return true; 
            IFormFile file =  value as IFormFile;   
            if (file != null)
            {
                var fileName = file.FileName;
                return ValidateExtension(fileName);
            }
            return (value is string valueAsString && ValidateExtension(valueAsString));
        }
            

        private bool ValidateExtension(string fileName)
        {
            return ExtensionsParsed.Contains(Path.GetExtension(fileName).ToLowerInvariant());
        }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

