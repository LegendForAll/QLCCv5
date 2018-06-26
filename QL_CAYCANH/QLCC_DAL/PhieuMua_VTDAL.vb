Imports System.Configuration
Imports System.Data.SqlClient
Imports QLCC_DTO
Imports Utility

Public Class PhieuMua_VTDAL
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
        query &= "SELECT TOP 1 [ID_PHIEUMUA_VT] "
        query &= "FROM [PHIEUMUA_VT] "
        query &= "ORDER BY [ID_PHIEUMUA_VT] DESC "

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
                            msOnDB = reader("ID_PHIEUMUA_VT")
                        End While
                    End If

                    'Neu trong database rong
                    If (msOnDB = "") Then
                        nextID = "PM01"
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
                        nextID = "PM" + id.ToString()
                        System.Console.WriteLine(nextID)
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Failed bill code creation", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function insert(phieu As PhieuMuaVTDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [PHIEUMUA_VT] ([ID_PHIEUMUA_VT], [NGAYMUAVT])"
        query &= "VALUES (@ID_PHIEUMUA_VT,@NGAYMUAVT)"

        'get MSHS
        Dim nextID = "1"
        getNextID(nextID)
        phieu.MS_Phieu = nextID

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_PHIEUMUA_VT", phieu.MS_Phieu)
                    .Parameters.AddWithValue("@NGAYMUAVT", phieu.TG_Mua)

                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "More bill failed", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function update(phieu As PhieuMuaVTDTO) As Result

        Dim query As String = String.Empty
        query &= " UPDATE [PHIEUMUA_VT] SET"
        query &= " [NGAYMUAVT] = @NGAYMUAVT "
        query &= "WHERE "
        query &= " [ID_PHIEUMUA_VT] = @ID_PHIEUMUA_VT "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_PHIEUMUA_VT", phieu.MS_Phieu)
                    .Parameters.AddWithValue("@NGAYMUAVT", phieu.TG_Mua)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Cập nhật phieu mua không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL(ByRef listPhieu As List(Of PhieuMuaVTDTO)) As Result

        Dim query As String = String.Empty
        query &= " SELECT [ID_PHIEUMUA_VT], [NGAYMUAVT]"
        query &= " FROM [PHIEUMUA_VT]"


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
                        listPhieu.Clear()
                        While reader.Read()
                            listPhieu.Add(New PhieuMuaVTDTO(reader("ID_PHIEUMUA_VT"), reader("NGAYMUAVT")))
                        End While
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    ' them that bai!!!
                    Return New Result(False, "Lấy tất cả phieu mua không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function

    Public Function delete(maPhieu As String) As Result

        Dim query As String = String.Empty
        query &= " DELETE FROM [PHIEUMUA_VT] "
        query &= " WHERE "
        query &= " [ID_PHIEUMUA_VT] = @ID_PHIEUMUA_VT "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@ID_PHIEUMUA_VT", maPhieu)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    Console.WriteLine(ex.StackTrace)
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Xóa phieu mua không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True)  ' thanh cong
    End Function
End Class
