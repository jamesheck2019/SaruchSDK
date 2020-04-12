using System;
using System.Collections.Generic;
using System.Linq;

namespace SaruchSDK
{
    public class Utilitiez
    {
       public enum UploadTypes
        {
            FilePath,
            Stream,
            BytesArry
        }
        public static string AsQueryString(Dictionary<string, string> parameters)
        {
            if (!parameters.Any())
                return string.Empty;
            var builder = new System.Text.StringBuilder("?");
            var separator = string.Empty;
            foreach (var kvp in parameters.Where(P => !string.IsNullOrEmpty(P.Value)))
            {
                builder.AppendFormat("{0}{1}={2}", separator, System.Net.WebUtility.UrlEncode(kvp.Key), System.Net.WebUtility.UrlEncode(kvp.Value.ToString()));
                separator = "&";
            }
            return builder.ToString();
        }

        public static string hs1FileHash(System.IO.Stream FileStream)
        {
            using (System.IO.BufferedStream bs = new System.IO.BufferedStream(FileStream))
            {
                using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    System.Text.StringBuilder formatted = new System.Text.StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                        formatted.AppendFormat("{0:X2}", b);
                    return formatted.ToString();
                }
            }
        }

        public static string stringValueOf(Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static object enumValueOf(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);

            foreach (string name in names)
            {
                if (stringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                    return Enum.Parse(enumType, name);
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }

      public   enum ResolutionEnum
        {
            x144 = 144,
            x240 = 240,
            x360 = 360,
            x480 = 480,
            x720 = 720,
            x1080 = 1080,
            x1440 = 1440,
            x2160 = 2160
        }
        public enum OrderByEnum
        {
            desc,
            asc
        }
        public enum SortEnum
        {
            created_at,
            size,
            name,
            views
        }

        public enum LanguageCodeEnum
        {
            [System.ComponentModel.Description("ab")]
            Abkhazian,
            [System.ComponentModel.Description("aa")]
            Afar,
            [System.ComponentModel.Description("af")]
            Afrikaans,
            [System.ComponentModel.Description("ak")]
            Akan,
            [System.ComponentModel.Description("sq")]
            Albanian,
            [System.ComponentModel.Description("am")]
            Amharic,
            [System.ComponentModel.Description("ar")]
            Arabic,
            [System.ComponentModel.Description("an")]
            Aragonese,
            [System.ComponentModel.Description("hy")]
            Armenian,
            [System.ComponentModel.Description("as")]
            Assamese,
            [System.ComponentModel.Description("av")]
            Avaric,
            [System.ComponentModel.Description("ae")]
            Avestan,
            [System.ComponentModel.Description("ay")]
            Aymara,
            [System.ComponentModel.Description("az")]
            Azerbaijani,
            [System.ComponentModel.Description("bm")]
            Bambara,
            [System.ComponentModel.Description("ba")]
            Bashkir,
            [System.ComponentModel.Description("eu")]
            Basque,
            [System.ComponentModel.Description("be")]
            Belarusian,
            [System.ComponentModel.Description("bn")]
            Bengali,
            [System.ComponentModel.Description("bh")]
            Bihari,
            [System.ComponentModel.Description("bi")]
            Bislama,
            [System.ComponentModel.Description("bs")]
            Bosnian,
            [System.ComponentModel.Description("br")]
            Breton,
            [System.ComponentModel.Description("bg")]
            Bulgarian,
            [System.ComponentModel.Description("my")]
            Burmese,
            [System.ComponentModel.Description("ca")]
            Catalan,
            [System.ComponentModel.Description("ch")]
            Chamorro,
            [System.ComponentModel.Description("ce")]
            Chechen,
            [System.ComponentModel.Description("ny")]
            Chichewa_Chewa_Nyanja,
            [System.ComponentModel.Description("zh")]
            Chinese,
            [System.ComponentModel.Description("cv")]
            Chuvash,
            [System.ComponentModel.Description("kw")]
            Cornish,
            [System.ComponentModel.Description("co")]
            Corsican,
            [System.ComponentModel.Description("cr")]
            Cree,
            [System.ComponentModel.Description("hr")]
            Croatian,
            [System.ComponentModel.Description("cs")]
            Czech,
            [System.ComponentModel.Description("da")]
            Danish,
            [System.ComponentModel.Description("dv")]
            Divehi_Dhivehi_Maldivian,
            [System.ComponentModel.Description("nl")]
            Dutch,
            [System.ComponentModel.Description("dz")]
            Dzongkha,
            [System.ComponentModel.Description("en")]
            English,
            [System.ComponentModel.Description("eo")]
            Esperanto,
            [System.ComponentModel.Description("et")]
            Estonian,
            [System.ComponentModel.Description("ee")]
            Ewe,
            [System.ComponentModel.Description("fo")]
            Faroese,
            [System.ComponentModel.Description("fj")]
            Fijian,
            [System.ComponentModel.Description("fi")]
            Finnish,
            [System.ComponentModel.Description("fr")]
            French,
            [System.ComponentModel.Description("ff")]
            Fula_Fulah_Pulaar_Pular,
            [System.ComponentModel.Description("gl")]
            Galician,
            [System.ComponentModel.Description("gd")]
            Scottish,
            [System.ComponentModel.Description("gv")]
            Manx,
            [System.ComponentModel.Description("ka")]
            Georgian,
            [System.ComponentModel.Description("de")]
            German,
            [System.ComponentModel.Description("el")]
            Greek,
            [System.ComponentModel.Description("kl")]
            Greenlandic,
            [System.ComponentModel.Description("gn")]
            Guarani,
            [System.ComponentModel.Description("gu")]
            Gujarati,
            [System.ComponentModel.Description("ht")]
            Haitian_Creole,
            [System.ComponentModel.Description("ha")]
            Hausa,
            [System.ComponentModel.Description("he")]
            Hebrew,
            [System.ComponentModel.Description("hz")]
            Herero,
            [System.ComponentModel.Description("hi")]
            Hindi,
            [System.ComponentModel.Description("ho")]
            Hiri_Motu,
            [System.ComponentModel.Description("hu")]
            Hungarian,
            [System.ComponentModel.Description("is")]
            Icelandic,
            [System.ComponentModel.Description("io")]
            Ido,
            [System.ComponentModel.Description("ig")]
            Igbo,
            [System.ComponentModel.Description("id")]
            Indonesian,
            [System.ComponentModel.Description("ia")]
            Interlingua,
            [System.ComponentModel.Description("ie")]
            Interlingue,
            [System.ComponentModel.Description("iu")]
            Inuktitut,
            [System.ComponentModel.Description("ik")]
            Inupiak,
            [System.ComponentModel.Description("ga")]
            Irish,
            [System.ComponentModel.Description("it")]
            Italian,
            [System.ComponentModel.Description("ja")]
            Japanese,
            [System.ComponentModel.Description("jv")]
            Javanese,
            [System.ComponentModel.Description("kl")]
            Kalaallisut_Greenlandic,
            [System.ComponentModel.Description("kn")]
            Kannada,
            [System.ComponentModel.Description("kr")]
            Kanuri,
            [System.ComponentModel.Description("ks")]
            Kashmiri,
            [System.ComponentModel.Description("kk")]
            Kazakh,
            [System.ComponentModel.Description("km")]
            Khmer,
            [System.ComponentModel.Description("ki")]
            Kikuyu,
            [System.ComponentModel.Description("rw")]
            wanda,
            [System.ComponentModel.Description("rn")]
            Kirundi,
            [System.ComponentModel.Description("ky")]
            Kyrgyz,
            [System.ComponentModel.Description("kv")]
            Komi,
            [System.ComponentModel.Description("kg")]
            Kongo,
            [System.ComponentModel.Description("ko")]
            Korean,
            [System.ComponentModel.Description("ku")]
            Kurdish,
            [System.ComponentModel.Description("kj")]
            Kwanyama,
            [System.ComponentModel.Description("lo")]
            Lao,
            [System.ComponentModel.Description("la")]
            Latin,
            [System.ComponentModel.Description("lv")]
            Latvian,
            [System.ComponentModel.Description("li")]
            Limburgish,
            [System.ComponentModel.Description("ln")]
            Lingala,
            [System.ComponentModel.Description("lt")]
            Lithuanian,
            [System.ComponentModel.Description("lu")]
            Luga_Katanga,
            [System.ComponentModel.Description("lg")]
            Luganda_Ganda,
            [System.ComponentModel.Description("lb")]
            Luxembourgish,
            [System.ComponentModel.Description("mk")]
            Macedonian,
            [System.ComponentModel.Description("mg")]
            Malagasy,
            [System.ComponentModel.Description("ms")]
            Malay,
            [System.ComponentModel.Description("ml")]
            Malayalam,
            [System.ComponentModel.Description("mt")]
            Maltese,
            [System.ComponentModel.Description("mi")]
            Maori,
            [System.ComponentModel.Description("mr")]
            Marathi,
            [System.ComponentModel.Description("mh")]
            Marshallese,
            [System.ComponentModel.Description("mo")]
            Moldavian,
            [System.ComponentModel.Description("mn")]
            Mongolian,
            [System.ComponentModel.Description("na")]
            Nauru,
            [System.ComponentModel.Description("nv")]
            Navajo,
            [System.ComponentModel.Description("ng")]
            Ndonga,
            [System.ComponentModel.Description("nd")]
            Northern_Ndebele,
            [System.ComponentModel.Description("ne")]
            Nepali,
            [System.ComponentModel.Description("no")]
            Norwegian,
            [System.ComponentModel.Description("nb")]
            Norwegian_bokmal,
            [System.ComponentModel.Description("nn")]
            Norwegian_nynorsk,
            [System.ComponentModel.Description("ii")]
            Nuosu,
            [System.ComponentModel.Description("oc")]
            Occitan,
            [System.ComponentModel.Description("oj")]
            Ojibwe,
            [System.ComponentModel.Description("cu")]
            Old_Church_Slavonic_Old_Bulgarian,
            [System.ComponentModel.Description("or")]
            Oriya,
            [System.ComponentModel.Description("om")]
            Oromo,
            [System.ComponentModel.Description("os")]
            Ossetian,
            [System.ComponentModel.Description("pi")]
            Pāli,
            [System.ComponentModel.Description("ps")]
            Pashto_Pushto,
            [System.ComponentModel.Description("fa")]
            Persian,
            [System.ComponentModel.Description("pl")]
            Polish,
            [System.ComponentModel.Description("pt")]
            Portuguese,
            [System.ComponentModel.Description("pa")]
            Punjabi,
            [System.ComponentModel.Description("qu")]
            Quechua,
            [System.ComponentModel.Description("rm")]
            Romansh,
            [System.ComponentModel.Description("ro")]
            Romanian,
            [System.ComponentModel.Description("ru")]
            Russian,
            [System.ComponentModel.Description("se")]
            Sami,
            [System.ComponentModel.Description("sm")]
            Samoan,
            [System.ComponentModel.Description("sg")]
            Sango,
            [System.ComponentModel.Description("sa")]
            Sanskrit,
            [System.ComponentModel.Description("sr")]
            Serbian,
            [System.ComponentModel.Description("sh")]
            Serbo_Croatian,
            [System.ComponentModel.Description("st")]
            Sesotho,
            [System.ComponentModel.Description("tn")]
            Setswana,
            [System.ComponentModel.Description("sn")]
            Shona,
            [System.ComponentModel.Description("ii")]
            Sichuan_Yi,
            [System.ComponentModel.Description("sd")]
            Sindhi,
            [System.ComponentModel.Description("si")]
            Sinhalese,
            [System.ComponentModel.Description("ss")]
            Siswati,
            [System.ComponentModel.Description("sk")]
            Slovak,
            [System.ComponentModel.Description("sl")]
            Slovenian,
            [System.ComponentModel.Description("so")]
            Somali,
            [System.ComponentModel.Description("nr")]
            Southern_Ndebele,
            [System.ComponentModel.Description("es")]
            Spanish,
            [System.ComponentModel.Description("su")]
            Sundanese,
            [System.ComponentModel.Description("sw")]
            Swahili,
            [System.ComponentModel.Description("ss")]
            Swati,
            [System.ComponentModel.Description("sv")]
            Swedish,
            [System.ComponentModel.Description("tl")]
            Tagalog,
            [System.ComponentModel.Description("ty")]
            Tahitian,
            [System.ComponentModel.Description("tg")]
            Tajik,
            [System.ComponentModel.Description("ta")]
            Tamil,
            [System.ComponentModel.Description("tt")]
            Tatar,
            [System.ComponentModel.Description("te")]
            Telugu,
            [System.ComponentModel.Description("th")]
            Thai,
            [System.ComponentModel.Description("bo")]
            Tibetan,
            [System.ComponentModel.Description("ti")]
            Tigrinya,
            [System.ComponentModel.Description("to")]
            Tonga,
            [System.ComponentModel.Description("ts")]
            Tsonga,
            [System.ComponentModel.Description("tr")]
            Turkish,
            [System.ComponentModel.Description("tk")]
            Turkmen,
            [System.ComponentModel.Description("tw")]
            Twi,
            [System.ComponentModel.Description("ug")]
            Uyghur,
            [System.ComponentModel.Description("uk")]
            Ukrainian,
            [System.ComponentModel.Description("ur")]
            Urdu,
            [System.ComponentModel.Description("uz")]
            Uzbek,
            [System.ComponentModel.Description("ve")]
            Venda,
            [System.ComponentModel.Description("vi")]
            Vietnamese,
            [System.ComponentModel.Description("vo")]
            Volapük,
            [System.ComponentModel.Description("wa")]
            Wallon,
            [System.ComponentModel.Description("cy")]
            Welsh,
            [System.ComponentModel.Description("wo")]
            Wolof,
            [System.ComponentModel.Description("fy")]
            Western_Frisian,
            [System.ComponentModel.Description("xh")]
            Xhosa,
            [System.ComponentModel.Description("yi")]
            Yiddish,
            [System.ComponentModel.Description("yo")]
            Yoruba,
            [System.ComponentModel.Description("za")]
            Zhuang_Chuang,
            [System.ComponentModel.Description("zu")]
            Zulu
        }

        public enum UploadRemoteClearEnum
        {
            done,
            failed,
            on_queue
        }

        public enum VideoCategoryEnum
        {
            Regular = 1,
            Adult = 2
        }

        // 0 = false , 1 = true
        public enum PrivacyEnum
        {
            Public = 1,
            Private = 0
        }
    }
}