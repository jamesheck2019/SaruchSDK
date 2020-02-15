Imports SaruchSDK.JSON

Public Interface IRemoteUpload

    ''' <summary>
    ''' list all remote uploading tasks
    ''' </summary>
    ''' <param name="OffSet">items count to skip</param>
    Function Queue(Optional OffSet As Integer = 1) As Task(Of JSON_RemoteUploadQueue)
    ''' <summary>
    ''' search for remote upload task
    ''' </summary>
    ''' <param name="Keyword">source video url keyword</param>
    ''' <param name="OffSet">items count to skip</param>
    Function Search(Keyword As String, Optional OffSet As Integer = 1) As Task(Of JSON_RemoteUploadQueue)
    ''' <summary>
    ''' get remote upload status
    ''' </summary>
    ''' <param name="JobID">uploading id</param>
    Function Status(JobID As String) As Task(Of JSON_UploadRemoteStatus)
    ''' <summary>
    ''' delete remote upload task from the queue list
    ''' </summary>
    ''' <param name="JobID">uploading id</param>
    Function Delete(JobID As String) As Task(Of Boolean)
    ''' <summary>
    ''' clear remote upload queue list
    ''' </summary>
    Function Clear(ClearType As utilitiez.UploadRemoteClearEnum) As Task(Of String)


End Interface
