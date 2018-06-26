Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class CayDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub


    Public Function buildMS_Cay(ByRef nextMsCay As String) As Result

        nextMsCay = String.Empty

        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [ID_CAY] "
        query &= "FROM [CAY] "
        query &= "ORDER BY [ID_CAY] DESC "

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
                            msOnDB = reader("ID_CAY")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextMsCay = "TR01"
                        System.Console.WriteLine(nextMsCay)
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
                        nextMsCay = "TR" + id.ToString()
                        System.Console.WriteLine(nextMsCay)
                    End If

                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed tree code creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(cay As CayDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [CAY] ([ID_CAY], [TENCAY], [ID_LOAICAY], [ID_VITRI], [NGAYTRONG])"
        query &= "VALUES (@ID_CAY,@TENCAY,@ID_LOAICAY,@ID_VITRI,@NGAYTRONG)"

        'get MSHS
        Dim nextMsCay = "1"
        buildMS_Cay(nextMsCay)
        cay.MS_Cay = nextMsCay

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_CAY", cay.MS_Cay)
                    .Parameters.AddWithValue("@TENCAY", cay.TenCay)
                    .Parameters.AddWithValue("@ID_LOAICAY", cay.MS_LoaiCay)
                    .Parameters.AddWithValue("@ID_VITRI", cay.ViTriCay)
                    .Parameters.AddWithValue("@NGAYTRONG", cay.NgayTrong)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More trees failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function selectALL(ByRef listCay As List(Of CayDTO)) As Result

        Dim query As String = String.Empty
        query &= "SELECT [ID_CAY], [TENCAY], [ID_LOAICAY], [ID_VITRI], [NGAYTRONG]"
        query &= "FROM [CAY]"


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
                        listCay.Clear()
                        While reader.Read()
                            listCay.Add(New CayDTO(reader("ID_CAY"), reader("TENCAY"), reader("ID_LOAICAY"), reader("ID_VITRI"), reader("NGAYTRONG")))
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả cay không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL_ByMaChamSoc(iMaCS As Integer, ByRef listCay As List(Of CayDTO)) As Result

        Dim query As String = String.Empty
        query &= " SELECT [ID_CAY], [TENCAY], [ID_LOAICAY], [ID_VITRI], [NGAYTRONG] "
        query &= " FROM [CAY] "
        query &= "     ,[CHAMSOC_CAY] "
        query &= " WHERE "
        query &= "     [CAY].[ID_CAY] = [CHAMSOC_CAY].[ID_CAY]"
        query &= "     AND [CHAMSOC_CAY].[ID_CHAMSOC] = @ID_CHAMSOC"



        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_CHAMSOC", iMaCS)
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    If reader.HasRows = True Then
                        listCay.Clear()
                        While reader.Read()
                            listCay.Add(New CayDTO(reader("ID_CAY"), reader("TENCAY"), reader("ID_LOAICAY"), reader("ID_VITRI"), reader("NGAYTRONG")))
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả cay theo ma cham soc theo Lớp không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL_ByType(maLoaiCay As Integer, ByRef listCay As List(Of CayDTO)) As Result

        Dim query As String = String.Empty
        query &= "SELECT [ID_CAY], [TENCAY], [ID_LOAICAY], [ID_VITRI], [NGAYTRONG] "
        query &= "FROM [CAY] "
        query &= "WHERE [ID_LOAICAY] = @ID_LOAICAY"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_LOAICAY", maLoaiCay)
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    If reader.HasRows = True Then
                        listCay.Clear()
                        While reader.Read()
                            listCay.Add(New CayDTO(reader("ID_CAY"), reader("TENCAY"), reader("ID_LOAICAY"), reader("ID_VITRI"), reader("NGAYTRONG")))
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả cay theo Loại không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function


    Public Function update(cay As CayDTO) As Result

        Dim query As String = String.Empty
        query &= " UPDATE [CAY] SET"
        query &= " [TENCAY] = @TENCAY "
        query &= " ,[ID_LOAICAY] = @ID_LOAICAY "
        query &= " ,[ID_VITRI] = @ID_VITRI "
        query &= " ,[NGAYTRONG] = @NGAYTRONG "
        query &= " WHERE "
        query &= " [ID_CAY] = @ID_CAY "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@TENCAY", cay.TenCay)
                    .Parameters.AddWithValue("@ID_LOAICAY", cay.MS_LoaiCay)
                    .Parameters.AddWithValue("@ID_VITRI", cay.ViTriCay)
                    .Parameters.AddWithValue("@NGAYTRONG", cay.NgayTrong)
                    .Parameters.AddWithValue("@ID_CAY", cay.MS_Cay)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Cập nhật cay không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function


    Public Function delete(maCay As String) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [CAY] "
        query &= " WHERE "
        query &= " [ID_CAY] = @ID_CAY "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_CAY", maCay)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa cay không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)  ' thanh cong
    End Function
End Class
