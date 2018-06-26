Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class PhieuMuaVT_CTDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub
    Public Function buildMS_PhieuCT(ByRef nextID As String) As Result

        nextID = String.Empty
        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [ID_PHIEUMUA_CT] "
        query &= "FROM [PHIEUMUA_VTCT] "
        query &= "ORDER BY [ID_PHIEUMUA_CT] DESC "

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
                            msOnDB = reader("ID_PHIEUMUA_CT")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextID = "PCT1"
                        System.Console.WriteLine(nextID)
                    End If
                    'Neu trong database co du lieu
                    If (msOnDB <> "") Then

                        Dim tmp As String
                        Dim id As Integer
                        tmp = msOnDB.Substring(3)
                        id = Convert.ToInt32(tmp)
                        id += 1
                        If id > 9999999 Then
                            id = 1
                        End If
                        nextID = "PCT" + id.ToString()
                        System.Console.WriteLine(nextID)
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed bill detail code creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(PhieuCT As PhieuMuaVT_CTDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [PHIEUMUA_VTCT] ([ID_PHIEUMUA_CT], [ID_PHIEUMUA_VT], [DIACHIMUA], [SOLUONGMUA], [SOTIEN], [ID_VATTU])"
        query &= "VALUES (@ID_PHIEUMUA_CT,@ID_PHIEUMUA_VT, @DIACHIMUA, @SOLUONGMUA, @SOTIEN, @ID_VATTU)"

        'get MSHS
        Dim nextID = "1"
        buildMS_PhieuCT(nextID)
        PhieuCT.MS_CTPhieu = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_PHIEUMUA_CT", PhieuCT.MS_CTPhieu)
                    .Parameters.AddWithValue("@ID_PHIEUMUA_VT", PhieuCT.MS_Phieu)
                    .Parameters.AddWithValue("@DIACHIMUA", PhieuCT.DiaChi)
                    .Parameters.AddWithValue("@SOLUONGMUA", PhieuCT.SoLuongMua)
                    .Parameters.AddWithValue("@SOTIEN", PhieuCT.SoTien)
                    .Parameters.AddWithValue("@ID_VATTU", PhieuCT.MS_VatTu)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More bill detail failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function update(PhieuCT As PhieuMuaVT_CTDTO) As Result

        Dim query As String = String.Empty
        query &= " UPDATE [PHIEUMUA_VTCT] SET"
        query &= " [ID_PHIEUMUA_VT] = @ID_PHIEUMUA_VT "
        query &= " ,[DIACHIMUA] = @DIACHIMUA "
        query &= " ,[SOLUONGMUA] = @SOLUONGMUA "
        query &= " ,[SOTIEN] = @SOTIEN "
        query &= "WHERE "
        query &= " [ID_PHIEUMUA_CT] = @ID_PHIEUMUA_CT "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_PHIEUMUA_CT", PhieuCT.MS_CTPhieu)
                    .Parameters.AddWithValue("@ID_PHIEUMUA_VT", PhieuCT.MS_Phieu)
                    .Parameters.AddWithValue("@DIACHIMUA", PhieuCT.DiaChi)
                    .Parameters.AddWithValue("@SOLUONGMUA", PhieuCT.SoLuongMua)
                    .Parameters.AddWithValue("@SOTIEN", PhieuCT.SoTien)
                    .Parameters.AddWithValue("@ID_VATTU", PhieuCT.MS_VatTu)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Cập nhật phieu mua vat tu chi tiet không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL(ByRef listPhieuCT As List(Of PhieuMuaVT_CTDTO)) As Result

        Dim query As String = String.Empty
        query &= " SELECT [ID_PHIEUMUA_CT], [ID_PHIEUMUA_VT], [DIACHIMUA], [SOLUONGMUA], [SOTIEN], [ID_VATTU]"
        query &= " FROM [PHIEUMUA_VTCT]"


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
                        listPhieuCT.Clear()
                        While reader.Read()
                            listPhieuCT.Add(New PhieuMuaVT_CTDTO(reader("ID_PHIEUMUA_CT"), reader("ID_PHIEUMUA_VT"), reader("DIACHIMUA"), reader("SOLUONGMUA"), reader("SOTIEN"), reader("ID_VATTU")))
                        End While
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Lấy tất cả phieu mua vat tu chi tiet không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function delete(maPhieuCT As String) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [PHIEUMUA_VTCT] "
        query &= " WHERE "
        query &= " [ID_PHIEUMUA_CT] = @ID_PHIEUMUA_CT "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_PHIEUMUA_CT", maPhieuCT)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa phieu mua vat tu chi tiet không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)  ' thanh cong
    End Function
End Class
