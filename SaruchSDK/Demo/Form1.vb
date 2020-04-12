Public Class Form1
    Private Async Sub Button1_ClickAsync(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim client As SaruchSDK.IClient = New SaruchSDK.SClient("token", Nothing)

            Dim rslt = Await SaruchSDK.Authentication.OneHourToken("user", "pass")
            FlowLayoutPanel1.Controls.Add(New PropertyGridEx(rslt))



            'Dim rslt = client.Root.RootID()
            'FlowLayoutPanel1.Controls.Add(New PropertyGridEx(rslt))












        Catch ex As Exception

        End Try
    End Sub
End Class

Public Class PropertyGridEx
    Inherits PropertyGrid

    Public Sub New(obj As Object)
        MyBase.HelpVisible = False
        MyBase.Width = 250
        MyBase.Height = 350
        MyBase.SelectedObject = obj
    End Sub
End Class