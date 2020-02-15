Public Class utilitiez

    Enum UploadTypes
        FilePath
        Stream
        BytesArry
    End Enum
    Public Shared Function AsQueryString(parameters As Dictionary(Of String, String)) As String
        If Not parameters.Any() Then Return String.Empty
        Dim builder = New Text.StringBuilder("?")
        Dim separator = String.Empty
        For Each kvp In parameters.Where(Function(P) Not String.IsNullOrEmpty(P.Value))
            builder.AppendFormat("{0}{1}={2}", separator, Net.WebUtility.UrlEncode(kvp.Key), Net.WebUtility.UrlEncode(kvp.Value.ToString()))
            separator = "&"
        Next
        Return builder.ToString()
    End Function

    Shared Function hs1FileHash(FileStream As IO.Stream) As String
        Using bs As IO.BufferedStream = New IO.BufferedStream(FileStream)
            Using sha1 As Security.Cryptography.SHA1Managed = New Security.Cryptography.SHA1Managed
                Dim hash As Byte() = sha1.ComputeHash(bs)
                Dim formatted As Text.StringBuilder = New Text.StringBuilder(2 * hash.Length)
                For Each b As Byte In hash
                    formatted.AppendFormat("{0:X2}", b)
                Next
                Return formatted.ToString
            End Using
        End Using
    End Function

#Region "EnumUtils"
    Public Shared Function stringValueOf(ByVal value As [Enum]) As String
        Dim fi As Reflection.FieldInfo = value.[GetType]().GetField(value.ToString())
        Dim attributes As ComponentModel.DescriptionAttribute() = CType(fi.GetCustomAttributes(GetType(ComponentModel.DescriptionAttribute), False), ComponentModel.DescriptionAttribute())

        If attributes.Length > 0 Then
            Return attributes(0).Description
        Else
            Return value.ToString()
        End If
    End Function

    Public Shared Function enumValueOf(ByVal value As String, ByVal enumType As Type) As Object
        Dim names As String() = [Enum].GetNames(enumType)

        For Each name As String In names

            If stringValueOf(CType([Enum].Parse(enumType, name), [Enum])).Equals(value) Then
                Return [Enum].Parse(enumType, name)
            End If
        Next

        Throw New ArgumentException("The string is not a description or value of the specified enum.")
    End Function
#End Region

    Enum ResolutionEnum
        x144 = 144
        x240 = 240
        x360 = 360
        x480 = 480
        x720 = 720
        x1080 = 1080
        x1440 = 1440
        x2160 = 2160
    End Enum
    Enum OrderByEnum
        desc
        asc
    End Enum
    Enum SortEnum
        created_at
        size
        name
        views
    End Enum

    Enum LanguageCodeEnum
        <ComponentModel.Description("ab")> Abkhazian
        <ComponentModel.Description("aa")> Afar
        <ComponentModel.Description("af")> Afrikaans
        <ComponentModel.Description("ak")> Akan
        <ComponentModel.Description("sq")> Albanian
        <ComponentModel.Description("am")> Amharic
        <ComponentModel.Description("ar")> Arabic
        <ComponentModel.Description("an")> Aragonese
        <ComponentModel.Description("hy")> Armenian
        <ComponentModel.Description("as")> Assamese
        <ComponentModel.Description("av")> Avaric
        <ComponentModel.Description("ae")> Avestan
        <ComponentModel.Description("ay")> Aymara
        <ComponentModel.Description("az")> Azerbaijani
        <ComponentModel.Description("bm")> Bambara
        <ComponentModel.Description("ba")> Bashkir
        <ComponentModel.Description("eu")> Basque
        <ComponentModel.Description("be")> Belarusian
        <ComponentModel.Description("bn")> Bengali
        <ComponentModel.Description("bh")> Bihari
        <ComponentModel.Description("bi")> Bislama
        <ComponentModel.Description("bs")> Bosnian
        <ComponentModel.Description("br")> Breton
        <ComponentModel.Description("bg")> Bulgarian
        <ComponentModel.Description("my")> Burmese
        <ComponentModel.Description("ca")> Catalan
        <ComponentModel.Description("ch")> Chamorro
        <ComponentModel.Description("ce")> Chechen
        <ComponentModel.Description("ny")> Chichewa_Chewa_Nyanja
        <ComponentModel.Description("zh")> Chinese
        <ComponentModel.Description("cv")> Chuvash
        <ComponentModel.Description("kw")> Cornish
        <ComponentModel.Description("co")> Corsican
        <ComponentModel.Description("cr")> Cree
        <ComponentModel.Description("hr")> Croatian
        <ComponentModel.Description("cs")> Czech
        <ComponentModel.Description("da")> Danish
        <ComponentModel.Description("dv")> Divehi_Dhivehi_Maldivian
        <ComponentModel.Description("nl")> Dutch
        <ComponentModel.Description("dz")> Dzongkha
        <ComponentModel.Description("en")> English
        <ComponentModel.Description("eo")> Esperanto
        <ComponentModel.Description("et")> Estonian
        <ComponentModel.Description("ee")> Ewe
        <ComponentModel.Description("fo")> Faroese
        <ComponentModel.Description("fj")> Fijian
        <ComponentModel.Description("fi")> Finnish
        <ComponentModel.Description("fr")> French
        <ComponentModel.Description("ff")> Fula_Fulah_Pulaar_Pular
        <ComponentModel.Description("gl")> Galician
        <ComponentModel.Description("gd")> Scottish
        <ComponentModel.Description("gv")> Manx
        <ComponentModel.Description("ka")> Georgian
        <ComponentModel.Description("de")> German
        <ComponentModel.Description("el")> Greek
        <ComponentModel.Description("kl")> Greenlandic
        <ComponentModel.Description("gn")> Guarani
        <ComponentModel.Description("gu")> Gujarati
        <ComponentModel.Description("ht")> Haitian_Creole
        <ComponentModel.Description("ha")> Hausa
        <ComponentModel.Description("he")> Hebrew
        <ComponentModel.Description("hz")> Herero
        <ComponentModel.Description("hi")> Hindi
        <ComponentModel.Description("ho")> Hiri_Motu
        <ComponentModel.Description("hu")> Hungarian
        <ComponentModel.Description("is")> Icelandic
        <ComponentModel.Description("io")> Ido
        <ComponentModel.Description("ig")> Igbo
        <ComponentModel.Description("id")> Indonesian
        <ComponentModel.Description("ia")> Interlingua
        <ComponentModel.Description("ie")> Interlingue
        <ComponentModel.Description("iu")> Inuktitut
        <ComponentModel.Description("ik")> Inupiak
        <ComponentModel.Description("ga")> Irish
        <ComponentModel.Description("it")> Italian
        <ComponentModel.Description("ja")> Japanese
        <ComponentModel.Description("jv")> Javanese
        <ComponentModel.Description("kl")> Kalaallisut_Greenlandic
        <ComponentModel.Description("kn")> Kannada
        <ComponentModel.Description("kr")> Kanuri
        <ComponentModel.Description("ks")> Kashmiri
        <ComponentModel.Description("kk")> Kazakh
        <ComponentModel.Description("km")> Khmer
        <ComponentModel.Description("ki")> Kikuyu
        <ComponentModel.Description("rw")> wanda
        <ComponentModel.Description("rn")> Kirundi
        <ComponentModel.Description("ky")> Kyrgyz
        <ComponentModel.Description("kv")> Komi
        <ComponentModel.Description("kg")> Kongo
        <ComponentModel.Description("ko")> Korean
        <ComponentModel.Description("ku")> Kurdish
        <ComponentModel.Description("kj")> Kwanyama
        <ComponentModel.Description("lo")> Lao
        <ComponentModel.Description("la")> Latin
        <ComponentModel.Description("lv")> Latvian
        <ComponentModel.Description("li")> Limburgish
        <ComponentModel.Description("ln")> Lingala
        <ComponentModel.Description("lt")> Lithuanian
        <ComponentModel.Description("lu")> Luga_Katanga
        <ComponentModel.Description("lg")> Luganda_Ganda
        <ComponentModel.Description("lb")> Luxembourgish
        <ComponentModel.Description("mk")> Macedonian
        <ComponentModel.Description("mg")> Malagasy
        <ComponentModel.Description("ms")> Malay
        <ComponentModel.Description("ml")> Malayalam
        <ComponentModel.Description("mt")> Maltese
        <ComponentModel.Description("mi")> Maori
        <ComponentModel.Description("mr")> Marathi
        <ComponentModel.Description("mh")> Marshallese
        <ComponentModel.Description("mo")> Moldavian
        <ComponentModel.Description("mn")> Mongolian
        <ComponentModel.Description("na")> Nauru
        <ComponentModel.Description("nv")> Navajo
        <ComponentModel.Description("ng")> Ndonga
        <ComponentModel.Description("nd")> Northern_Ndebele
        <ComponentModel.Description("ne")> Nepali
        <ComponentModel.Description("no")> Norwegian
        <ComponentModel.Description("nb")> Norwegian_bokmal
        <ComponentModel.Description("nn")> Norwegian_nynorsk
        <ComponentModel.Description("ii")> Nuosu
        <ComponentModel.Description("oc")> Occitan
        <ComponentModel.Description("oj")> Ojibwe
        <ComponentModel.Description("cu")> Old_Church_Slavonic_Old_Bulgarian
        <ComponentModel.Description("or")> Oriya
        <ComponentModel.Description("om")> Oromo
        <ComponentModel.Description("os")> Ossetian
        <ComponentModel.Description("pi")> Pāli
        <ComponentModel.Description("ps")> Pashto_Pushto
        <ComponentModel.Description("fa")> Persian
        <ComponentModel.Description("pl")> Polish
        <ComponentModel.Description("pt")> Portuguese
        <ComponentModel.Description("pa")> Punjabi
        <ComponentModel.Description("qu")> Quechua
        <ComponentModel.Description("rm")> Romansh
        <ComponentModel.Description("ro")> Romanian
        <ComponentModel.Description("ru")> Russian
        <ComponentModel.Description("se")> Sami
        <ComponentModel.Description("sm")> Samoan
        <ComponentModel.Description("sg")> Sango
        <ComponentModel.Description("sa")> Sanskrit
        <ComponentModel.Description("sr")> Serbian
        <ComponentModel.Description("sh")> Serbo_Croatian
        <ComponentModel.Description("st")> Sesotho
        <ComponentModel.Description("tn")> Setswana
        <ComponentModel.Description("sn")> Shona
        <ComponentModel.Description("ii")> Sichuan_Yi
        <ComponentModel.Description("sd")> Sindhi
        <ComponentModel.Description("si")> Sinhalese
        <ComponentModel.Description("ss")> Siswati
        <ComponentModel.Description("sk")> Slovak
        <ComponentModel.Description("sl")> Slovenian
        <ComponentModel.Description("so")> Somali
        <ComponentModel.Description("nr")> Southern_Ndebele
        <ComponentModel.Description("es")> Spanish
        <ComponentModel.Description("su")> Sundanese
        <ComponentModel.Description("sw")> Swahili
        <ComponentModel.Description("ss")> Swati
        <ComponentModel.Description("sv")> Swedish
        <ComponentModel.Description("tl")> Tagalog
        <ComponentModel.Description("ty")> Tahitian
        <ComponentModel.Description("tg")> Tajik
        <ComponentModel.Description("ta")> Tamil
        <ComponentModel.Description("tt")> Tatar
        <ComponentModel.Description("te")> Telugu
        <ComponentModel.Description("th")> Thai
        <ComponentModel.Description("bo")> Tibetan
        <ComponentModel.Description("ti")> Tigrinya
        <ComponentModel.Description("to")> Tonga
        <ComponentModel.Description("ts")> Tsonga
        <ComponentModel.Description("tr")> Turkish
        <ComponentModel.Description("tk")> Turkmen
        <ComponentModel.Description("tw")> Twi
        <ComponentModel.Description("ug")> Uyghur
        <ComponentModel.Description("uk")> Ukrainian
        <ComponentModel.Description("ur")> Urdu
        <ComponentModel.Description("uz")> Uzbek
        <ComponentModel.Description("ve")> Venda
        <ComponentModel.Description("vi")> Vietnamese
        <ComponentModel.Description("vo")> Volapük
        <ComponentModel.Description("wa")> Wallon
        <ComponentModel.Description("cy")> Welsh
        <ComponentModel.Description("wo")> Wolof
        <ComponentModel.Description("fy")> Western_Frisian
        <ComponentModel.Description("xh")> Xhosa
        <ComponentModel.Description("yi")> Yiddish
        <ComponentModel.Description("yo")> Yoruba
        <ComponentModel.Description("za")> Zhuang_Chuang
        <ComponentModel.Description("zu")> Zulu
    End Enum

    Enum UploadRemoteClearEnum
        done
        failed
        on_queue
    End Enum

    Enum VideoCategoryEnum
        Regular = 1
        Adult = 2
    End Enum

    '0 = false , 1 = true
    Enum PrivacyEnum
        [Public] = 1
        [Private] = 0
    End Enum
End Class
Public Class ConnectionSettings
    Public Property TimeOut As System.TimeSpan = Nothing
    Public Property CloseConnection As Boolean? = True
    Public Property Proxy As ProxyConfig = Nothing
End Class