using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace onMRZ
{
    public static class MRZParser
    {
        static readonly Nationalities Nationalities = new Nationalities();
        //Parsing is based on https://en.wikipedia.org/wiki/Machine-readable_passport
        //Useful information https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf
        public static string DocumentType {
            get
            {
                if (string.IsNullOrEmpty(MrzLine1)) return string.Empty;
                return MrzLine1.Substring(0, 1);
            }
            set
            {
                if (DocumentType != value)
                    DocumentType = value;
            }
        }

        public static string DocumentTypeDescription
        {
            get
            {
                if (string.IsNullOrEmpty(DocumentType)) return string.Empty;
                var description = string.Empty;
                switch (DocumentType)
                {
                    case "P":
                        description= "Passport";
                        break;
                    case "C":
                        description = "Card";
                        break;
                    case "V":
                        description = "Visa";
                        break;
                }
                switch (NationalityIso)
                {
                    case "XXA":
                        description += " Stateless";
                        break;
                    case "XXB":
                        description += " Refugee";
                        break;
                    case "XXC":
                        description += " Refugee not defined per 1951 convention";
                        break;
                    case "XXX":
                        description += " Unspecified Nationality";
                        break;
                }
                return description;
            }
        }


        public static string AdditionalDocumentType
        {
            get //todo
            {
                if (string.IsNullOrEmpty(MrzLine1)) return string.Empty;
                return MrzLine1.Substring(0, 1);
            }
            set//todo
            {
                if (DocumentType != value)
                    DocumentType = value;
            }
        }

        public static string AdditionalDocumentTypeDescription
        {
            get//todo
            {
                if (string.IsNullOrEmpty(DocumentType)) return string.Empty;
                switch (DocumentType)
                {
                    case "P":
                        return "Passport";
                    case "C":
                        return "Card";
                    case "V":
                        return "Visa";
                }
                return string.Empty;
            }
        }




        public static  string IssuingCountryIso
        {
            get //todo
            {
                if (string.IsNullOrEmpty(MrzLine1)) return string.Empty;
                return MrzLine1.Substring(2, 3);
            }
            set//todo
            {
                if (IssuingCountryIso != value)
                    IssuingCountryIso = value;
            }
        }
        public static  string IssuingCountryName
        {
            get //todo
            {
                var natItem = Nationalities.NationalitybyCode(IssuingCountryIso);
                if (natItem != null)
                    return natItem.Name;
                else
                    return string.Empty;
            }
            set//todo
            {
                if (IssuingCountryName != value)
                    IssuingCountryName = value;
            }
        }

        public static string FirstName
        {
            get//todo
            {
                if (string.IsNullOrEmpty(MrzLine1) || MrzLine1.Length < 88) return string.Empty;
                var nameArraySplit = MrzLine1.Substring(5).Split(new[] { "<<" }, StringSplitOptions.RemoveEmptyEntries);
                if (nameArraySplit.Length >= 2) //This should be exactly 2
                {
                    return nameArraySplit[1].Replace("<", " ");
                }
                else
                {
                    return nameArraySplit[0].Replace("<", " ");
                }

            }
            set
            {
                if (FirstName != value)
                    FirstName = value;
            }
        }

        public static string LastName
        {
            get//todo
            {
                if (string.IsNullOrEmpty(MrzLine1) || MrzLine1.Length < 88) return string.Empty;
                var nameArraySplit = MrzLine1.Substring(5).Split(new[] {"<<"}, StringSplitOptions.RemoveEmptyEntries);
                if (nameArraySplit.Length >= 2)
                {
                    return nameArraySplit[0].Replace("<", " ");
                }
                    else
                {
                    return string.Empty; //No last name. all is firstname
                }

            }
            set
            {
                if (LastName != value)
                    LastName = value;
            }
        }

        public static  string FullName => (FirstName + " " + LastName).Trim();
        public static  string DocumentNumber
        {
            get//todo
            {
                if (string.IsNullOrEmpty(MrzLine2) || MrzLine2.Length <88) return string.Empty;
                return MrzLine2.Substring(0, 9);
            }
            set
            {
                if (DocumentNumber != value)
                    DocumentNumber = value;
            }
        }

        public static  string NationalityIso
        {
            get//todo
            {
                if (string.IsNullOrEmpty(MrzLine2) || MrzLine2.Length < 88) return string.Empty;
                return MrzLine2.Substring(10, 3);
            }
            set
            {
                if (NationalityIso != value)
                    NationalityIso = value;
            }
        }
        public static  string NationalityName
        {
            get //todo
            {
                var natItem = Nationalities.NationalitybyCode(NationalityIso);
                if (natItem != null)
                    return natItem.Name;
                else
                    return string.Empty;
            }
            set//todo
            {
                if (NationalityIso != value)
                    NationalityIso = value;
            }
        }
        public static  DateTime DateOfBirth
        {
            get //todo
            {
                if (string.IsNullOrEmpty(MrzLine2)) return new DateTime(1900, 1, 1);
                return new DateTime(int.Parse("20" + MrzLine2.Substring(13, 2)), int.Parse(MrzLine2.Substring(15, 2)), Int32.Parse(MrzLine2.Substring(17, 2)));
            }
            set//todo
            {
                if (DateOfBirth != value)
                    DateOfBirth = value;
            }
        }

        public static  double Age => DateTime.Now.Subtract(DateOfBirth).TotalDays/365;

        public static  string Gender
        {
            get//todo
            {
                if (string.IsNullOrEmpty(MrzLine2) || MrzLine2.Length < 88) return string.Empty;
                return MrzLine2.Substring(20,1);
            }
            set
            {
                if (Gender != value)
                    Gender = value;
            }
        }
        public static  DateTime ExpireDate
        {
            get //todo
            {
                if (string.IsNullOrEmpty(MrzLine2)) return new DateTime(1900,1,1);
                return new DateTime( int.Parse("20" + MrzLine2.Substring(21, 2)), int.Parse(MrzLine2.Substring(23, 2)), Int32.Parse(MrzLine2.Substring(25, 2))); 
            }
            set//todo
            {
                if (ExpireDate != value)
                    ExpireDate = value;
            }
        }
        public static  DateTime IssueDate { get; set; }
        public static  string IssuingAuthority { get; set; }
        public static  string PlaceOfBirth { get; set; }
        public static  string MrzLine1 => !string.IsNullOrEmpty(Mrz) && Mrz.Length >= 44?  Mrz.Substring(0, 44): string.Empty;
        public static string MrzLine2 => !string.IsNullOrEmpty(Mrz) && Mrz.Length >= 88 ? Mrz.Replace("\n","").Replace("\r","").Substring(0, 44) : string.Empty;
        public static  string Mrz { get; set; }
        public static  string MrzwFullName { get; private set; }



    }
}
