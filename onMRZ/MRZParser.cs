using System;
using System.Collections.Generic;
using System.Linq;

namespace onMRZ
{
    public static class MRZParser
    {
        private static readonly Nationalities Nationalities = new Nationalities();


        private static readonly Dictionary<char, int> CheckDigitArray = new Dictionary<char, int>();

        //Parsing is based on https://en.wikipedia.org/wiki/Machine-readable_passport
        //Useful information https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf
        public static string DocumentType
        {
            get
            {
                return string.IsNullOrEmpty(MrzLine1) ? string.Empty : MrzLine1.Substring(0, 1);
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
                var description = "Unknown Document Type";
                switch (DocumentType)
                {
                    case "P":
                        description = "Passport";
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


        public static string AdditionalDocumentType //Type (for countries that distinguish between different types of passports)
        {
            get 
            {
                return string.IsNullOrEmpty(MrzLine1) ? string.Empty : MrzLine1.Substring(1, 1);
            }
            set 
            {
                if (AdditionalDocumentType != value)
                    AdditionalDocumentType = value;
            }
        }



        public static string IssuingCountryIso
        {
            get 
            {
                return string.IsNullOrEmpty(MrzLine1) ? string.Empty : MrzLine1.Substring(2, 3);
            }
            set //todo
            {
                if (IssuingCountryIso != value)
                    IssuingCountryIso = value;
            }
        }

        public static string IssuingCountryName
        {
            get  
            {
                var natItem = Nationalities.NationalitybyCode(IssuingCountryIso);
                return natItem != null ? natItem.Name : string.Empty;
            }
            set  
            {
                if (IssuingCountryName != value)
                    IssuingCountryName = value;
            }
        }

        public static string FirstName
        {
            get  
            {
                if (string.IsNullOrEmpty(MrzLine1) || MrzLine1.Length < 44) return string.Empty;
                var nameArraySplit = MrzLine1.Substring(5).Split(new[] {"<<"}, StringSplitOptions.RemoveEmptyEntries);
                return nameArraySplit.Length >= 2 ? nameArraySplit[1].Replace("<", " ") : nameArraySplit[0].Replace("<", " ");
            }
            set
            {
                if (FirstName != value)
                    FirstName = value;
            }
        }

        public static string LastName
        {
            get  
            {
                if (string.IsNullOrEmpty(MrzLine1) || MrzLine1.Length < 44) return string.Empty;
                var nameArraySplit = MrzLine1.Substring(5).Split(new[] {"<<"}, StringSplitOptions.RemoveEmptyEntries);
                return nameArraySplit.Length >= 2 ? nameArraySplit[0].Replace("<", " ") : string.Empty;
            }
            set
            {
                if (LastName != value)
                    LastName = value;
            }
        }

        public static string FullName => (FirstName + " " + LastName).Trim();

        public static string DocumentNumber
        {
            get  
            {
                if (string.IsNullOrEmpty(MrzLine2) || MrzLine2.Length < 44) return string.Empty;
                return MrzLine2.Substring(0, 9).Replace("<", string.Empty);
            }
            set
            {
                if (DocumentNumber != value)
                    DocumentNumber = value;
            }
        }

        public static string NationalityIso
        {
            get  
            {
                if (string.IsNullOrEmpty(MrzLine2) || MrzLine2.Length < 44) return string.Empty;
                return MrzLine2.Substring(10, 3);
            }
            set
            {
                if (NationalityIso != value)
                    NationalityIso = value;
            }
        }

        public static string NationalityName
        {
            get  
            {
                var natItem = Nationalities.NationalitybyCode(NationalityIso);
                return natItem != null ? natItem.Name : string.Empty;
            }
            set  
            {
                if (NationalityName != value)
                    NationalityName = value;
            }
        }

        public static DateTime DateOfBirth
        {
            get  
            {
                if (string.IsNullOrEmpty(MrzLine2)) return new DateTime(1900, 1, 1);
                return new DateTime(int.Parse("20" + MrzLine2.Substring(13, 2)), int.Parse(MrzLine2.Substring(15, 2)),
                    int.Parse(MrzLine2.Substring(17, 2)));
            }
            set  
            {
                if (DateOfBirth != value)
                    DateOfBirth = value;
            }
        }

        public static double Age => DateTime.Now.Subtract(DateOfBirth).TotalDays / 365;

        public static string Gender
        {
            get  
            {
                if (string.IsNullOrEmpty(MrzLine2) || MrzLine2.Length < 44) return string.Empty;
                return MrzLine2.Substring(20, 1);
            }
            set
            {
                if (Gender != value)
                    Gender = value;
            }
        }

        public static DateTime ExpireDate
        {
            get 
            {
                if (string.IsNullOrEmpty(MrzLine2)) return new DateTime(1900, 1, 1);
                return new DateTime(int.Parse("20" + MrzLine2.Substring(21, 2)), int.Parse(MrzLine2.Substring(23, 2)),
                    int.Parse(MrzLine2.Substring(25, 2)));
            }
            set 
            {
                if (ExpireDate != value)
                    ExpireDate = value;
            }
        }

        public static DateTime IssueDate { get; set; }
        public static string IssuingAuthority { get; set; }
        public static string PlaceOfBirth { get; set; }

        public static string MrzLine1 =>
            !string.IsNullOrEmpty(Mrz) && Mrz.Length >= 44 ? Mrz.Substring(0, 44) : string.Empty;

        public static string MrzLine2 => !string.IsNullOrEmpty(Mrz) && Mrz.Length >= 88
            ? Mrz.Replace("\n", "").Replace("\r", "").Substring(44, 44)
            : string.Empty;

        private static string _mrz;
        public static string Mrz
        {
            get
            {
                if (string.IsNullOrEmpty(_mrz))
                {
                    return CreatMrz(false); 
                }
                return _mrz;
            }
            set
            {
                _mrz = value;
            }
        }

        public static string MrzwFullName { get; private set; }

        private static string CreatMrz(bool isMakeFullName)
        {
            if (string.IsNullOrEmpty(IssuingCountryIso) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(DocumentNumber) ||
                string.IsNullOrEmpty(NationalityIso) || DateOfBirth.Year < 1901 || string.IsNullOrEmpty(Gender) || ExpireDate.Year < 1901) return string.Empty;
            var line1 = "P<" + IssuingCountryIso + (LastName + "<<" + FirstName).Replace(" ", "<");
            if (isMakeFullName)
                line1 = "P<" + IssuingCountryIso + (FirstName + "<" + LastName).Replace(" ", "<");
            line1 = line1.PadRight(44, '<').Replace("-", "<");
            if (line1.Length > 44)
                line1 = line1.Substring(0, 44);
            var line2 = DocumentNumber.PadRight(9, '<') + CheckDigit(DocumentNumber.PadRight(9, '<')) + NationalityIso +
                        DateOfBirth.ToString("yyMMdd") +
                        CheckDigit(DateOfBirth.ToString("yyMMdd")) + Gender.Substring(0, 1) +
                        ExpireDate.ToString("yyMMdd") +
                        CheckDigit(ExpireDate.ToString("yyMMdd"));
            line2 = line2.PadRight(42, '<') + "0";
            var compositeCheckDigit =
                CheckDigit(line2.Substring(0, 10) + line2.Substring(13, 7) +
                           line2.Substring(21, 22));
            line2 = line2 + compositeCheckDigit.Replace("-", "<");
            return line1 + line2;
        }

        internal static string CheckDigit(string icaoPassportNumber)
        {
            //http://www.highprogrammer.com/alan/numbers/mrp.html#checkdigit
            if (!CheckDigitArray.Any())
                FillCheckDigitDictionary();
            icaoPassportNumber = icaoPassportNumber.ToUpper();
            var inputArray = icaoPassportNumber.Trim().ToCharArray();
            var multiplier = 7;
            var total = 0;
            foreach (var dig in inputArray)
            {
                total = total + CheckDigitArray[dig] * multiplier;
                if (multiplier == 7) multiplier = 3;
                else if (multiplier == 3) multiplier = 1;
                else if (multiplier == 1) multiplier = 7;
            }

            long result;
            Math.DivRem(total, 10, out result);
            return result.ToString();
        }

        private static void FillCheckDigitDictionary()
        {
            CheckDigitArray.Add('<', 0);
            CheckDigitArray.Add('0', 0);
            CheckDigitArray.Add('1', 1);
            CheckDigitArray.Add('2', 2);
            CheckDigitArray.Add('3', 3);
            CheckDigitArray.Add('4', 4);
            CheckDigitArray.Add('5', 5);
            CheckDigitArray.Add('6', 6);
            CheckDigitArray.Add('7', 7);
            CheckDigitArray.Add('8', 8);
            CheckDigitArray.Add('9', 9);
            CheckDigitArray.Add('A', 10);
            CheckDigitArray.Add('B', 11);
            CheckDigitArray.Add('C', 12);
            CheckDigitArray.Add('D', 13);
            CheckDigitArray.Add('E', 14);
            CheckDigitArray.Add('F', 15);
            CheckDigitArray.Add('G', 16);
            CheckDigitArray.Add('H', 17);
            CheckDigitArray.Add('I', 18);
            CheckDigitArray.Add('J', 19);
            CheckDigitArray.Add('K', 20);
            CheckDigitArray.Add('L', 21);
            CheckDigitArray.Add('M', 22);
            CheckDigitArray.Add('N', 23);
            CheckDigitArray.Add('O', 24);
            CheckDigitArray.Add('P', 25);
            CheckDigitArray.Add('Q', 26);
            CheckDigitArray.Add('R', 27);
            CheckDigitArray.Add('S', 28);
            CheckDigitArray.Add('T', 29);
            CheckDigitArray.Add('U', 30);
            CheckDigitArray.Add('V', 31);
            CheckDigitArray.Add('W', 32);
            CheckDigitArray.Add('X', 33);
            CheckDigitArray.Add('Y', 34);
            CheckDigitArray.Add('Z', 35);
        }
    }
}