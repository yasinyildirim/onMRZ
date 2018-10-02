﻿using System;

namespace onMRZ
{
    public class Customer
    {
        public bool IsValid { get; set; }

        public string ValidationMessage { get; set; }
        public string DocumentType { get; set; }


        public string DocumentTypeDescription { get; set; }


        public string AdditionalDocumentType { get; set; }


        public string IssuingCountryIso { get; set; }

        public string IssuingCountryName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string DocumentNumber { get; set; }

        public string NationalityIso { get; set; }

        public string NationalityName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public DateTime ExpireDate { get; set; }

        public DateTime IssueDate { get; set; }

        public string IssuingAuthority { get; set; }

        public string PlaceOfBirth { get; set; }

        public string OptionalData { get; set; }
    }
}