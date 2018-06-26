Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class VatTuDAL
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
        query &= "SELECT TOP 1 [ID_VATTU] "
        query &= "FROM [VATTU] "
        query &= "ORDER BY [ID_VATTU] DESC "

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
                            msOnDB = reader("ID_VATTU")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextID = "SP01"
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
                        nextID = "SP" + id.ToString()
                        System.Console.WriteLine(nextID)
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed supplies code creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(vatt As VatTuDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [VATTU] ([ID_VATTU], [TEN_VATTU], [ID_DONVI])"
        query &= "VALUES (@ID_VATTU,@TEN_VATTU,@ID_DONVI)"

        'get MSHS
        Dim nextID = "1"
        getNextID(nextID)
        vatt.MS_VatTu = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_VATTU", vatt.MS_VatTu)
                    .Parameters.AddWithValue("@TEN_VATTU", vatt.Ten_VatTu)
                    .Parameters.AddWithValue("@ID_DONVI", vatt.MS_DonVi)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More supplies failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function update(vatt As VatTuDTO) As Result

        Dim query As String = String.Empty
        query &= " UPDATE [VATTU] SET"
        query &= " [TEN_VATTU] = @TEN_VATTU "
        query &= " ,[ID_DONVI] = @ID_DONVI "
        query &= "WHERE "
        query &= " [ID_VATTU] = @ID_VATTU "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_VATTU", vatt.MS_VatTu)
                    .Parameters.AddWithValue("@TEN_VATTU", vatt.Ten_VatTu)
                    .Parameters.AddWithValue("@ID_DONVI", vatt.MS_DonVi)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Cập nhật vat tu không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL(ByRef listVatTu As List(Of VatTuDTO)) As Result

        Dim query As String = String.Empty
        query &= " SELECT [ID_VATTU], [TEN_VATTU], [ID_DONVI]"
        query &= " FROM [VATTU]"


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
                        listVatTu.Clear()
                        While reader.Read()
                            listVatTu.Add(New VatTuDTO(reader("ID_DONVI"), reader("TEN_DONVI"), reader("TEN_DONVI")))
                        End While
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Lấy tất cả vat tu không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function delete(mvitri As Integer) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [VATTU] "
        query &= " WHERE "
        query &= " [ID_VATTU] = @ID_VATTU "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_VATTU", mvitri)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Xóa vat tu không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
End Class
