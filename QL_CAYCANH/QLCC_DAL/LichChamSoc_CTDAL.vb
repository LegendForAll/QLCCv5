Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class LichChamSoc_CTDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub

    Public Function buildMS_LichCT(ByRef nextID As String) As Result

        nextID = String.Empty
        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [ID_LICHCT] "
        query &= "FROM [LICHCT] "
        query &= "ORDER BY [ID_LICHCT] DESC "

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
                            msOnDB = reader("ID_LICHCT")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextID = "CD01"
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
                        nextID = "CD" + id.ToString()
                        System.Console.WriteLine(nextID)
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed ID calendar care creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(lichCT As LichChamSoc_CTDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [LICHCT] ([ID_LICHCT], [ID_LICHCHAMSOC], [ID_CHAMSOC])"
        query &= "VALUES (@ID_LICHCT,@ID_LICHCHAMSOC, @ID_CHAMSOC)"

        'get MSHS
        Dim nextID = "1"
        buildMS_LichCT(nextID)
        lichCT.MS_LichCSCT = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LICHCT", lichCT.MS_LichCSCT)
                    .Parameters.AddWithValue("@ID_LICHCHAMSOC", lichCT.MS_LichCS)
                    .Parameters.AddWithValue("@ID_CHAMSOC", lichCT.MS_ChamSoc)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More calendar care detail failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function update(lichCT As LichChamSoc_CTDTO) As Result

        Dim query As String = String.Empty
        query &= " UPDATE [LICHCT] SET"
        query &= " [ID_LICHCHAMSOC] = @ID_LICHCHAMSOC "
        query &= " ,[ID_CHAMSOC] = @ID_CHAMSOC "
        query &= "WHERE "
        query &= " [ID_LICHCT] = @ID_LICHCT "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LICHCT", lichCT.MS_LichCSCT)
                    .Parameters.AddWithValue("@ID_LICHCHAMSOC", lichCT.MS_LichCS)
                    .Parameters.AddWithValue("@ID_CHAMSOC", lichCT.MS_ChamSoc)
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
    Public Function selectALL(ByRef listlichCT As List(Of LichChamSoc_CTDTO)) As Result

        Dim query As String = String.Empty
        query &= " SELECT [ID_LICHCT], [ID_LICHCHAMSOC], [ID_CHAMSOC]"
        query &= " FROM [LICHCT]"


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
                        listlichCT.Clear()
                        While reader.Read()
                            listlichCT.Add(New LichChamSoc_CTDTO(reader("ID_LICHCT"), reader("ID_LICHCHAMSOC"), reader("ID_CHAMSOC")))
                        End While
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Lấy tất cả lich cham soc chi tiet không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function delete(malich As String) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [LICHCT] "
        query &= " WHERE "
        query &= " [ID_LICHCT] = @ID_LICHCT "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LICHCT", malich)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa lich cham soc chi tiet không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)  ' thanh cong
    End Function
End Class
