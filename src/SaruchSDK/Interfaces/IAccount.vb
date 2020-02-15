Imports SaruchSDK.JSON

Public Interface IAccount

    ''' <summary>
    ''' user metadata
    ''' </summary>
    Function UserInfo() As Task(Of JSON_UserInfo)
    ''' <summary>
    ''' account storage info
    ''' </summary>
    Function StorageInfo() As Task(Of JSON_StorageInfo)
    ''' <summary>
    ''' renew authentication token
    ''' </summary>
    Function RenewToken() As Task(Of JSON_GetToken)
    ''' <summary>
    ''' get video Conversion Settings
    ''' </summary>
    Function GetConversionSettings() As Task(Of JSON_GetConversionSetings)
    ''' <summary>
    ''' change video Conversion Settings
    ''' </summary>
    Function ChangeConversionSettings(Resolutions As Dictionary(Of utilitiez.ResolutionEnum, Boolean)) As Task(Of Boolean)
End Interface
