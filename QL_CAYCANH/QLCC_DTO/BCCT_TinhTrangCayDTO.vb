Public Class BCCT_TinhTrangCayDTO
    Private strMaBCCT_TinhTrang As String
    Private strMaBC_TinhTrang As String
    Private strMSCay As String

    Public Sub New()

    End Sub
    Public Sub New(strMaBCCT_TinhTrang As String, strMaBC_TinhTrang As String, strMSCay As String)
        Me.strMaBCCT_TinhTrang = strMaBCCT_TinhTrang
        Me.strMaBC_TinhTrang = strMaBC_TinhTrang
        Me.strMSCay = strMSCay
    End Sub
    Property MS_BCCTTinhTrang() As String
        Get
            Return strMaBCCT_TinhTrang
        End Get
        Set(ByVal Value As String)
            strMaBCCT_TinhTrang = Value
        End Set
    End Property
    Property MS_BCTinhTrang() As String
        Get
            Return strMaBC_TinhTrang
        End Get
        Set(ByVal Value As String)
            strMaBC_TinhTrang = Value
        End Set
    End Property
    Property MS_Cay() As String
        Get
            Return strMSCay
        End Get
        Set(ByVal Value As String)
            strMSCay = Value
        End Set
    End Property
End Class
