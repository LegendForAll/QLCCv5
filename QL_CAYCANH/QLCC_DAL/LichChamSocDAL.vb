Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class LichChamSocDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub

    Public Function buildMS_LichCS(ByRef nextID As String) As Result

        nextID = String.Empty
        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [ID_LICHCHAMSOC] "
        query &= "FROM [LICHCHAMSOC] "
        query &= "ORDER BY [ID_LICHCHAMSOC] DESC "

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
                            msOnDB = reader("ID_LICHCHAMSOC")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextID = "CA01"
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
                        nextID = "CA" + id.ToString()
                        System.Console.WriteLine(nextID)
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed calendar care code creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(lich As LichChamSocDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [LICHCHAMSOC] ([ID_LICHCHAMSOC], [NGAYLAP])"
        query &= "VALUES (@ID_LICHCHAMSOC,@NGAYLAP)"

        'get MSHS
        Dim nextID = "1"
        buildMS_LichCS(nextID)
        lich.MS_LichCS = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LICHCHAMSOC", lich.MS_LichCS)
                    .Parameters.AddWithValue("@NGAYLAP", lich.TG_ChamSoc)

                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More calendar care failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function update(lich As LichChamSocDTO) As Result

        Dim query As String = String.Empty
        query &= " UPDATE [LICHCHAMSOC] SET"
        query &= " [NGAYLAP] = @NGAYLAP "
        query &= "WHERE "
        query &= " [ID_LICHCHAMSOC] = @ID_LICHCHAMSOC "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LICHCHAMSOC", lich.MS_LichCS)
                    .Parameters.AddWithValue("@NGAYLAP", lich.TG_ChamSoc)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Cập nhật lich cham soc không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL(ByRef listlich As List(Of LichChamSocDTO)) As Result

        Dim query As String = String.Empty
        query &= " SELECT [ID_LICHCHAMSOC], [NGAYLAP]"
        query &= " FROM [LICHCHAMSOC]"


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
                    If reader.HasRows = True Then
                        listlich.Clear()
                        While reader.Read()
                            listlich.Add(New LichChamSocDTO(reader("ID_LICHCHAMSOC"), reader("NGAYLAP")))
                        End While
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Lấy tất cả lich cham soc không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function delete(malich As String) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [LICHCHAMSOC] "
        query &= " WHERE "
        query &= " [ID_LICHCHAMSOC] = @ID_LICHCHAMSOC "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LICHCHAMSOC", malich)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa lich cham soc không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)  ' thanh cong
    End Function
End Class
