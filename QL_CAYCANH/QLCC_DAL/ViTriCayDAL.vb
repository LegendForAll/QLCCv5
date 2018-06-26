Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class ViTriCayDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub

    Public Function getNextID(ByRef nextID As String) As Result

        nextID = String.Empty

        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [ID_VITRI] "
        query &= "FROM [VITRI] "
        query &= "ORDER BY [ID_VITRI] DESC "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    Dim msOnDB As String
                    msOnDB = Nothing
                    If reader.HasRows = True Then
                        While reader.Read()
                            msOnDB = reader("ID_VITRI")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextID = "LC01"
                        System.Console.WriteLine(nextID)
                    End If
                    'Neu trong database co du lieu
                    If (msOnDB <> "") Then

                        Dim tmp As String
                        Dim id As Integer
                        tmp = msOnDB.Substring(2)
                        id = Convert.ToInt32(tmp)
                        id += 1
                        If id > 9999999 Then
                            id = 1
                        End If
                        nextID = "LC" + id.ToString()
                        System.Console.WriteLine(nextID)
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed location code creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(lvt As ViTriCayDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [VITRI] ([ID_VITRI], [TEN_VITRI])"
        query &= "VALUES (@ID_VITRI,@TEN_VITRI)"

        'get MSHS
        Dim nextID = "1"
        getNextID(nextID)
        lvt.Ma_VT = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_VITRI", lvt.Ma_VT)
                    .Parameters.AddWithValue("@TEN_VITRI", lvt.Ten_VT)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More location failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function update(lvt As ViTriCayDTO) As Result

    End Function
    Public Function selectALL(ByRef listViTri As List(Of ViTriCayDTO)) As Result

    End Function
    Public Function delete(mvitri As Integer) As Result

    End Function
End Class
