Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility
Imports System.Data.DataTable

Public Class ClassVitriDAL

    Dim table As New DataTable("Table")
    ' Read ConnectionString value from App.config file
    Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
    Public Sub New()
        ' Read ConnectionString value from App.config file
        Dim connection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
    End Sub

    Public Sub ExcuteQuery(query As String)
        Dim command As New SqlCommand(query, connection)
        connection.Open()
        command.ExecuteNonQuery()
        connection.Close()
    End Sub

    Public Function insert(lvt As ViTriCayDTO) As Boolean

        Dim updateQuery As String = "INSERT INTO [VITRI] ([ID_VITRI],[TEN_VITRI])
                                                        VALUES
                                                        ('" & lvt.Ma_VT & "','" & lvt.Ten_VT & "')"
        ExcuteQuery(updateQuery)
        Return True
    End Function
    Public Function update(lvt As ViTriCayDTO) As Boolean

        Dim updateQuery As String = "UPDATE [VITRI] SET [ID_VITRI] = '" & lvt.Ma_VT & "'
                                                            ,[TEN_VITRI] = '" & lvt.Ten_VT & "'
                                                            WHERE [ID_VITRI] = '" & lvt.Ma_VT & "'"
        ExcuteQuery(updateQuery)
        Return True
    End Function
End Class
